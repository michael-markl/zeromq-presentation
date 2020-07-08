using System;
using NetMQ;
using NetMQ.Sockets;
using NetMQ.WebSockets;

namespace PresentationDealer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            using (var pull = new PullSocket()) // For pulling updates from presentation host
            using (var publisher = new WSPublisher()) // For publishing updates from presentation host to audience
            using (var router = new WSRouter()) // Handling on-demand requests for late-joining or crashing clients
            {
                pull.Bind("tcp://*:3000");
                publisher.Bind("ws://*:3001");
                router.Bind("ws://*:3002");
                
                byte step = 0;
                
                pull.ReceiveReady += (_, __) => {
                    if (!pull.TryReceiveFrameBytes(out var received)) return;
                    step = received[0];
                    Console.Out.WriteLine("Sending " + step + " to audience.");
                    publisher.TrySendFrame(new[] {step});
                };
                router.ReceiveReady += (_, __) => {
                    NetMQMessage msg = null;
                    if (!router.TryReceiveMultipartMessage(ref msg)) return;
                    var identity = msg.Pop().Buffer;
                    var request = msg.Pop().ConvertToString();
                    msg.Clear();
                    if (request == "Give me an update!")
                        router.TrySendMultipartBytes(identity, new[] {step});
                    else {
                        if (!router.TrySendFrame(identity, true)) return;
                        router.TrySendFrameEmpty();
                    }
                };
                new NetMQPoller {pull, router}.Run(); // Polling both subscriber and router sockets.
            }
        }
    }
}
