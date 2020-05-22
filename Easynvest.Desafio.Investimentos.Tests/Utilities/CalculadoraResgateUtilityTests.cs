using AutoFixture;
using Easynvest.Desafio.Investimentos.Domain.Interfaces;
using Easynvest.Desafio.Investimentos.Domain.Models;
using Easynvest.Desafio.Investimentos.Domain.Services;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using Easynvest.Desafio.Investimentos.Domain.Utilities;
using System;
using Easynvest.Desafio.Investimentos.Domain.Models.Taxa;

namespace Easynvest.Desafio.Investimentos.Tests.Utilities
{
    public class CalculadoraResgateUtilityTests
    {
        private Mock<ITaxaRepository> _taxaIrRepository;
        private CalculadoraResgateUtility _calculadoraResgateUtility;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _taxaIrRepository = new Mock<ITaxaRepository>();
            _calculadoraResgateUtility = new CalculadoraResgateUtility(_taxaIrRepository.Object);
            _fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            _taxaIrRepository.VerifyAll();
        }

        [TestCase(100, 91, 0.15f, 110, 1.5f, 2.5f)]
        [TestCase(100, 85, 0.15f, 100, 0, 0)]
        [Test]
        public async Task DeveCalcularResgateParaInvestimentoComAteTresMesesVencimentoETambemMaisMetadeVencimentoComTaxaDeMetade(double valorInvestido, double valorResgate, double taxa, double valorTotal, double ir, double iof)
        {
            var investimento = _fixture.Create<InvestimentoTest>();
            investimento.DataReferenciaResgate = DateTime.Now;
            investimento.DataOperacao = investimento.DataReferenciaResgate.AddMonths(-4);
            investimento.Vencimento = investimento.DataReferenciaResgate.AddMonths(2);
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;
            investimento.Ir = ir;
            investimento.Iof = iof;

            _taxaIrRepository.Setup(x => x.GetTaxaCustodiaByTipoCustodia(TipoTaxaCustodia.MetadeVencimento)).ReturnsAsync(taxa);

            var result = await _calculadoraResgateUtility.CalcularValorResgate(investimento);

            result.Should().BeApproximately(valorResgate, 2);
        }

        [TestCase(100, 100, 0.06f, 110, 1.5f, 2.5f)]
        [TestCase(100, 94, 0.06f, 100, 0, 0)]
        [Test]
        public async Task DeveCalcularResgateParaInvestimentoComAteTresMesesVencimento(double valorInvestido, double valorResgate, double taxa, double valorTotal, double ir, double iof)
        {
            var investimento = _fixture.Create<InvestimentoTest>();
            investimento.DataReferenciaResgate = DateTime.Now;
            investimento.DataOperacao = investimento.DataReferenciaResgate.AddMonths(-1);
            investimento.Vencimento = investimento.DataReferenciaResgate.AddMonths(2);
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;
            investimento.Ir = ir;
            investimento.Iof = iof;

            _taxaIrRepository.Setup(x => x.GetTaxaCustodiaByTipoCustodia(TipoTaxaCustodia.TresMesesVencimento)).ReturnsAsync(taxa);

            var result = await _calculadoraResgateUtility.CalcularValorResgate(investimento);

            result.Should().BeApproximately(valorResgate, 2);
        }

        [TestCase(100, 66, 0.3f, 100, 1.5f, 2.5f)]
        [TestCase(100, 70, 0.3f, 100, 0, 0)]
        [Test]
        public async Task DeveCalcularResgateParaInvestimentoComTaxaCustodiaDefault(double valorInvestido, double valorResgate, double taxa, double valorTotal, double ir, double iof)
        {
            var investimento = _fixture.Create<InvestimentoTest>();
            investimento.DataReferenciaResgate = DateTime.Now;
            investimento.DataOperacao = investimento.DataReferenciaResgate.AddMonths(-1);
            investimento.Vencimento = investimento.DataReferenciaResgate.AddMonths(3);
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;
            investimento.Ir = ir;
            investimento.Iof = iof;

            _taxaIrRepository.Setup(x => x.GetTaxaCustodiaByTipoCustodia(TipoTaxaCustodia.Default)).ReturnsAsync(taxa);

            var result = await _calculadoraResgateUtility.CalcularValorResgate(investimento);

            result.Should().BeApproximately(valorResgate, 2);
        }

        [TestCase(799.472, 706.74f, 0.15f, 829.68, 3.0208f, 0)]
        [TestCase(100, 85, 0.15f, 100, 0, 0)]
        [Test]
        public async Task DeveCalcularResgateParaInvestimentoComMetadeVencimento(double valorInvestido, double valorResgate, double taxa, double valorTotal, double ir, double iof)
        {
            var investimento = _fixture.Create<InvestimentoTest>();
            investimento.DataReferenciaResgate = DateTime.Now;
            investimento.DataOperacao = investimento.DataReferenciaResgate.AddMonths(-6);
            investimento.Vencimento = investimento.DataReferenciaResgate.AddMonths(5);
            investimento.ValorInvestido = valorInvestido;
            investimento.ValorTotal = valorTotal;
            investimento.Ir = ir;
            investimento.Iof = iof;

            _taxaIrRepository.Setup(x => x.GetTaxaCustodiaByTipoCustodia(TipoTaxaCustodia.MetadeVencimento)).ReturnsAsync(taxa);

            var result = await _calculadoraResgateUtility.CalcularValorResgate(investimento);

            result.Should().BeApproximately(valorResgate, 2);
        }

        [TestCase(1000, 100, 100, 800)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(1000, 0, 0, 1000)]
        [Test]
        public async Task DeveCalcularResgateAposVencimento(double valorTotal, double ir, double iof, double valorResgate)
        {
            var investimento = _fixture.Create<InvestimentoTest>();
            investimento.DataReferenciaResgate = DateTime.Now;
            investimento.DataOperacao = investimento.DataReferenciaResgate.AddMonths(-4);
            investimento.Vencimento = investimento.DataReferenciaResgate.AddMonths(-1);
            investimento.ValorTotal = valorTotal;
            investimento.Ir = ir;
            investimento.Iof = iof;

            var result = await _calculadoraResgateUtility.CalcularValorResgate(investimento);

            result.Should().BeApproximately(valorResgate, 2);
        }
    }
}
