using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitNet.Introduction.ConsoleSender
{
    class Program
    {
        private static string RABBITMQ_HOSTNAME = "localhost";

        static void Main(string[] args)
        {
            Console.Title = "RabbitMQ: Sender";

            var factory = new ConnectionFactory
            {
                HostName = RABBITMQ_HOSTNAME
            };

            using (var connection = factory.CreateConnection())
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

                for (var i = 0; i < 10; i++)
                {
                    var body = Encoding.UTF8.GetBytes(DateTime.Now.ToLongTimeString());

                    channel.BasicPublish
                    (
                        exchange: "",
                        routingKey: queue,
                        basicProperties: null,
                        body: body
                    );

                    Console.WriteLine("Message sent to [{0}/{1}]", RABBITMQ_HOSTNAME, queue);

                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}