using System;
using System.Collections.Generic;
using System.Text;

namespace Util.RabbitMQUtils.Publisher
{
    class RTCPublisherQueue : PublisherBase
    {
        private RTCPublisherQueue(string hostName = "localhost")
            : base(hostName) { }

        public static RTCPublisherQueue Create(string hostName = "localhost")
        {
            var queue = new RTCPublisherQueue(hostName);
            queue._qm.DeclareRTCQueue();
            return queue;
        }
    }
}
