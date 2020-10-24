using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Consumer
{
    class TopicConsumerQueue : ConsumerBase
    {
        private TopicConsumerQueue(string hostName = "localhost")
            : base(hostName) { }


        public static TopicConsumerQueue Create(string queueName, string hostName = "localhost", string routingKey = "")
        {
            var queue = new TopicConsumerQueue(hostName);
            queue._qm.DeclareExchangeConsumer(queueName, ExchangeType.Topic, routingKey);
            return queue;
        }
    }
}
