using Newtonsoft.Json;
using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace Routing.CalculatorMulClientApp
{
    class MulReceiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithRouteKeyTypes.Direct, 
                "calculator", 
                routingKey: "mul"))
            {
                queue.BasicConsume((sender, message) 
                    => Mul(message));

                ConsoleExtensions.WriteLineWait("Mul\nPress [enter] to exit.");
            }
        }

        private static void Mul(string message)
        {
            var numbers = (int[])JsonConvert.DeserializeObject(message, typeof(int[]));
            Console.WriteLine($"{numbers[0]} x {numbers[1]} = {numbers[0] * numbers[1]}");
        }
    }
}
