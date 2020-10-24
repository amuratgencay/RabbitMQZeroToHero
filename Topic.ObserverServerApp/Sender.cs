using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace Topic.ObserverServerApp
{
    class Sender
    {
        static void Main(string[] args)
        {
            using (var queue = PublisherFactory.Create(
                PublisherWithQueueNameTypes.Topic, 
                "observer"))
            {
                var employee = new Employee(queue, "Elvis", "Presly", 1750m);
                
                employee.FirstName = "George";
                employee.LastName = "Michael";
                employee.Salary = 1875m;

                ConsoleExtensions.WriteLineWait("Press [enter] to exit.");
            }
        }
    }
}
