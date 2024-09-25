
namespace Lancamentos.Domain.Entities
{
    public class Lancamento
    {
        public int Id { get; set; }
        public required DateTime Data { get; set; }
        public required string Descricao { get; set; }
        public required decimal Valor { get; set; }
        public required int Tipo { get; set; }
    }
}
    