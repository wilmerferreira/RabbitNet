using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitNet.Introduction.ConsoleReceiver
{
    class Program
    {
        private static string RABBITMQ_HOSTNAME = "localhost";

        static void Main(string[] args)
        {
            Console.Title = "RabbitMQ: Receiver";

            var factory = new ConnectionFactory
            {
                HostName = RABBITMQ_HOSTNAME
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queue = "mytestqueue";

                    channel.QueueDeclare
                    (
                        queue: queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine("Message received from [{0}/{1}]: {2}", RABBITMQ_HOSTNAME, queue, message);
                    };

                    channel.BasicConsume
                    (
                        queue: queue,
                        autoAck: true,
                        consumer: consumer
                    );

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}