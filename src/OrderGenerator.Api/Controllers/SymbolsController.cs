using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Application.Interfaces;

namespace OrderGenerator.Api.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/symbols")]
    [Authorize]
    public class SymbolsController : ControllerBase
    {
        private readonly ISymbolService _symbolService;

        public SymbolsController(ISymbolService symbolService)
        {
            _symbolService = symbolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSymbols(CancellationToken cancellationToken)
        {
            var symbols = await _symbolService.GetSymbolsAsync(cancellationToken);

            return Ok(symbols);
        }
    }
}
