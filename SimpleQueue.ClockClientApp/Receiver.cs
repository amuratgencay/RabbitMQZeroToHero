using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;
namespace SimpleQueue.ClockClientApp
{
    class Receiver
    {
        public static void Main()
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithQueueNameTypes.Basic,
                queueName: "clock"))
            {
                queue.BasicConsume((sender, message) => ShowClock(message));
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static void ShowClock(string message)
        {
            var now = DateTime.Now.ToBinary();
            var remote = Convert.ToInt64(message);
            var diff = now - remote;

            ConsoleExtensions.WriteLinePos(
                $"Remote:{DateTime.FromBinary(remote)} " +
                $"- Local: {DateTime.FromBinary(now)} " +
                $"- Tick Diff: {TimeSpan.FromTicks(diff)}",
                0, 1);
        }
    }
}


