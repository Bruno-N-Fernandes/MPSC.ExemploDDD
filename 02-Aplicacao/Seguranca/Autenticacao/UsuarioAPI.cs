using MPSC.DomainDrivenDesign.Aplicacao.Abstracao;
using MPSC.DomainDrivenDesign.Dominio.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;
using System.Linq;

namespace MPSC.DomainDrivenDesign.Aplicacao.Seguranca.Autenticacao
{
	public class UsuarioAPI : BaseAPI
	{
		private Usuarios _usuarios;
		private Usuarios Usuarios { get { return _usuarios ?? (_usuarios = new Usuarios()); } }

		public void TrocarSenha(String eMail, String celular, String senhaAntigaCriptografada, String novaSenhaCriptografada, String confirmaNovaSenhaCriptografada)
		{
			var usuarios = Usuarios.ObterPor(new Usuario { EMail = eMail, Celular = celular });
			AssegureQue.NaoEhNulo(usuarios, "Não foi encontrado um usuário cadastrado com este eMail e / ou celular");
			AssegureQue.EhVerdadeiro(usuarios.Count() == 1, "EMail e / ou celular inválido. Confira as informações e tente novamente");

			var usuario = usuarios.First();
			usuario.Preencher(Usuarios.BuscarSenhas(usuario.Id));
			var senha = usuario.TrocarSenha(senhaAntigaCriptografada, novaSenhaCriptografada, confirmaNovaSenhaCriptografada);
			Usuarios.Gravar(senha);
		}

		public void CadastrarUsuario(String nome, String eMail, String celular, String novaSenhaCriptografada, String confirmaNovaSenhaCriptografada)
		{
			var usuario = new Usuario { Nome = nome, EMail = eMail, Celular = celular, Inclusao = DateTime.UtcNow };
			usuario.DefinirSenhaInicial(novaSenhaCriptografada, confirmaNovaSenhaCriptografada);
			usuario.EhValido();

			var usuarios = Usuarios.ObterPor(usuario);
			AssegureQue.EhVazio(usuarios, "Já existe um usuário cadastrado com este eMail e / ou celular");
			Usuarios.Gravar(usuario);
		}

		public void Autenticar(String eMailOuCelular, String senhaCriptografada)
		{
			AssegureQue.NaoEhNulo(eMailOuCelular, "EMail e / ou celular não informado (1)");
			AssegureQue.NaoEhVazio(eMailOuCelular, "EMail e / ou celular não informado (2)");

			var usuario = new Usuario { EMail = eMailOuCelular, Celular = eMailOuCelular };
			var usuarios = Usuarios.ObterPor(usuario);
			AssegureQue.NaoEhVazio(usuarios, "Usuário não encontrado com este eMail e / ou celular");
			AssegureQue.EhVerdadeiro(usuarios.Count() == 1, "EMail e / ou celular inválido. Confira as informações e tente novamente");

			usuario = Usuarios.ObterPorIdComSenhas(usuarios.First().Id);
			AssegureQue.EhVerdadeiro(usuario.ConfirmarSenha(senhaCriptografada), "A senha informada não confere!");
		}
	}
}