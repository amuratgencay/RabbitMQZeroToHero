using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Consumer
{

    class BasicConsumerWithQosQueue : ConsumerBase
    {
        private BasicConsumerWithQosQueue(string hostName = "localhost") 
            : base(hostName) { }

        public static BasicConsumerWithQosQueue Create(
            string queueName,
            string hostName = "localhost",
            uint prefetchSize = 0,
            ushort prefetchCount = 1)
        {
            var queue = new BasicConsumerWithQosQueue(hostName);
            queue._qm.DeclareQueueWithBasicQos(
                queueName, 
                prefetchSize: prefetchSize, 
                prefetchCount: prefetchCount, 
                durable: true);
            queue.autoAck = false;
            return queue;
        }
    }
}
