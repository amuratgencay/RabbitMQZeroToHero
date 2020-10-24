using Newtonsoft.Json;
using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace Routing.CalculatorServerApp
{
    class Sender
    {
        private static readonly string[] operations = new string[] { "add", "sub", "mul", "div" };
        static void Main(string[] args)
        {
            using (var queue = PublisherFactory.Create(
                PublisherWithQueueNameTypes.Direct, 
                "calculator"))
            {
                queue.IntervalPublish(
                    interval: 1000,
                    elapsed: (route) => GenerateNumbers(route),
                    route: () => Route);

                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static string Route 
            => operations[new Random().Next(operations.Length)];
        

        private static string GenerateNumbers(string route)
        {
            var rnd = new Random();
            var arr = new int[] { rnd.Next(1, 100), rnd.Next(1, 100) };
            var serializedArr = JsonConvert.SerializeObject(arr);
            Console.WriteLine($"{route} {serializedArr}");
            return serializedArr;
        }
    }
}
