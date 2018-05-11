using MPSC.DomainDrivenDesign.Aplicacao.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;
using System.ServiceModel;

namespace MPSC.DomainDrivenDesign.Servicos.WCF.Seguranca.Autenticacao
{
	public class UsuarioService : IUsuarioService
	{
		public void TrocarSenha(String eMail, String celular, String senhaAntigaCriptografada, String novaSenhaCriptografada, String confirmaNovaSenhaCriptografada)
		{
			try
			{
				var usuarioAPI = new UsuarioAPI();
				usuarioAPI.TrocarSenha(eMail, celular, senhaAntigaCriptografada, novaSenhaCriptografada, confirmaNovaSenhaCriptografada);
			}
			catch (DomainException exception)
			{
				throw new FaultException(exception.Messages());
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.Message);
			}
		}
	}
}