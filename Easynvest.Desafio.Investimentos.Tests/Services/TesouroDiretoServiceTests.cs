using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Services;
using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Easynvest.Desafio.Investimentos.Domain.Models.Investimentos;

namespace Easynvest.Desafio.Investimentos.Tests.Services
{
    [TestFixture]
    public class TesouroDiretoServiceTests
    {
        private TesouroDiretoService _tesouroDiretoService;
        private Mock<IConfiguration> _configuration;
        private const string TwoTesouroDiretoJson = @"{
                ""tds"": [
                    {
                        ""valorInvestido"": 799.4720,
			            ""valorTotal"": 829.68,
			            ""vencimento"": ""2025-03-01T00:00:00"",
			            ""dataDeCompra"": ""2015-03-01T00:00:00"",
			            ""iof"": 0,
			            ""indice"": ""SELIC"",
			            ""tipo"": ""TD"",
			            ""nome"": ""Tesouro Selic 2025""
                    },
		            {
                        ""valorInvestido"": 467.1470,
			            ""valorTotal"": 502.787,
			            ""vencimento"": ""2020-02-01T00:00:00"",
			            ""dataDeCompra"": ""2010-02-10T00:00:00"",
			            ""iof"": 0,
			            ""indice"": ""IPCA"",
			            ""tipo"": ""TD"",
			            ""nome"": ""Tesouro IPCA 2035""
                    }
	            ]
            }";

        [OneTimeSetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["TesouroDiretoUrl"]).Returns("http://fundosurl.mock");
            _tesouroDiretoService = new TesouroDiretoService(_configuration.Object);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _configuration.VerifyAll();
        }

        [Test]
        public async Task ShouldReturnTwoInvestimentos()
        {
            IEnumerable<Investimento> result = null;
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(TwoTesouroDiretoJson, 200);
                result = await _tesouroDiretoService.GetInvestimentosByIdCliente("CLIENT_123");
            }

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.ElementAt(1).Tipo.Should().Be(TipoInvestimento.TesouroDireto);
            result.ElementAt(1).DataOperacao.ToString("s").Should().Be("2010-02-10T00:00:00");
            result.ElementAt(1).Vencimento.ToString("s").Should().Be("2020-02-01T00:00:00");
            result.ElementAt(1).ValorTotal.Should().BeApproximately(502.787f, 3);
            result.ElementAt(1).ValorInvestido.Should().BeApproximately(467.1470f, 4);
            result.ElementAt(1).Rentabilidade.Should().BeApproximately(35.64f, 4);
            result.ElementAt(1).Nome.Should().Be("Tesouro IPCA 2035");
        }

        [Test]
        public async Task ShouldConvertTesouroDiretoCorrectly()
        {

            IEnumerable<Investimento> result = null;
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(TwoTesouroDiretoJson, 200);
                result = await _tesouroDiretoService.GetInvestimentosByIdCliente("CLIENT_123");
            }

            var tesouroDireto = (TesouroDireto)result.ElementAt(1);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().AllBeOfType<TesouroDireto>();
            tesouroDireto.Iof.Should().BeApproximately(0f, 2);
            tesouroDireto.Indice.Should().Be("IPCA");
        }
    }
}

