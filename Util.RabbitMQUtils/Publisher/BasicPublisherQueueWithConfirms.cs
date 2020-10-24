using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Publisher
{
    class BasicPublisherQueueWithConfirms : PublisherBase
    {
        private BasicPublisherQueueWithConfirms(string hostName = "localhost") 
            : base(hostName) { }

        public override string BasicPublish(string routingKey, object value, EventHandler<string> eventHandler = null)
        {
            return _qm.ConfirmPublish(routingKey, value);
        }

        public static BasicPublisherQueueWithConfirms Create(string hostName = "localhost")
        {
            var queue = new BasicPublisherQueueWithConfirms(hostName);
            queue._qm.DeclareBasicQueueConfirms();
            return queue;
        }
    }
}
