using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Publisher
{
    class FanoutPublisherQueue : PublisherBase
    {

        private FanoutPublisherQueue(string hostName = "localhost")
            : base(hostName) { }
        public static FanoutPublisherQueue Create(string queueName, string hostName = "localhost")
        {
            var queue = new FanoutPublisherQueue(hostName);
            queue._qm.DeclareExchangePublisher(queueName, ExchangeType.Fanout);
            return queue;
        }
    }
}
