using System;
using System.Text;
using System.Threading;
using NetMQ;
using NetMQ.WebSockets;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = NetMQContext.Create())
            {
                using (WSPublisher publisher = context.CreateWSPublisher())
                {
                    
                    publisher.Bind("ws://localhost:81");
                    while (true)
                    {
                        publisher.SendMore("chat").Send("asdf");
                        Thread.Sleep(2000);
                    }
                }
            }
        }
    }
}