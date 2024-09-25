using Lancamentos.Domain.Entities;
using Lancamentos.Infrastructure.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Lancamentos.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _hostname = "localhost";
        private readonly int _port = 5672;
        private readonly string _queueName = "Lancamentos";
        private readonly string _userName = "guest";
        private readonly string _password = "guest";
        private readonly string _virtualHost = "/";
        private IConnection _connection;
        private IModel _channel;

        public ConsumerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                Port = _port,
                UserName = _userName,
                Password = _password,
                VirtualHost = _virtualHost
            };
            _connection = factory.CreateConnection();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensagem = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensagem recebida: {mensagem}");

                // Cria um escopo para obter o DbContext
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    try
                    {
                        // Desserializa e adiciona o lançamento ao contexto
                        var lancamento = JsonSerializer.Deserialize<Lancamento>(mensagem);
                        if (lancamento != null)
                        {
                            _context.Lancamentos.Add(lancamento);
                            await _context.SaveChangesAsync(stoppingToken);  // Await o método SaveChangesAsync
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Erro ao desserializar a mensagem: {jsonEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                    }
                }
            };

            // Consome as mensagens da fila
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            // Mantém o serviço rodando até que o token de cancelamento seja acionado
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Aguarda um segundo antes de checar novamente
            }
        }

        //public async Task ExecuteAsyncPublic(CancellationToken stoppingToken)
        //{
        //    await ExecuteAsync(stoppingToken);
        //}

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
