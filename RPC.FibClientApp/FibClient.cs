using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace RPC.FibClientApp
{
    class FibClient
    {
        static void Main(string[] args)
        {
            using (var queue = PublisherFactory.Create(PublisherWithoutQueueNameTypes.RPC))
            {
                queue.IntervalPublish(
                   interval: 3000,
                   route: () => "fib",
                   elapsed: (route) 
                        => GenerateNumber(),
                   eventHandler: (sender, response) 
                        => ShowResponse(response)
                   );
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static void ShowResponse(string response)
        {
            Console.WriteLine("Response: '{0}'", response);
        }

        private static string GenerateNumber()
        {
            var number = new Random().Next(1, 20).ToString();
            Console.WriteLine($"Fib({number})");
            return number;
        }
    }
}
