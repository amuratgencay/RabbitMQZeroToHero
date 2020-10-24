using System;
using Util.ConsoleUtils;
using Newtonsoft.Json;
using Util.RabbitMQUtils.Factory.Enum;
using Util.RabbitMQUtils.Factory;

namespace Routing.CalculatorAddClientApp
{
    class AddReceiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithRouteKeyTypes.Direct,
                "calculator",
                routingKey: "add"))
            {
                queue.BasicConsume((sender, message)
                    => Add(message));
                ConsoleExtensions.WriteLineWait("Add\nPress [enter] to exit.");
            }
        }

        private static void Add(string message)
        {
            var numbers = (int[])JsonConvert.DeserializeObject(message, typeof(int[]));
            Console.WriteLine($"{numbers[0]} + {numbers[1]} = {numbers[0] + numbers[1]}");
        }
    }
}
