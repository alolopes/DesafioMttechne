using Lancamentos.Domain.Entities;
using Lancamentos.Infrastructure.Data;
using Lancamentos.Services;
using Moq;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Xunit;

public class ConsumerServiceTests
{
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly Mock<IServiceScope> _mockScope;
    private readonly Mock<ApplicationDbContext> _mockDbContext;

    public ConsumerServiceTests()
    {
        // Mock para o ApplicationDbContext
        _mockDbContext = new Mock<ApplicationDbContext>();

        // Mock para o IServiceScope
        _mockScope = new Mock<IServiceScope>();
        _mockScope.Setup(x => x.ServiceProvider.GetService(typeof(ApplicationDbContext)))
                  .Returns(_mockDbContext.Object);

        // Mock para o IServiceScopeFactory
        _mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockScopeFactory.Setup(x => x.CreateScope()).Returns(_mockScope.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ProcessesMessageAndSavesToDatabase()
    {
        // Arrange
        var consumerService = new ConsumerService(_mockScopeFactory.Object);

        // Simula uma mensagem de RabbitMQ
        var lancamento = new Lancamento
        {
            Id = 1,
            Data = DateTime.Now,
            Descricao = "Teste de Lançamento",
            Valor = 1250,
            Tipo = 1

        };
        var mensagemJson = JsonSerializer.Serialize(lancamento);
        var body = Encoding.UTF8.GetBytes(mensagemJson);

        var ea = new BasicDeliverEventArgs { Body = body };

        // Simula o método Received do RabbitMQ Consumer
        var mockConsumer = new Mock<EventingBasicConsumer>(null);
        mockConsumer.Raise(m => m.Received += null, null, ea);

        // Act
        //await consumerService.ExecuteAsyncPublic(new CancellationToken());

        // Assert
        _mockDbContext.Verify(db => db.Lancamentos.Add(It.Is<Lancamento>(
            l => 
            l.Descricao == "Teste de Lançamento" &
            l.Data == DateTime.Now &
            l.Descricao == "Teste de Lançamento" &
            l.Valor == 1250 &
            l.Tipo == 1

            )), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
