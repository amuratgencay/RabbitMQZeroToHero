using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Consumer
{
    class FanoutConsumerQueue : ConsumerBase
    {
        private FanoutConsumerQueue(string hostName = "localhost")
            : base(hostName) { }


        public static FanoutConsumerQueue Create(string queueName, string hostName = "localhost")
        {
            var queue = new FanoutConsumerQueue(hostName);
            queue._qm.DeclareExchangeConsumer(queueName, ExchangeType.Fanout);
            return queue;
        }
    }
}
