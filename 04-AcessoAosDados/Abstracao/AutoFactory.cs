using MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Abstracao.AutoMaping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Abstracao.v2
{
	public static class CtrlConexao
	{
		[ThreadStatic]
		internal static Conexao _atual;

		public static void Inicializar<TIDbConnection>(String stringConexao) where TIDbConnection : IDbConnection, new()
		{
			IDisposable d = _atual;
			if (d != null) d.Dispose();
			_atual = new Conexao(stringConexao, () => new TIDbConnection());
		}
	}

	public class Conexao : IDisposable
	{
		private Boolean _suportaMultipleActiveResultSet;

		private int _contaTransacao = 0;
		private IDbConnection conexaoAtiva = null;
		private IDbTransaction dbTransaction = null;

		private IList<IDbConnection> conexoes = new List<IDbConnection>();
		private IList<IDbCommand> comandos = new List<IDbCommand>();
		private IList<IDataReader> leitores = new List<IDataReader>();


		private readonly String _stringDeConexao;
		private readonly Func<IDbConnection> _newIDbConnection;

		public Conexao(String stringDeConexao, Func<IDbConnection> newIDbConnection, Boolean? suportaMultipleActiveResultSet = null)
		{
			_stringDeConexao = stringDeConexao;
			_newIDbConnection = newIDbConnection;
			_suportaMultipleActiveResultSet = suportaMultipleActiveResultSet ?? stringDeConexao.ToUpper().Contains("MULTIPLEACTIVERESULTSET=TRUE");
		}

		public T Obter<T, TId>(String query, TId id) where T : new()
		{
			var retorno = default(T);
			var iDbCommand = CriarComando(query);
			var iDataReader = ExecuteReader(iDbCommand);
			if (iDataReader.Read())
			{
				var mapa = Mapeamento.Obter<T>();
				retorno = mapa.Preencher(new T(), iDataReader);
				iDataReader.Close();
				iDataReader.Dispose();
			}
			iDbCommand.Dispose();

			return retorno;
		}

		public IEnumerable<T> Query<T>(String query, Object parametros) where T : new()
		{
			var iDbCommand = CriarComando(query);
			var iDataReader = ExecuteReader(iDbCommand);
			var mapa = Mapeamento.Obter<T>();
			while (iDataReader.Read())
			{
				yield return mapa.Preencher(new T(), iDataReader);
			}
			iDataReader.Close();
			iDataReader.Dispose();
			iDbCommand.Dispose();
		}


		private IDataReader ExecuteReader(Object dbCommand)
		{
			var iDbCommand = dbCommand as IDbCommand;
			var iDataReader = iDbCommand.ExecuteReader();
			leitores.Add(iDataReader);
			return iDataReader;
		}

		private IDbCommand CriarComando(String querySQL)
		{
			return CriarComando(querySQL, null);
		}

		private IDbCommand CriarComando(String querySQL, Object param)
		{
			var ehUmSelect = Regex.Replace(querySQL, @"[^a-zA-Z]", String.Empty).ToUpper().StartsWith("SELECT");
			var vEhProcedure = Regex.Replace(querySQL, @"[^a-zA-Z]", String.Empty).ToUpper().StartsWith("EXEC") || Regex.Replace(querySQL, @"[^a-zA-Z]", String.Empty).ToUpper().StartsWith("SP_");
			var vDbConnection = ObterConexao(ehUmSelect);

			var vDbCommand = ConfigurarParams(vDbConnection.CreateCommand(), param, querySQL);
			vDbCommand.CommandType = vEhProcedure ? CommandType.StoredProcedure : CommandType.Text;
			vDbCommand.Transaction = (ehUmSelect && !_suportaMultipleActiveResultSet) ? null : dbTransaction;
			comandos.Add(vDbCommand);
			return vDbCommand;
		}

		private static IDbCommand ConfigurarParams(IDbCommand dbCommand, Object param, String querySQL)
		{
			dbCommand.CommandText = querySQL;
			if (param != null)
			{
				var mapa = Mapeamento.Obter(param.GetType());
				var vMatchCollection = (new Regex("[@][a-zA-Z0-9_]+")).Matches(querySQL);
				//if ((vMatchCollection.Count == 0) && (mapa.Any()))
				//{
				//    foreach (var vNomeParam in mapa)
				//        if (!vNomeParam.Contains("-"))
				//            AdicionarParam(dbCommand, vNomeParam, param, mapa);
				//}
				//else
				{
					foreach (Match vMatch in vMatchCollection)
						AdicionarParam(dbCommand, vMatch.Value, param, mapa);
				}
			}

			return dbCommand;
		}

		private static void AdicionarParam(IDbCommand dbCommand, String nomeParam, Object instancia, Mapa param)
		{
			if (nomeParam.StartsWith("@") && !param.Existe(nomeParam))
				nomeParam = nomeParam.Substring(1);

			if (param.Existe(nomeParam) && !dbCommand.Parameters.Contains(nomeParam))
			{
				var valor = param.Obter(nomeParam, instancia);
				var iDbParameter = dbCommand.CreateParameter();
				iDbParameter.ParameterName = nomeParam;
				iDbParameter.DbType = ObterTipo(valor);
				iDbParameter.Value = ObterValor(valor);
				dbCommand.Parameters.Add(iDbParameter);
			}
		}

		public static DbType ObterTipo(Object valor)
		{
			if (valor == null)
				return DbType.String;
			else if (valor is Boolean)
				return DbType.Boolean;
			else if (valor is Enum)
				return DbType.Int32;
			else if (valor is Byte)
				return DbType.Byte;
			else if (valor is SByte)
				return DbType.SByte;
			else if (valor is Int16)
				return DbType.Int16;
			else if (valor is Int32)
				return DbType.Int32;
			else if (valor is Int64)
				return DbType.Int64;
			else if (valor is UInt16)
				return DbType.UInt16;
			else if (valor is UInt32)
				return DbType.UInt32;
			else if (valor is UInt64)
				return DbType.UInt64;
			else if (valor is Single)
				return DbType.Single;
			else if (valor is Double)
				return DbType.Double;
			else if (valor is Decimal)
				return DbType.Decimal;
			else if (valor is DateTime)
				return DbType.DateTime;
			else if (valor is String)
				return DbType.String;
			else if (valor is StringBuilder)
				return DbType.Object;
			else if (valor is Char)
				return DbType.StringFixedLength;
			else
				return DbType.Object;
		}

		public static Boolean EhNulo(Object valor)
		{
			Boolean vEhNulo = (valor == null);
			vEhNulo |= ((valor is String) && ((String)valor) == String.Empty);
			vEhNulo |= ((valor is StringBuilder) && ((StringBuilder)valor).ToString() == String.Empty);
			vEhNulo |= ((valor is Char) && ((Char)valor) == Char.MinValue);
			vEhNulo |= ((valor is Byte) && ((Byte)valor) == Byte.MinValue);
			vEhNulo |= ((valor is SByte) && ((SByte)valor) == SByte.MinValue);

			vEhNulo |= ((valor is Int16) && ((Int16)valor) == Int16.MinValue);
			vEhNulo |= ((valor is Int32) && ((Int32)valor) == Int32.MinValue);
			vEhNulo |= ((valor is Int64) && ((Int64)valor) == Int64.MinValue);
			vEhNulo |= ((valor is IntPtr) && ((IntPtr)valor) == IntPtr.Zero);

			vEhNulo |= ((valor is UInt16) && ((UInt16)valor) == UInt16.MinValue);
			vEhNulo |= ((valor is UInt32) && ((UInt32)valor) == UInt32.MinValue);
			vEhNulo |= ((valor is UInt64) && ((UInt64)valor) == UInt64.MinValue);
			vEhNulo |= ((valor is UIntPtr) && ((UIntPtr)valor) == UIntPtr.Zero);

			vEhNulo |= ((valor is Single) && ((Single)valor) == Single.MinValue);
			vEhNulo |= ((valor is Double) && ((Double)valor) == Double.MinValue);
			vEhNulo |= ((valor is Decimal) && ((Decimal)valor) == Decimal.MinValue);
			vEhNulo |= ((valor is DateTime) && ((DateTime)valor) == DateTime.MinValue);

			vEhNulo |= ((valor is Enum) && !Enum.IsDefined(valor.GetType(), valor));
			vEhNulo |= ((valor is DBNull) && ((DBNull)valor) == DBNull.Value);
			return vEhNulo;
		}

		public static object ObterValor(Object valor)
		{
			return EhNulo(valor) ? DBNull.Value : valor;
		}


		public Boolean IniciarTransacao()
		{
			Boolean vRetorno = false;
			try
			{
				_contaTransacao++;
				if (dbTransaction == null)
				{
					var vDbConnection = ObterConexao(false);
					dbTransaction = vDbConnection.BeginTransaction();
				}
				vRetorno = true;
			}
			catch (Exception)
			{
				vRetorno = false;
			}
			return vRetorno;
		}

		public Boolean TerminarTransacao(Boolean commit)
		{
			Boolean vRetorno = true;
			try
			{
				_contaTransacao--;
				if (_contaTransacao == 0)
				{
					PreTerminarTransacao();

					if (dbTransaction != null)
					{
						if (commit)
							dbTransaction.Commit();
						else
							dbTransaction.Rollback();
					}

					PosTerminarTransacao();
				}
				vRetorno = true;
			}
			catch (Exception)
			{
				vRetorno = false;
			}

			return vRetorno;
		}

		public void Dispose()
		{
			PreTerminarTransacao();
			PosTerminarTransacao();
		}

		private void PreTerminarTransacao()
		{
			foreach (var vDbDataReader in leitores)
			{
				try
				{
					if (!vDbDataReader.IsClosed)
						vDbDataReader.Close();
					vDbDataReader.Dispose();
				}
				catch (Exception) { }
			}
			foreach (var vDbCommand in comandos)
			{
				try
				{
					vDbCommand.Dispose();
				}
				catch (Exception) { }
			}
			comandos.Clear();
			leitores.Clear();
		}

		private void PosTerminarTransacao()
		{
			try
			{
				if (dbTransaction != null)
					dbTransaction.Dispose();
			}
			catch (Exception) { }
			finally
			{
				dbTransaction = null;
				conexaoAtiva = null;
			}

			foreach (var vDbConnection in conexoes)
			{
				try
				{
					if (vDbConnection.State == ConnectionState.Open)
						vDbConnection.Close();
					vDbConnection.Dispose();
				}
				catch (Exception) { }
			}
			conexoes.Clear();
		}

		private IDbConnection ObterConexao(Boolean ehSelect)
		{
			var vDbConnection = ehSelect ? ObterConexaoParaSelect() : ObterConexaoAtiva();

			if (vDbConnection.State != ConnectionState.Open)
				vDbConnection.Open();

			if (!conexoes.Contains(vDbConnection))
				conexoes.Add(vDbConnection);

			return vDbConnection;
		}

		private IDbConnection ObterConexaoParaSelect()
		{
			if (_suportaMultipleActiveResultSet)
				return ObterConexaoAtiva();
			else
				return NovaConexao(_stringDeConexao);
		}

		private IDbConnection ObterConexaoAtiva()
		{
			if (conexaoAtiva == null)
				conexaoAtiva = NovaConexao(_stringDeConexao);

			return conexaoAtiva;
		}

		private IDbConnection NovaConexao(String stringConexao)
		{
			IDbConnection dbConnection = null;

			try
			{
				dbConnection = _newIDbConnection();
				dbConnection.ConnectionString = stringConexao;
			}
			catch (Exception vException)
			{
				throw new Exception("dbConnection = Activator.CreateInstance(typeof(iDbConnectionActivator)) as IDbConnection;", vException);
			}

			return dbConnection;
		}
	}
	/*

























        public class Conexao : IDisposable
        {
            private enum Tipo { NonQuery, Scalar, Reader }
            private readonly String _stringDeConexao;
            private readonly Func<IDbConnection> _newIDbConnection;
            private readonly List<PoolConexao> _pool = new List<PoolConexao>();
            private IDbTransaction _iDbTransaction;
            private Int32 _transactionCount = 0;

            public Conexao(String stringDeConexao, Func<IDbConnection> newIDbConnection)
            {
                _stringDeConexao = stringDeConexao;
                _newIDbConnection = newIDbConnection;
            }

            public void AbrirTransacao()
            {
                if (_transactionCount++ == 0)
                    _iDbTransaction = Para(Tipo.NonQuery).BeginTransaction();
            }

            public void FecharTransacao(Boolean comCommit)
            {
                if (_iDbTransaction != null)
                {
                    if (!comCommit)
                    {
                        _iDbTransaction.Rollback();
                        _iDbTransaction = null;
                        _transactionCount = 0;
                    }
                    else if (_transactionCount == 1)
                    {
                        _iDbTransaction.Commit();
                        _iDbTransaction = null;
                        _transactionCount = 0;
                    }
                    else
                        _transactionCount--;
                }
            }


            public T Obter<T, TId>(String query, TId id)
            {
                var command = Para(Tipo.Scalar).CriarComando(query);


                return default(T);
            }

            public IEnumerable<T> Query<T>(String query, Object parametros)
            {
                var command = Para(Tipo.Reader).CriarComando(query);

                return null;
            }

            private PoolConexao Para(Tipo para)
            {
                var conexao = _pool.OrderBy(p => p.Usado).FirstOrDefault(p => p.For == para);
                if ((conexao == null) || ((para == Tipo.Reader) && conexao.UsadoRecentemente))
                {
                    conexao = new PoolConexao { For = para, IDbConnection = _newIDbConnection() };
                }
                conexao.Usado = DateTime.Now;
                return conexao
            }

            public class PoolConexao
            {
                public Tipo For { get; set; }
                public IDbConnection IDbConnection { get; set; }

                public DateTime Usado { get; set; }

                public bool UsadoRecentemente { get { return (DateTime.Now - Usado).TotalSeconds < 1; } }

                public IDbCommand CriarComando(String query)
                {
                    var iDbCommand = IDbConnection.CreateCommand();

                    return
                }
            }

            void IDisposable.Dispose()
            {
                //if (iDbConnection != null)
                //{
                //    iDbConnection.Close();
                //    iDbConnection.Dispose();
                //}
            }
        }
     * 
     */
}
