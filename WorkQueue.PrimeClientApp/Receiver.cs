using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace WorkQueue.PrimeClientApp
{
    class Receiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithQosTypes.BasicWithQos,
                "prime"))
            {
                queue.BasicConsume((sender, message)
                    => IsPrime(message));
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }

        private static void IsPrime(string message)
        {
            var number = Convert.ToInt32(message);
            bool isPrime = true;
            if (number < 2) isPrime = false;
            else if (number == 2) isPrime = true;
            else
            {
                for (int i = 2; i <= number / 2 + 1; i++)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
            }
            Console.WriteLine($"{number} is {(isPrime ? "prime" : "not prime")}");
        }
    }
}
