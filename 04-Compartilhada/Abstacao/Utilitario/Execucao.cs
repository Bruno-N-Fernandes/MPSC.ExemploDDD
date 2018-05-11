using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.AcessoAosDados;
using System;
using System.Data;
using System.Diagnostics;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario
{
	public class Execucao
	{
		protected readonly ILog _iLog;
		public Exception Exception { get; protected set; }
		public Boolean HouveErro { get { return Exception != null; } }
		public Object Retorno { get; protected set; }

		public Execucao(ILog iLog)
		{
			_iLog = iLog;
		}

		public T Executar<T>(Func<ILog, T> acao)
		{
			try
			{
				return acao(_iLog);
			}
			catch (DomainException exception)
			{
				_iLog.Aviso(exception.Messages());
				Exception = exception;
				return default(T);
			}
			catch (Exception exception)
			{
				_iLog.Erro(exception);
				Exception = exception;
				return default(T);
			}
		}

		public static Execucao Proteger<T>(Func<ILog, T> acao)
		{
			var execucao = new Execucao(ConsoleLog.Instancia);
			execucao.Retorno = execucao.Executar(acao);
			return execucao;
		}
	}

	public class Transacao : Execucao
	{
		public Transacao(ILog iLog) : base(iLog) { }

		public T Executar<T>(Func<IDbTransaction, ILog, T> acao)
		{
			IDbTransaction transacao = null;
			var sucesso = false;
			T retorno;
			try
			{
				var iDbConnection = Conexao.Atual as IDbConnection;
				transacao = iDbConnection.BeginTransaction();
				retorno = acao(transacao, _iLog);
				sucesso = true;
			}
			catch (DomainException exception)
			{
				sucesso = false;
				retorno = default(T);
				Exception = exception;
				_iLog.Aviso(exception.Messages());
			}
			catch (Exception exception)
			{
				sucesso = false;
				retorno = default(T);
				Exception = exception;
				_iLog.Erro(exception);
			}
			finally
			{
				if (transacao != null)
				{
					if (sucesso)
						transacao.Commit();
					else
						transacao.Rollback();
				}
			}
			return retorno;
		}

		[DebuggerStepThrough]
		public static Transacao Proteger<T>(Func<IDbTransaction, ILog, T> acao)
		{
			var transacao = new Transacao(ConsoleLog.Instancia);
			transacao.Retorno = transacao.Executar(acao);
			return transacao;
		}
	}
}