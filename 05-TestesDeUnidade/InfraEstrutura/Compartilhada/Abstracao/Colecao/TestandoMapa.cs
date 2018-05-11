using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Colecao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPSC.DomainDrivenDesign.Testes.DeUnidade.Infra.Compartilhada.Abstracao.Colecao
{
	[TestClass]
	public class TestandoMapa
	{
		[TestMethod]
		public void QuandoInclui10ElementosEFazConsultasSequenciais()
		{
			var dic = Mapeador.De<String, Int32>()
				.Mapear("0", 00).Mapear("1", 10)
				.Mapear("2", 20).Mapear("3", 30)
				.Mapear("4", 40).Mapear("5", 50)
				.Mapear("6", 60).Mapear("7", 70)
				.Mapear("8", 80).Mapear("9", 90)
				.Criar();

			Assert.IsNotNull(dic);
			Assert.AreEqual(00, dic["0"]);
			Assert.AreEqual(10, dic["1"]);
			Assert.AreEqual(20, dic["2"]);
			Assert.AreEqual(30, dic["3"]);
			Assert.AreEqual(40, dic["4"]);
			Assert.AreEqual(70, dic["7"]);
			Assert.AreEqual(80, dic["8"]);
			Assert.AreEqual(90, dic["9"]);
		}

		[TestMethod, ExpectedException(typeof(InvalidOperationException))]
		public void QuandoRepeteUmAMesmaKeyNoMapeamento_DeveLancarExcecao()
		{
			var builder = Mapeador.De<Int32, Int32>()
				.Mapear(0, 00).Mapear(1, 10)
				.Mapear(2, 20).Mapear(3, 30)
				.Mapear(4, 40).Mapear(5, 50)
				.Mapear(6, 60).Mapear(7, 70)
				.Mapear(0, 80).Mapear(9, 90);

			var mapa = builder.Criar();
			Assert.Fail("Deveria Lançar Excecao Mas Nao Lançou");
		}

		[TestMethod, ExpectedException(typeof(InvalidOperationException))]
		public void QuandoMapeiaUmaKey_NulaDeveLancarExcecao()
		{
			var builder = Mapeador.De<String, Int32>()
				.Mapear("0", 00).Mapear("1", 10)
				.Mapear("2", 20).Mapear("3", 30)
				.Mapear("4", 40).Mapear("5", 50)
				.Mapear(null, 60).Mapear("7", 70);

			var mapa = builder.Criar();
			Assert.Fail("Deveria Lançar Excecao Mas Nao Lançou");
		}
	}
}