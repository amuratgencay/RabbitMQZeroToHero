using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace Util.RabbitMQUtils
{
    public class QueueManager : IDisposable
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private QueueGenerator _queueGenerator;


        private IModel _channel;
        private string _queueName = "";
        private string _exchange = "";
        private System.Timers.Timer _timer;
        private Func<string, string> _elapsed;
        private EventHandler<string> _eventHandler;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private EventingBasicConsumer _consumer;
        private Func<string> _route;
        private IBasicProperties _properties;
        ConcurrentDictionary<ulong, string> outstandingConfirms = new ConcurrentDictionary<ulong, string>();
        private bool _rtcMode = false;

        public QueueManager(string hostName = "localhost")
        {
            _connectionFactory = new ConnectionFactory() { HostName = hostName };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueGenerator = new QueueGenerator(_channel);
        }



        public string ConfirmPublish(string routingKey, object message)
        {
            outstandingConfirms.TryAdd(_channel.NextPublishSeqNo, message.ToString());
            _queueGenerator.BasicPublish(exchange: _exchange, routingKey: routingKey, basicProperties: _properties, body: message);
            _queueGenerator.BasicConsume(_consumer, _queueName);
            if (!WaitUntil(60, () => outstandingConfirms.IsEmpty))
                throw new Exception("All messages could not be confirmed in 60 seconds");

            return respQueue.Take();
        }

        public void DeclareRPCConsumer(Func<string, string> eventHandler, bool autoAck = true)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var response = eventHandler(message);
                _queueGenerator.BasicPublish(exchange: _exchange, routingKey: props.ReplyTo, basicProperties: replyProps, body: response);

                if (!autoAck)
                {
                    _queueGenerator.BasicAck(ea.DeliveryTag);
                }
            };
            _queueGenerator.BasicConsume(consumer, _queueName, autoAck);
        }


        #region consume


        public void BasicConsume(EventHandler<string> eventHandler, bool autoAck = true)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                eventHandler.Invoke(this, message);
                if (!autoAck)
                {
                    _queueGenerator.BasicAck(ea.DeliveryTag);
                }
            };
            _queueGenerator.BasicConsume(consumer, _queueName, autoAck);
        }
        #endregion

        #region publish
        public void BasicPublish(string routingKey, object value, EventHandler<string> eventHandler = null)
        {
            _eventHandler = eventHandler;
            _rtcMode = _eventHandler != null;
            _queueGenerator.BasicPublish(exchange: _exchange, routingKey: routingKey, basicProperties: _properties, body: value);
            if (_rtcMode)
            {
                _queueGenerator.BasicConsume(_consumer, _queueName);
                _eventHandler(this, respQueue.Take());
            }
        }

        public void IntervalPublish(int interval, Func<string, string> elapsed, Func<string> route = null, EventHandler<string> eventHandler = null)
        {
            _eventHandler = eventHandler;
            _rtcMode = _eventHandler != null;
            _elapsed = elapsed;
            _route = route != null ? route : (() => _queueName);
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var routingKey = _route();
            var body = _elapsed(routingKey);
            _queueGenerator.BasicPublish(exchange: _exchange, routingKey: routingKey, basicProperties: _properties, body: body);
            if (_rtcMode)
            {
                _queueGenerator.BasicConsume(_consumer, _queueName);
                _eventHandler(this, respQueue.Take());
            }
        }
        #endregion


        #region queue

        public void DeclareBasicQueue(string queueName, bool durable = false)
        {
            _queueName = queueName;
            _queueGenerator.DeclareQueue(queueName, durable);
        }
        public void DeclareQueueWithBasicQos(string queueName, uint prefetchSize = 0, ushort prefetchCount = 1, bool durable = false)
        {
            DeclareBasicQueue(queueName, durable);
            _queueGenerator.DeclareBasicQos(prefetchSize, prefetchCount);
        }
        public void DeclareDurableQueue(string queueName)
        {
            DeclareBasicQueue(queueName, true);
            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true;
        }
        private void DeclareCallBackQueue(bool confirms = false)
        {
            _queueName = _queueGenerator.DeclareQueue();
            if (confirms)
                _channel.ConfirmSelect();

            _consumer = new EventingBasicConsumer(_channel);
            _properties = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            _properties.CorrelationId = correlationId;
            _properties.ReplyTo = _queueName;
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };
        }
        public void DeclareRTCQueue()
        {
            DeclareCallBackQueue();
        }
        public void DeclareBasicQueueConfirms()
        {
            DeclareCallBackQueue(confirms: true);

            void cleanOutstandingConfirms(ulong sequenceNumber, bool multiple)
            {
                if (multiple)
                {
                    var confirmed = outstandingConfirms.Where(k => k.Key <= sequenceNumber);
                    foreach (var entry in confirmed)
                        outstandingConfirms.TryRemove(entry.Key, out _);
                }
                else
                    outstandingConfirms.TryRemove(sequenceNumber, out _);
            }

            _channel.BasicAcks += (sender, ea) => cleanOutstandingConfirms(ea.DeliveryTag, ea.Multiple);
            _channel.BasicNacks += (sender, ea) =>
            {
                outstandingConfirms.TryGetValue(ea.DeliveryTag, out string body);
                Console.WriteLine($"Message with body {body} has been nack-ed. Sequence number: {ea.DeliveryTag}, multiple: {ea.Multiple}");
                cleanOutstandingConfirms(ea.DeliveryTag, ea.Multiple);
            };
        }
        #endregion

        #region exchange
        public void DeclareExchangePublisher(string exchange, string type)
        {
            _exchange = exchange;
            switch (type)
            {
                case ExchangeType.Direct: _queueGenerator.DeclareDirectExchange(exchange); break;
                case ExchangeType.Fanout: _queueGenerator.DeclareFanoutExchange(exchange); break;
                case ExchangeType.Topic: _queueGenerator.DeclareTopicExchange(exchange); break;
            }
        }

        public void DeclareExchangeConsumer(string exchange, string type, string routingKey = "")
        {
            DeclareExchangePublisher(exchange, type);
            _queueName = _queueGenerator.DeclareQueue();
            _queueGenerator.BindChannel(_queueName, exchange, routingKey);

        }
        #endregion

        public void Dispose()
        {
            _timer?.Stop();
            _channel?.Dispose();
            _connection?.Dispose();
        }
        public bool WaitUntil(int numberOfSeconds, Func<bool> condition)
        {
            int waited = 0;
            while (!condition() && waited < numberOfSeconds * 1000)
            {
                Thread.Sleep(100);
                waited += 100;
            }

            return condition();
        }


    }
}
