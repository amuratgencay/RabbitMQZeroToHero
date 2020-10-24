using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Publisher
{
    class TopicPublisherQueue : PublisherBase
    {
        private TopicPublisherQueue(string hostName = "localhost")
            : base(hostName) { }
        public static TopicPublisherQueue Create(string queueName, string hostName = "localhost")
        {
            var queue = new TopicPublisherQueue(hostName);
            queue._qm.DeclareExchangePublisher(queueName, ExchangeType.Topic);
            return queue;
        }
    }
}
