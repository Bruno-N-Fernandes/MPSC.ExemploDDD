using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.AcessoAosDados;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System.Data.SqlClient;

namespace MPSC.DomainDrivenDesign.Aplicacao.Abstracao
{
	public abstract class BaseAPI
	{
		protected BaseAPI()
		{
			var rdbms = Recurso.DeConfiguracao("RDBMS", "SqlConnection");
			var strConexao = Recurso.DeConexao(rdbms);

			if (rdbms.Equals("SqlConnection"))
				CtrlConexao.Inicializar<SqlConnection>(strConexao);

			//else if(rdbms.Equals("MySqlConnection"))
			//	CtrlConexao.Inicializar<MySqlConnection>(strConexao);
		}
	}
}