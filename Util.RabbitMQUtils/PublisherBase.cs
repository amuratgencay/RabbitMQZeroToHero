using System;

namespace Util.RabbitMQUtils
{
    public abstract class PublisherBase : QueueBase
    {
        protected PublisherBase(string hostName = "localhost")
            : base(hostName) { }
        public virtual void IntervalPublish(int interval, Func<string, string> elapsed, Func<string> route = null, EventHandler<string> eventHandler = null)
        {
            _qm.IntervalPublish(interval, elapsed, route, eventHandler);
        }
        public virtual string BasicPublish(string routingKey, object value, EventHandler<string> eventHandler = null)
        {
            _qm.BasicPublish(routingKey, value, eventHandler);
            return routingKey;

        }

    }
}
