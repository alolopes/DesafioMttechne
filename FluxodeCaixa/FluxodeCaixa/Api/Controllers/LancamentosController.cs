
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluxoDeCaixa.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LancamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetLancamentos()
        {
            return await _context.Lancamentos.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Lancamento>> GetLancamento(int id)
        {
            return await _context.Lancamentos.FindAsync(id);
        }


        [HttpGet("{data}")]
        public async Task<ActionResult<IEnumerable<Lancamento>>> GetLancamentos(DateTime data)
        {
            return await _context.Lancamentos.Where(l => l.Data.Date.Equals(data)).ToListAsync();
        }

        [HttpGet("{Data}/Consolidar")]
        public async Task<List<Consolidacao>> Consolidar(DateTime Data)
        {
            var totalSoma = await _context.Lancamentos
                .GroupBy(l => l.Tipo) 
                .Select(g => new Consolidacao
                {
                    Tipo = g.Key,          
                    Total = g.Sum(l => l.Valor)
                })
                .ToListAsync();

            return totalSoma;
        }

    }
}
    