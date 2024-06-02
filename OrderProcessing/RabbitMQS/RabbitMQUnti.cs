
using System.Text;
using Microsoft.EntityFrameworkCore;
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
        public async Task ReceiveTable(IModel channel, string Key, CancellationToken cancellationToken)
        {
            try
            {
                channel.ExchangeDeclare(exchange: "table_booking_exchange", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                        exchange: "table_booking_exchange",
                        routingKey: "table_booking_queue");


                var con = new AsyncEventingBasicConsumer(channel);
                con.Received += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine($"Received message: {message}");
                        var tablebooking = await _context.TableDatas.FirstOrDefaultAsync(t => t.Name == message);                      
                           if (tablebooking == null)
                            {
                            TableData newtable = new TableData();
                            newtable.Name = message;
                            newtable.IsAvailable = "false";
                            await _context.TableDatas.AddAsync(newtable);
                            await _context.SaveChangesAsync();
                            Console.WriteLine($"Added succeeded for: {message}, New Status: {newtable.IsAvailable}");
                            }
                        else
                            {
                            tablebooking.IsAvailable = "false"; 
                            await _context.SaveChangesAsync(cancellationToken);
                            Console.WriteLine($"Updated succeeded for: {message}, New Status: {tablebooking.IsAvailable}");
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
