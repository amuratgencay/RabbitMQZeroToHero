using System;
using System.Collections.Generic;
using System.Text;
using Util.RabbitMQUtils.Factory.Enum;
using Util.RabbitMQUtils.Publisher;

namespace Util.RabbitMQUtils.Factory
{
    public static class PublisherFactory
    {
        public static PublisherBase Create(PublisherWithQueueNameTypes type, string queueName, string hostName = "localhost")
        {
            switch (type)
            {
                case PublisherWithQueueNameTypes.Basic:
                    return BasicPublisherQueue.Create(queueName, hostName);
                case PublisherWithQueueNameTypes.Durable:
                    return BasicPublisherQueue.Create(queueName, hostName, durable:true);
                case PublisherWithQueueNameTypes.Direct:
                    return DirectPublisherQueue.Create(queueName, hostName);
                case PublisherWithQueueNameTypes.Fanout:
                    return FanoutPublisherQueue.Create(queueName, hostName);
                case PublisherWithQueueNameTypes.Topic:
                    return TopicPublisherQueue.Create(queueName, hostName);
                default:
                    throw new ArgumentException("Value not supported.");
            }
        }

        public static PublisherBase Create(PublisherWithoutQueueNameTypes type, string hostName = "localhost")
        {
            switch (type)
            {
                case PublisherWithoutQueueNameTypes.BasicWithConfirms:
                    return BasicPublisherQueueWithConfirms.Create(hostName);
                case PublisherWithoutQueueNameTypes.RPC:
                    return RTCPublisherQueue.Create(hostName);
                default:
                    throw new ArgumentException("Value not supported.");
            }
        }
    }
}
