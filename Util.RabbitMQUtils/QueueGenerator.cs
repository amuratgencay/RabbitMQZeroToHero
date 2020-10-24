using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Util.RabbitMQUtils
{
    class QueueGenerator
    {
        private IModel _channel;

        public QueueGenerator(IModel channel)
        {
            _channel = channel;
        }

        public string DeclareQueue() =>
                    _channel.QueueDeclare().QueueName;
        public void DeclareQueue(string queueName, bool durable = false) =>
                    _channel.QueueDeclare(queue: queueName,
                                    durable: durable,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        private void DeclareExchange(string exchange, string exchangeType) =>
            _channel.ExchangeDeclare(exchange: exchange, type: exchangeType);

        public void DeclareFanoutExchange(string exchange) => DeclareExchange(exchange, ExchangeType.Fanout);
        public void DeclareTopicExchange(string exchange) => DeclareExchange(exchange, ExchangeType.Topic);
        public void DeclareDirectExchange(string exchange) => DeclareExchange(exchange, ExchangeType.Direct);


        public void BindChannel(string queueName, string exchange, string routingKey) => _channel.QueueBind(queue: queueName,
                             exchange: exchange,
                             routingKey: routingKey);

        public void DeclareBasicQos(uint prefetchSize, ushort prefetchCount) =>
            _channel.BasicQos(prefetchSize: prefetchSize, prefetchCount: prefetchCount, global: false);



        public void BasicPublish(string exchange = "", string routingKey = "", IBasicProperties basicProperties = null, byte[] body = null)
        {
            _channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: basicProperties, body: body);
        }

        public void BasicPublish(string exchange = "", string routingKey = "", IBasicProperties basicProperties = null, string body = "")
        {
            BasicPublish(exchange, routingKey, basicProperties, Encoding.UTF8.GetBytes(body));
        }
        public void BasicPublish(string exchange = "", string routingKey = "", IBasicProperties basicProperties = null, object body = null)
        {
            var reqBody = body != null
                ? (body is string
                    ? Encoding.UTF8.GetBytes(body.ToString())
                    : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body)))
                    : null;
            BasicPublish(exchange, routingKey, basicProperties, reqBody);
        }
        public void BasicConsume(EventingBasicConsumer consumer, string queueName, bool autoAck = true)
        {
            _channel.BasicConsume(
               consumer: consumer,
               queue: queueName,
               autoAck: autoAck);
        }

        public void BasicAck(ulong deliveryTag, bool multiple = false)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: multiple);
        }

    }
}
