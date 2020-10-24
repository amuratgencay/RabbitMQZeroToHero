using System;

namespace Util.RabbitMQUtils
{

    public abstract class ConsumerBase : QueueBase
    {
        protected bool autoAck = true;
        protected ConsumerBase(string hostName = "localhost") 
            : base(hostName) { }
        public virtual void BasicConsume(EventHandler<string> eventHandler)
        {
            _qm.BasicConsume(eventHandler, autoAck);
        }

        public virtual void BasicConsume(Func<string, string> eventHandler)
        {
            _qm.DeclareRPCConsumer(eventHandler, false);
        }

    }
}
