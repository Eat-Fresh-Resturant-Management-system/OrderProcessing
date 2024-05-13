
using RabbitMQ.Client;

namespace OrderProcessing.RabbitMQS
{
    public class RabbitMqServicecs : BackgroundService
    {
        private readonly IRabbitMQ _rabbitMQ;
        private  IModel _channel;
        private  IConnection _connection;

        public RabbitMqServicecs(IRabbitMQ rabbitMQ) {


            _rabbitMQ = rabbitMQ;



        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {


            var factory = new ConnectionFactory
            {
               Uri = new Uri("amqp://guest:guest@rabbitmq:5672/"),
              /* HostName = "localhost",
               UserName = "guest",
               Password = "guest",*/

                DispatchConsumersAsync = true,
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await _rabbitMQ.ResTable(_channel, "Table.Booking", stoppingToken);


        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();

            return base.StopAsync(cancellationToken);
        }
    }
}
