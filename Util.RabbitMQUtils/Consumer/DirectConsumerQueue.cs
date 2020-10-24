using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Consumer
{
    class DirectConsumerQueue : ConsumerBase
    {
        private DirectConsumerQueue(string hostName = "localhost") 
            : base(hostName) { }


        public static DirectConsumerQueue Create(string queueName, string hostName = "localhost", string routingKey = "")
        {
            var queue = new DirectConsumerQueue(hostName);
            queue._qm.DeclareExchangeConsumer(queueName, ExchangeType.Direct, routingKey);
            return queue;
        }
    }
}
