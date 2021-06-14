using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Models.ViewModels;
using System;
using System.Text;

namespace TransactionAPI.Messaging.Send
{
    internal class Program
    {
        private static IConnection _connection;
        private static readonly string _queueName = "TransactionQueue";
        private static readonly string _hostname = "localhost";
        private static readonly string _username = "guest";
        private static readonly string _password = "guest";

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SendCustomer(new PostTransaction
            {
                Amount = 50,
                Description = "sent from q",
                PostDate = DateTimeOffset.UtcNow,
                TransactionType = Shared.Helpers.Enums.TransactionTypes.CREDIT,
                TransactionCategoryId = Guid.Parse("9a9e48e7-7cb7-4dcf-b7b9-afec6e38aa45")
            });
        }

        public static void SendCustomer(PostTransaction postTransaction)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(postTransaction);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
        }

        private static void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password,
                    Port = 5672
                };
                _connection = factory.CreateConnection();
                var c = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private static bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}