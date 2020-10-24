using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace RPC.SingletonServerApp
{
    class SingletonServer
    {
        private static int _counter = 0;
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithQosTypes.RPC, 
                "singleton"))
            {
                queue.BasicConsume(
                     (message)
                     => HandleRequest(message));
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static string HandleRequest(string message)
        {
            Console.Write("request: {0} - current value: {1} ", message, _counter);

            switch (message)
            {
                case "inc": _counter++; break;
                case "dec": _counter--; break;
            }
            Console.WriteLine("new value: {0}", _counter);

            return _counter.ToString();
        }
    }
}
