using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace RPC.FibServerApp
{
    class FibServer
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithQosTypes.RPC,
                "fib"))
            {
                queue.BasicConsume((message)
                    => Fib(message));
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");

            }
        }

        private static string Fib(string message)
        {
            int n = int.Parse(message);
            var response = Fib(n).ToString();
            Console.WriteLine($"fib({n}) = {response}");
            return response;
        }

        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return Fib(n - 1) + Fib(n - 2);
        }
    }
}
