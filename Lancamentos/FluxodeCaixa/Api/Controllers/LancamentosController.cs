
using Lancamentos.Domain.Entities;
using Lancamentos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Lancamentos.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController(ProducerService msgProducer) : ControllerBase
    {
        private readonly ProducerService _Msgproducer = msgProducer;

        [HttpPost]
        public ActionResult PostLancamento(Lancamento mensagem)
        {
            _Msgproducer.EnviarMensagem(JsonSerializer.Serialize(mensagem));

            return Ok();
        }
    }
}
    