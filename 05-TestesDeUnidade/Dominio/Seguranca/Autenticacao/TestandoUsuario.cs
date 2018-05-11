using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSC.DomainDrivenDesign.Dominio.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;

namespace MPSC.DomainDrivenDesign.Testes.DeUnidade.Dominio.Seguranca.Autenticacao
{
	[TestClass]
	public class TestandoUsuario
	{
		[TestMethod]
		public void QuandoAcabaDeCriarUmUsuarioDevePossuirApenasUmaSenha()
		{
			var usuario = new Usuario
			{
				Nome = "Fulano",
				EMail = "usuario@dominio.com.br",
				Celular = "21 9 9876-5432",
			};
			usuario.DefinirSenhaInicial("A", "A");

			Assert.AreEqual(1, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("A"));
		}

		[TestMethod]
		public void QuandoTentaDefinirUmaSenhaInicialDepoisDeJaTerUmaSenhaDeveLancarExcecao()
		{
			var usuario = new Usuario
			{
				Nome = "Beltrano",
				EMail = "usuario@dominio.com.br",
				Celular = "21 9 9876-5432",
			};
			var execucao1 = Execucao.Proteger(log => usuario.DefinirSenhaInicial("A", "A"));
			Assert.IsFalse(execucao1.HouveErro);
			Assert.IsNull(execucao1.Exception);

			Assert.AreEqual(1, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("A"));

			var execucao2 = Execucao.Proteger(log => usuario.DefinirSenhaInicial("A", "A"));
			Assert.IsTrue(execucao2.HouveErro);
			Assert.IsNotNull(execucao2.Exception);
		}


		[TestMethod]
		public void QuandoTentaAlterarASenhaParaAsUltimas5SenhasDeveLancarExcecao()
		{
			var usuario = new Usuario
			{
				Nome = "Sicrano",
				EMail = "usuario@dominio.com.br",
				Celular = "21 9 9876-5432",
			};

			usuario.DefinirSenhaInicial("A", "A");
			Assert.AreEqual(1, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("A"));

			usuario.TrocarSenha("A", "B", "B");
			Assert.AreEqual(2, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("B"));

			usuario.TrocarSenha("B", "C", "C");
			Assert.AreEqual(3, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("C"));

			usuario.TrocarSenha("C", "D", "D");
			Assert.AreEqual(4, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("D"));

			usuario.TrocarSenha("D", "E", "E");
			Assert.AreEqual(5, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("E"));

			usuario.TrocarSenha("E", "F", "F");
			Assert.AreEqual(6, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("F"));

			var execucao = Execucao.Proteger(log => usuario.TrocarSenha("F", "F", "F"));
			Assert.IsTrue(execucao.HouveErro);
			Assert.IsNotNull(execucao.Exception);
			Assert.AreEqual(6, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("F"));

			execucao = Execucao.Proteger(log => usuario.TrocarSenha("F", "E", "E"));
			Assert.IsTrue(execucao.HouveErro);
			Assert.IsNotNull(execucao.Exception);
			Assert.AreEqual(6, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("F"));

			execucao = Execucao.Proteger(log => usuario.TrocarSenha("F", "D", "D"));
			Assert.IsTrue(execucao.HouveErro);
			Assert.IsNotNull(execucao.Exception);
			Assert.AreEqual(6, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("F"));

			execucao = Execucao.Proteger(log => usuario.TrocarSenha("F", "C", "C"));
			Assert.IsTrue(execucao.HouveErro);
			Assert.IsNotNull(execucao.Exception);
			Assert.AreEqual(6, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("F"));

			execucao = Execucao.Proteger(log => usuario.TrocarSenha("F", "B", "B"));
			Assert.IsTrue(execucao.HouveErro);
			Assert.IsNotNull(execucao.Exception);
			Assert.AreEqual(6, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("F"));

			execucao = Execucao.Proteger(log => { log.Aviso("Tentando Trocar Senha"); var s = usuario.TrocarSenha("F", "A", "A"); log.Aviso("Senha Alterada"); return s; });
			Assert.IsFalse(execucao.HouveErro);
			Assert.IsNull(execucao.Exception);
			Assert.AreEqual(7, usuario.Senhas.Count);
			Assert.IsTrue(usuario.ConfirmarSenha("A"));
		}
	}
}