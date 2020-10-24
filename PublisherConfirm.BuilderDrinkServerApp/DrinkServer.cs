using Newtonsoft.Json;
using PublisherConfirm.BuilderModel;
using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils;
using Util.RabbitMQUtils.Consumer;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace PublisherConfirm.BuilderDrinkServerApp
{
    class DrinkServer
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(ConsumerWithQosTypes.RPC, "drink"))
            {
                queue.BasicConsume((message) =>
                {
                    Console.WriteLine("Drink Server generate {0}", message.ToUpper());
                    if (message == "pepsi")
                        return JsonConvert.SerializeObject(new Pepsi());
                    return JsonConvert.SerializeObject(new Coke());
                });
                Console.WriteLine("Waiting for generate drinks.");
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");

            }
        }
    }
}
