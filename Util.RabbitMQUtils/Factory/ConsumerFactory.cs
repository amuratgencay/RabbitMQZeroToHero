using System;
using System.Collections.Generic;
using System.Text;
using Util.RabbitMQUtils.Consumer;
using Util.RabbitMQUtils.Factory.Enum;
using Util.RabbitMQUtils.Publisher;

namespace Util.RabbitMQUtils.Factory
{
    public static class ConsumerFactory
    {
        public static ConsumerBase Create(
            ConsumerWithQueueNameTypes type, 
            string queueName, 
            string hostName = "localhost")
        {
            switch (type)
            {
                case ConsumerWithQueueNameTypes.Basic:
                    return BasicConsumerQueue.Create(queueName, hostName);
                case ConsumerWithQueueNameTypes.Fanout:
                    return FanoutConsumerQueue.Create(queueName, hostName);
                default:
                    throw new ArgumentException("Value not supported.");
            }
        }

        public static ConsumerBase Create(
            ConsumerWithRouteKeyTypes type,
            string queueName,
            string hostName = "localhost",
            string routingKey = "")
        {
            switch (type)
            {
                case ConsumerWithRouteKeyTypes.Direct:
                    return DirectConsumerQueue.Create(queueName, hostName, routingKey);
                case ConsumerWithRouteKeyTypes.Topic:
                    return TopicConsumerQueue.Create(queueName, hostName, routingKey);
                default:
                    throw new ArgumentException("Value not supported.");

            }
        }

        public static ConsumerBase Create(
            ConsumerWithQosTypes type,
            string queueName,
            string hostName = "localhost",
            uint prefetchSize = 0,
            ushort prefetchCount = 1)
        {
            switch (type)
            {
                case ConsumerWithQosTypes.BasicWithQos:
                    return BasicConsumerWithQosQueue.Create(queueName, hostName, prefetchSize, prefetchCount);
                case ConsumerWithQosTypes.RPC:
                    return RTCConsumerQueue.Create(queueName, hostName, prefetchSize, prefetchCount);
                default:
                    throw new ArgumentException("Value not supported.");
            }
        }
    }
}
