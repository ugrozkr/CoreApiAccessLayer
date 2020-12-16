using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wall.Integration.MessagingBus
{
    public class EventBusRabbitMQ<T> : IDisposable where T : class
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;
        private string _queueName;
        T _service;

        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, T service, string queueName = null)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _service = service;
            _queueName = queueName;
        }

        public IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;
            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };
            return channel;
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "userInsertMsgQ")
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                List<object> inputObj = JsonConvert.DeserializeObject<List<object>>(message);
                var declaredPublicInstanceMethods = _service.GetType()
                         .GetMethods()
                         .Where(m => m.Name == "CustomTransaction")
                         .Select(m => new {
                             Method = m,
                             Params = m.GetParameters(),
                             Args = m.GetGenericArguments()
                         })
                         .Where(x => x.Params.Length == 1
                                     && x.Args.Length == 1
                                     && x.Params[0].ParameterType == x.Args[0])
                         .Select(x => x.Method)
                         .First();
                object saveFeedback = declaredPublicInstanceMethods.Invoke(obj: null, parameters: new object[] { inputObj }); ;
                PublishUserSaveFeedback("userInsertMsgQ_feedback", saveFeedback, e.BasicProperties.Headers);
            }
        }

        public void PublishUserSaveFeedback(string _queueName, object publishModel, IDictionary<string, object> headers)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                properties.Headers = headers;
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();
                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("RabbitMQ server'a gönderildi");
                    //Acknowledge işlemleri isteğe bağlı burada yapılabilir masstransit ESB tarafında Ack ve diğer tüm Queue pipeline süreçleri detaylı ele alınmaktadır.
                };
                channel.ConfirmSelect();
            }
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }
    }
}
