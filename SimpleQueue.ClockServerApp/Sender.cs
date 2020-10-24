using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace SimpleQueue.ClockServerApp
{
    class Sender
    {
        public static void Main()
        {
            using (var queue = PublisherFactory.Create(
                PublisherWithQueueNameTypes.Basic,
                queueName: "clock"))
            {
                queue.IntervalPublish(
                    interval: 1000,
                    elapsed: (route) => GetClock());
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static string GetClock()
        {
            var now = DateTime.Now;
            var message = now.ToBinary().ToString();
            ConsoleExtensions.WriteLinePos(now.ToString(), 0, 1);
            return message;
        }
    }
}

