using RabbitMQ.Client;
using System.Text;

namespace Lancamentos.Services
{
    public class ProducerService
    {
        private readonly string _hostname = "localhost";
        private readonly int _port = 5672;
        private readonly string _queueName = "Lancamentos";
        private readonly string _userName = "guest";
        private readonly string _password = "guest";
        private readonly string _virtualHost = "/";
        private IConnection _connection;

        public ProducerService()
        {
            CreateConnection();
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory() { 
                HostName = _hostname,
                Port = _port,
                UserName = _userName,
                Password = _password,
                VirtualHost = _virtualHost

            };
            _connection = factory.CreateConnection();
        }

        public void EnviarMensagem(string mensagem)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(mensagem);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
