using Newtonsoft.Json;
using PublisherConfirm.BuilderModel;
using System;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Consumer;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace PublisherConfirm.BuilderBurgerServerApp
{
    class BurgerServer
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(ConsumerWithQosTypes.RPC, "burger"))
            {
                queue.BasicConsume((message) =>
                {
                    Console.WriteLine("Burger Server generate {0} burger", message.ToUpper());
                    if (message == "veg")
                        return JsonConvert.SerializeObject(new VegBurger());
                    return JsonConvert.SerializeObject(new ChickenBurger());
                });
                Console.WriteLine("Waiting for generate burgers.");
                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");

            }
        }
    }
}
