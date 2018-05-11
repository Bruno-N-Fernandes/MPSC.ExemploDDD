using System;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario
{
	public interface ILog
	{
		void Info(String mensagem, params Object[] args);
		void Aviso(String mensagem, params Object[] args);
		void Erro(String mensagem, params Object[] args);
		void Erro(Exception exception);
	}
}