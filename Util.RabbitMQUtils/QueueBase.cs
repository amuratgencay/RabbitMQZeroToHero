using System;

namespace Util.RabbitMQUtils
{
    public abstract class QueueBase : IDisposable
    {
        protected QueueManager _qm;
        protected QueueBase(string hostName = "localhost")
        {
            _qm = new QueueManager(hostName);
        }
        public void Dispose()
        {
            _qm.Dispose();
        }
    }
}
