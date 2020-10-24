using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Consumer
{
    class BasicConsumerQueue : ConsumerBase
    {
        private BasicConsumerQueue(string hostName = "localhost")
            : base(hostName) { }


        public static BasicConsumerQueue Create(string queueName, string hostName = "localhost")
        {
            var queue = new BasicConsumerQueue(hostName);
            queue._qm.DeclareBasicQueue(queueName);
            return queue;
        }
    }
}
