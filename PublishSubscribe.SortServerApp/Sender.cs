using Newtonsoft.Json;
using System;
using System.Linq;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;
using Util.StringUtils;

namespace PublishSubscribe.SortServerApp
{
    class Sender
    {
        static void Main(string[] args)
        {
            using (var queue = PublisherFactory.Create(
                PublisherWithQueueNameTypes.Fanout,
                "sort"))
            {
                queue.IntervalPublish(
                    interval: 1000,
                    elapsed: (route) =>GenerateRandomArray());
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static string GenerateRandomArray(int min = 0, int max = 1000)
        {
            Random rnd = new Random();
            var arr = Enumerable
                .Repeat(0, 5)
                .Select(i => rnd.Next(min, max))
                .ToArray();

            var serializedArr = JsonConvert.SerializeObject(arr);
            Console.WriteLine(StringExtentions.FromIntArray(arr));
            return serializedArr;
        }

       
    }
}
