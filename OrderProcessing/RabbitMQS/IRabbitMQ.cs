using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderProcessing.RabbitMQS
{
    public interface IRabbitMQ
    {


        Task ReceiveTable(IModel channel, string Key, CancellationToken cancellationToken);



    }
}
