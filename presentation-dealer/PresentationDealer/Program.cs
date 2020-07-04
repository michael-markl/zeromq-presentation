using System;
using System.Threading;
using NetMQ;
using NetMQ.WebSockets;

namespace PresentationDealer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            using (var publisher = new WSPublisher())
            {
                publisher.Bind("ws://localhost:3001");
                while (true)
                {
                    Console.Out.WriteLine("Sending...");
                    publisher.SendFrame("Sample");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
