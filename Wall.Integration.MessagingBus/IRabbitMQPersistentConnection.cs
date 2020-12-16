using RabbitMQ.Client;
using System;

namespace Wall.Integration.MessagingBus
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
