using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace Topic.ObserverClientApp
{
    class ObserverReceiver
    {
        static void Main(string[] args)
        {
            var bindingKey = args.Length > 0 
                ? args[0] 
                : "#";

            using (var queue = ConsumerFactory.Create(
                ConsumerWithRouteKeyTypes.Topic, 
                "observer", 
                routingKey: bindingKey))
            {
                queue.BasicConsume((sender, message) =>
                {
                    Console.WriteLine($"{bindingKey} changed {message}");
                });
                Console.WriteLine("Listening: {0}", bindingKey);
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }
    }
}
