using MPSC.DomainDrivenDesign.Aplicacao.Seguranca.Autenticacao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPSC.DomainDrivenDesign.Testes.Integrados.Aplicacao.CasosDeUso.Seguranca.Autenticacao
{
	[TestClass]
	public class TestandoUsuarioUC
	{
		[TestMethod]
		public void QuandoPedePraCadastrarUmUsuario()
		{
			var usuarioAPI = new UsuarioAPI();
			usuarioAPI.CadastrarUsuario("Fulano", "usuario@dominio.com.br", "21 9 9876-5432", "SenhaPessoalIntransferível", "SenhaPessoalIntransferível");
		}

		[TestMethod]
		public void QuandoPedePraAlterarASenha()
		{
			var usuarioAPI = new UsuarioAPI();
			usuarioAPI.TrocarSenha("usuario@dominio.com.br", "21 9 9876-5432", "SenhaPessoalIntransferível", "NovaSenha", "NovaSenha");
		}
	}
}