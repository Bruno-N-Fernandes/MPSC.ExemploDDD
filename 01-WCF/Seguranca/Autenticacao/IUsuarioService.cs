using System;
using System.ServiceModel;

namespace MPSC.DomainDrivenDesign.Servicos.WCF.Seguranca.Autenticacao
{
	[ServiceContract]
	public interface IUsuarioService
	{
		[OperationContract]
		void TrocarSenha(String eMail, String celular, String senhaAntigaCriptografada, String novaSenhaCriptografada, String confirmaNovaSenhaCriptografada);
	}
}