using System;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario
{
	public class ConsoleLog : ILog
	{
		public static readonly ILog Instancia = new ConsoleLog();
		protected ConsoleLog() { }

		public void Info(String mensagem, params Object[] args) { Log(ConsoleColor.White, "INFO: " + mensagem, args); }
		public void Aviso(String mensagem, params Object[] args) { Log(ConsoleColor.Yellow, "AVISO: " + mensagem, args); }
		public void Erro(String mensagem, params Object[] args) { Log(ConsoleColor.Red, "ERRO: " + mensagem, args); }
		public void Erro(Exception exception) { Log(ConsoleColor.Red, "ERRO: " + exception.Messages()); }

		protected virtual void Log(ConsoleColor consoleColor, String mensagem, params Object[] args)
		{
			lock (Instancia)
			{
				Console.ForegroundColor = consoleColor;
				Console.WriteLine(mensagem, args);
			}
		}
	}

	public class FileLog : ConsoleLog
	{
		protected FileLog() { }
		protected override void Log(ConsoleColor consoleColor, String mensagem, params Object[] args)
		{
			base.Log(consoleColor, mensagem, args);
		}
	}
}