using PlatformService.DTOs;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;

            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += _connection_ConnectionShutdown;

                Console.WriteLine($"Connected to Message Bus");

            }
            catch (Exception exp)
            {
                Console.WriteLine($"Could't connect to Message Bus: {exp.Message}");
            }
        }

        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Connection RabbitMQ Shutdown");
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);

            Console.WriteLine("Message sent: " + message);
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus dispose");

            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto model)
        {
            var msg = JsonSerializer.Serialize(model);

            if (_connection.IsOpen)
            {
                Console.WriteLine("Msg Bus connection Open, Sending data");

                SendMessage(msg);
            }
            else
            {
                Console.WriteLine("Msg Bus connection Closed, not sending");
            }
        }
    }
}
