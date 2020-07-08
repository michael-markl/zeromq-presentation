using System;
using NetMQ;
using NetMQ.Sockets;
using NetMQ.WebSockets;

namespace PresentationDealer {
    static class Program {
        private static void Main(string[] args) {
            var deciSecond = new TimeSpan(10000);
            
            using (var subscriber = new SubscriberSocket()) // For receiving updates from presentation host
            using (var publisher = new WSPublisher()) // For publishing updates from presentation host to audience
            using (var router = new WSRouter()) // Handling on-demand requests for late-joining or crashing clients
            {
                subscriber.Bind("tcp://*:3000");
                subscriber.SubscribeToAnyTopic();
                publisher.Bind("ws://*:3001");
                router.Bind("ws://*:3002");

                byte step = 0;
                subscriber.ReceiveReady += (_, __) => {
                    if (!subscriber.TryReceiveFrameBytes(deciSecond, out var received)) return;
                    step = received[0];
                    Console.Out.WriteLine("Sending " + step + " to audience.");
                    publisher.TrySendFrame(deciSecond, new[] {step});
                };
                router.ReceiveReady += (_, __) => {
                    NetMQMessage msg = null;
                    if (!router.TryReceiveMultipartMessage(deciSecond, ref msg)) return;
                    var identity = msg.Pop().Buffer;
                    var request = msg.Pop().ConvertToString();
                    msg.Clear();
                    if (request == "Which slide are we on?")
                        router.TrySendMultipartBytes(deciSecond, identity, new[] {step});
                    else {
                        if (!router.TrySendFrame(deciSecond, identity, true)) return;
                        router.TrySendFrameEmpty(deciSecond);
                    }
                };
                new NetMQPoller {subscriber, router}.Run(); // Polling both subscriber and router sockets.
            }
        }
    }
}