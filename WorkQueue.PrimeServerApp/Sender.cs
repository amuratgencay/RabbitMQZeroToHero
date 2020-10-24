using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace WorkQueue.PrimeServerApp
{
    class Sender
    {
        static void Main(string[] args)
        {
            using (var queue = PublisherFactory.Create(
                PublisherWithQueueNameTypes.Durable, 
                "prime"))
            {
                queue.IntervalPublish(
                    interval: 1000,
                    elapsed: (route) => GenerateRandomNumber());
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static string GenerateRandomNumber()
        {
            var rnd = new Random();
            var number = rnd.Next(10000).ToString();
            Console.WriteLine($"Is {number} prime?");
            return number;
        }
    }
}
