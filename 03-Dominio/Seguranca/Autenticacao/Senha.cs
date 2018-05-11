using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Dominio;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;

namespace MPSC.DomainDrivenDesign.Dominio.Seguranca.Autenticacao
{
	public class Senha : Entidade
	{
		private static Int64 _controle = 0;
		public Int32 UsuarioId { get { return Usuario == null ? 0 : Usuario.Id; } }
		public Usuario Usuario { get; set; }
		public String SenhaCriptografada { get; set; }
		public Senha()
		{
			Inclusao = DateTime.UtcNow.AddMilliseconds(_controle++);
		}

		public override void EhValido()
		{
			AssegureQue.NaoEhNulo(Usuario, "Usuário não pode ser nulo");
		}
	}
}