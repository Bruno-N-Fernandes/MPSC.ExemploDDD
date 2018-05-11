using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.AcessoAosDados
{
	public class Conexao : IDbConnection
	{
		public static Conexao Atual { get { return CtrlConexao._atual; } }

		private readonly IDbConnection _iDbConnection;
		internal Conexao(IDbConnection iDbConnection, String stringConexao)
		{
			_iDbConnection = iDbConnection;
			_iDbConnection.ConnectionString = stringConexao;
			_iDbConnection.Open();
			RDBMS = RDBMS.Create(_iDbConnection);
		}
		void IDbConnection.ChangeDatabase(String databaseName) { _iDbConnection.ChangeDatabase(databaseName); }

		IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il) { return _iDbConnection.BeginTransaction(il); }

		IDbTransaction IDbConnection.BeginTransaction() { return _iDbConnection.BeginTransaction(); }

		void IDbConnection.Open() { _iDbConnection.Open(); }

		void IDbConnection.Close() { _iDbConnection.Close(); }

		void IDisposable.Dispose() { _iDbConnection.Dispose(); }

		String IDbConnection.ConnectionString { get { return _iDbConnection.ConnectionString; } set { _iDbConnection.ConnectionString = value; } }

		int IDbConnection.ConnectionTimeout { get { return _iDbConnection.ConnectionTimeout; } }

		IDbCommand IDbConnection.CreateCommand() { return _iDbConnection.CreateCommand(); }

		String IDbConnection.Database { get { return _iDbConnection.Database; } }

		ConnectionState IDbConnection.State { get { return _iDbConnection.State; } }

		public readonly RDBMS RDBMS;
	}

	public abstract class RDBMS
	{
		public abstract String CmdSqlUltimoIdGerado { get; }

		internal static RDBMS Create(IDbConnection iDbConnection)
		{
			if (iDbConnection is MySqlConnection)
				return new MySqlRDBMS();
			return new SqlServerRDBMS();
		}
	}

	public class SqlServerRDBMS : RDBMS
	{
		public override String CmdSqlUltimoIdGerado
		{
			get { return @"
Declare @NewInternalId BigInt; Set @NewInternalId = SCOPE_IDENTITY(); Select @NewInternalId as Retorno;"; }
		}
	}

	public class MySqlRDBMS : RDBMS
	{
		public override String CmdSqlUltimoIdGerado
		{
			get { return @"
Declare @NewInternalId BigInt; Set @NewInternalId = LAST_INSERT_ID(); Select @NewInternalId as Retorno;"; }
		}
	}

	public static class CtrlConexao
	{
		[ThreadStatic]
		internal static Conexao _atual;

		public static void Inicializar<TIDbConnection>(String stringConexao) where TIDbConnection : IDbConnection, new()
		{
			var conexao = _atual as IDbConnection;
			if (conexao != null)
			{
				conexao.Close();
				conexao.Dispose();
				conexao = _atual = null;
			}
			_atual = new Conexao(new TIDbConnection(), stringConexao);
		}
	}
}