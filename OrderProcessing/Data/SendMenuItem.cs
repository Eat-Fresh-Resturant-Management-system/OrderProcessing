/*using System;
using System.Text;
using RabbitMQ.Client;
using OrderProcessing;
using System.Text.Json;
namespace OrderProcessing.Data
{
    
        public class SendMenuItem
        {
        public SendMenuItem()
            { }
            public static void UserOperation(String StringifiedUser)
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest:guest@rabbitmq:5672/")
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                

                channel.ExchangeDeclare(exchange: "maher", type: ExchangeType.Topic);


                var body = Encoding.UTF8.GetBytes(StringifiedUser);
                channel.BasicPublish(exchange: "maher",
                                     routingKey: "message.user",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [x] Sent routingkey: message.user message'{StringifiedUser}'");
             
        }
        }
    }
*/