using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Publisher
{
    class BasicPublisherQueue : PublisherBase
    {
        private BasicPublisherQueue(string hostName = "localhost")
            : base(hostName) { }

        public static BasicPublisherQueue Create(string queueName, string hostName = "localhost", bool durable = false)
        {
            var queue = new BasicPublisherQueue(hostName);
            queue._qm.DeclareBasicQueue(queueName, durable);
            return queue;
        }

    }
}
