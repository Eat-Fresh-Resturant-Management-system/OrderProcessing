using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrderProcessing.Data;
using OrderProcessing.Models;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderProcessing.RabbitMQS
{
    public class RabbitMQUnti : IRabbitMQ
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Order_Db _context;

        public RabbitMQUnti(Order_Db context, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _context = context;

        }

        public async Task ResTable(IModel channel, string rout, CancellationToken cancellationToken)
        {
            try
            {
                channel.ExchangeDeclare(exchange: "table_booking_exchange", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                        exchange: "table_booking_exchange",
                        routingKey: "Table.Booking");


                var con = new AsyncEventingBasicConsumer(channel);
                con.Received += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        string[] messageparts = message.Split('-');

                        // Log modtaget besked
                        Console.WriteLine($"Received message: {message}");

                        if (int.TryParse(messageparts[0], out int tabelId))
                        {
                            var putuser = await _context.TableDatas.FindAsync(tabelId);
                            if (putuser != null)
                            {
                                if (putuser.IsAvailable != null)
                                {
                                    putuser.IsAvailable = messageparts[1] ?? putuser.IsAvailable;
                                }
                                else
                                {
                                    putuser.IsAvailable = messageparts[1];
                                }

                                await _context.SaveChangesAsync(cancellationToken);

                                Console.WriteLine($"Update succeeded for ID: {messageparts[0]}, New Status: {messageparts[1]}");
                            }
                            else
                            {
                                Console.WriteLine($"Tabel with ID {messageparts[0]} not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid ID format: {messageparts[0]}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                    }
                };

                channel.BasicConsume(queue: queueName,autoAck: true, consumer: con);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Feil under behandling av meldinger: {ex.Message}");
            }
        }


    }
}
