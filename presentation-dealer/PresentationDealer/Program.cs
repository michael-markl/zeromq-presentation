using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using NetMQ.WebSockets;

namespace PresentationDealer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            using (var pull = new PullSocket()) // For receiving updates from presentation host
            using (var publisher = new WSPublisher()) // For publishing updates from presentation host to audience
            using (var router = new WSRouter()) { // Handling on-demand requests for late-joining or crashing clients
                pull.Bind("tcp://*:3000");
                publisher.Bind("ws://*:3001");
                router.Bind("ws://*:3002");

                byte step = 0;
                pull.ReceiveReady += (_, __) => {
                    step = pull.ReceiveFrameBytes()[0];
                    Console.Out.WriteLine("Sending " + step + " to audience.");
                    publisher.SendFrame(new [] { step });
                };
                router.ReceiveReady += (_, __) => {
                    var msg = router.ReceiveMultipartMessage();
                    var identity = msg.Pop().Buffer;
                    var request = msg.Pop().ConvertToString();
                    msg.Clear();
                    if (request == "Give me an update!") {
                        router.SendMultipartBytes(identity, new [] {step});
                    } else {
                        router.SendMoreFrame(identity);
                        router.SendFrameEmpty();
                    }
                };

                new NetMQPoller {pull, router}.Run(); // Polling both subscriber and router sockets.
            }
        }
    }
}
