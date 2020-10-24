using Newtonsoft.Json;
using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace Routing.CalculatorSubClientApp
{
    class SubReceiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithRouteKeyTypes.Direct, 
                "calculator", 
                routingKey: "sub"))
            {
                queue.BasicConsume((sender, message) 
                    => Sub(message));
                ConsoleExtensions.WriteLineWait("Sub\nPress [enter] to exit.");
            }

        }

        private static void Sub(string message)
        {
            var numbers = (int[])JsonConvert.DeserializeObject(message, typeof(int[]));
            Console.WriteLine($"{numbers[0]} - {numbers[1]} = {numbers[0] - numbers[1]}");
        }
    }
}
