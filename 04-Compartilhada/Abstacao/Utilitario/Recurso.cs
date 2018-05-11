using System;
using System.Configuration;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario
{
	public class Recurso
	{
		public static String DeConexao(String nome)
		{
			var cs = ConfigurationManager.ConnectionStrings[nome];
			return ((cs != null) ? cs.ConnectionString : null);
		}

		public static T DeConfiguracao<T>(String key, T padrao = default(T))
		{
			var value = Obter(key);
			return (value == null) ? padrao : (T)(Convert.ChangeType(value, typeof(T)) ?? padrao);
		}

		private static String Obter(String key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}