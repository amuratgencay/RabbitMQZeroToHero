using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Publisher
{
    class DirectPublisherQueue : PublisherBase
    {
        private DirectPublisherQueue(string hostName = "localhost")
            : base(hostName) { }
        public static DirectPublisherQueue Create(string queueName, string hostName = "localhost")
        {
            var queue = new DirectPublisherQueue(hostName);
            queue._qm.DeclareExchangePublisher(queueName, ExchangeType.Direct);
            return queue;
        }

    }
}
