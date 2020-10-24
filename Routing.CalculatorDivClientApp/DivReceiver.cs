using Newtonsoft.Json;
using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace Routing.CalculatorDivClientApp
{
    class DivReceiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithRouteKeyTypes.Direct,
                "calculator",
                routingKey: "div"))
            {
                queue.BasicConsume((sender, message)
                    => Div(message));
                ConsoleExtensions.WriteLineWait("Div\nPress [enter] to exit.");

            }
        }

        private static void Div(string message)
        {
            var numbers = (int[])JsonConvert.DeserializeObject(message, typeof(int[]));
            Console.WriteLine($"{numbers[0]} / {numbers[1]} = {(double)numbers[0] / numbers[1]}");
        }
    }
}
