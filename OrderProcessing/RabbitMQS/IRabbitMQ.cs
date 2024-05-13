using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderProcessing.RabbitMQS
{
    public interface IRabbitMQ
    {


        Task ResTable(IModel channel, string rout, CancellationToken cancellationToken);



    }
}
