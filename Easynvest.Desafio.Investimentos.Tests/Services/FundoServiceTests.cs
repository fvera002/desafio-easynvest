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
    public class FundoServiceTests
    {
        private FundoService _fundoService;
        private Mock<IConfiguration> _configuration;
        private const string FundosJson = @"{
                    ""fundos"": [{
                            ""capitalInvestido"": 1000,
			                ""ValorAtual"": 1159,
			                ""dataResgate"": ""2022-10-01T00:00:00"",
			                ""dataCompra"": ""2017-10-01T00:00:00"",
			                ""iof"": 0,
			                ""nome"": ""ALASKA"",
			                ""totalTaxas"": 53.49,
			                ""quantity"": 1
                        },
		                {
			                ""capitalInvestido"": 10000.0,
			                ""ValorAtual"": 12300.52,
			                ""dataResgate"": ""2022-11-15T00:00:00"",
			                ""dataCompra"": ""2019-11-15T00:00:00"",
			                ""iof"": 0,
			                ""nome"": ""REAL"",
			                ""totalTaxas"": 134.49,
			                ""quantity"": 1
		                }
	                ]
                }";
        private const string ServiceUrl = "http://fundosurl.mock";

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["FundosUrl"]).Returns(ServiceUrl);
            _fundoService = new FundoService(_configuration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _configuration.VerifyAll();
        }

        [Test]
        public async Task DeveRetornarDoisFundosInvestimento()
        {
            IEnumerable<Investimento> result = null;
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(FundosJson, 200);
                result = await _fundoService.GetInvestimentosByIdCliente("CLIENT_123");
            }

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.ElementAt(1).Tipo.Should().Be(TipoInvestimento.Fundos);
            result.ElementAt(1).DataOperacao.ToString("s").Should().Be("2019-11-15T00:00:00");
            result.ElementAt(1).Vencimento.ToString("s").Should().Be("2022-11-15T00:00:00");
            result.ElementAt(1).ValorTotal.Should().BeApproximately(12300.52f, 2);
            result.ElementAt(1).ValorInvestido.Should().BeApproximately(10000, 2);
            result.ElementAt(1).Rentabilidade.Should().BeApproximately(2300.52f, 2);
            result.ElementAt(1).Nome.Should().Be("REAL");
        }

        [Test]
        public async Task DeveVerificarConversaoParaTipoFundo()
        {
            IEnumerable<Investimento> result = null;
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(FundosJson, 200);
                result = await _fundoService.GetInvestimentosByIdCliente("CLIENT_123");
            }

            var fundo = (Fundo)result.ElementAt(1);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().AllBeOfType<Fundo>();
            fundo.TotalTaxas.Should().BeApproximately(134.49f, 2);
            fundo.Iof.Should().BeApproximately(0f, 2);
            fundo.Quantity.Should().Be(1);
        }
    }
}
        
