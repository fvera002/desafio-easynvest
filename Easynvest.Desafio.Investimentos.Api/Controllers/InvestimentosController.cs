using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easynvest.Desafio.Investimentos.Api.Controllers
{
    [ApiController]
    public class InvestimentosController : ControllerBase
    {
        private const string CacheKey = "GetPortifolioInvestimentosByClient";
        private readonly IMemoryCache _cache;
        private readonly IPortifolioInvestimentosService _portifolioInvestimentosService;
        private readonly long _hoursInCache;

        public InvestimentosController(IConfiguration configuration, IMemoryCache cache, IPortifolioInvestimentosService portifolioInvestimentosService)
        {
            _cache = cache;
            _portifolioInvestimentosService = portifolioInvestimentosService;

            var configHoursInCache = configuration["HoursInCache"];
            _hoursInCache = string.IsNullOrEmpty(configHoursInCache) ? 24 : long.Parse(configHoursInCache);
        }

        private MemoryCacheEntryOptions CacheOptions
        { 
            get
            {
                var option = new MemoryCacheEntryOptions();
                option.AbsoluteExpiration = DateTime.Today.AddHours(_hoursInCache);
                option.SlidingExpiration = TimeSpan.FromHours(_hoursInCache);

                return option;
            }
        }

        [HttpGet]
        [Route("api/v1/clients/{idCliente}/[controller]")]
        public async Task<IActionResult> GetPortifolioInvestimentosByClient([FromRoute] string idCliente)
        {
            PortifolioInvestimentos result;
            var cacheKey = CacheKey + idCliente;

            if (!_cache.TryGetValue(cacheKey, out result))
            {
                // Key not in cache, so get data.
                result = await _portifolioInvestimentosService.GetPortifolioInvestimentosByIdCliente(idCliente);
                _cache.Set(cacheKey, result, CacheOptions);
            }

            return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
        }
    }
}
