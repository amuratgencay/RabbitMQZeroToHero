using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Consumer
{
    class RTCConsumerQueue : ConsumerBase
    {
        private RTCConsumerQueue(string hostName = "localhost")
            : base(hostName) { }

        public static RTCConsumerQueue Create(string queueName, string hostName = "localhost", uint prefetchSize = 0, ushort prefetchCount = 1)
        {
            var queue = new RTCConsumerQueue(hostName);
            queue._qm.DeclareQueueWithBasicQos(
                queueName,
                prefetchSize: prefetchSize,
                prefetchCount:
                prefetchCount,
                durable: true);
            queue.autoAck = false;
            return queue;
        }
    }
}
