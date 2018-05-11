using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Abstracao.AutoMaping
{
	public sealed class Mapeamento
	{
		internal static readonly Dictionary<Type, Mapa> _mapa = new Dictionary<Type, Mapa>();

		public static Mapa Obter<TEntidade>()
		{
			return Obter(typeof(TEntidade));
		}

		public static Mapa Obter(Type tipo)
		{
			return _mapa.TryGetValue(tipo, out Mapa mapa) ? mapa : Mapear(tipo);
		}

		private static Mapa Mapear(Type tipo)
		{
			var mapeamentoParaEntidade = ObterMapeamentoPara(tipo) ?? tipo.CriarMapeamento();
			return (mapeamentoParaEntidade != null) ? Activator.CreateInstance(mapeamentoParaEntidade) as Mapa : new Mapa(tipo);
		}

		private static Type ObterMapeamentoPara(Type tipo)
		{
			return ObterMapeamentosPor(t => t.BaseType.GetGenericArguments().All(g => g == tipo)).FirstOrDefault();
		}

		private static IEnumerable<Type> ObterMapeamentosPor(Func<Type, Boolean> filtro)
		{
			var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
			var classTypes = allTypes.Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && (t.BaseType != null) && t.BaseType.IsGenericType).ToArray();
			return classTypes.Where(filtro);
		}

		public static void Mapear()
		{
			var tipo = typeof(Mapa);
			var allMaps = ObterMapeamentosPor(t => t.BaseType.GetInterfaces().Any(i => i == tipo));
			foreach (var tipoDoMapa in allMaps)
				Activator.CreateInstance(tipoDoMapa);
		}
	}

	public static class CompilerUtil
	{
		public static Type CriarMapeamento(this Type tipo)
		{
			var classe = String.Format("namespace MapeamentoVirtual {{\r\n public class Mapa{0} : {1}.Mapa<{2}> {{	}} \r\n}}", tipo.Name, typeof(Mapa).Namespace, tipo.FullName);
			return CompilarClasseVirtual(classe, "MapeamentoVirtual.Mapa" + tipo.Name);
		}

		private static Type CompilarClasseVirtual(String codigoFonte, String nomeCompletoDaClasse)
		{
			var codeCompiler = new CSharpCodeProvider();
			var compilerResults = codeCompiler.CompileAssemblyFromSource(CreateCompillerParameters(false, true), codigoFonte);
			return compilerResults.CompiledAssembly.GetType(nomeCompletoDaClasse, false, true);
		}

		private static CompilerParameters CreateCompillerParameters(Boolean generateExecutable, Boolean includeDebugInformation)
		{
			return new CompilerParameters(new String[] { Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase) })
			{
				GenerateInMemory = !generateExecutable,
				GenerateExecutable = generateExecutable,
				IncludeDebugInformation = includeDebugInformation
			};
		}
	}


	public class Mapa
	{
		private readonly List<IMapping> _mapa;

		internal Mapa(Type tipo)
		{
			_mapa = new List<IMapping>();
			Mapeamento._mapa[tipo] = this;
			Configurar(tipo);
		}

		private void Configurar(Type tipo)
		{
			Configurar();
			if (!_mapa.Any())
			{
				var props = tipo.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
				foreach (var p in props)
					Add(new Mapping(p.Name, e => p.GetValue(e, null), (e, d, i) => { if (!d.IsDBNull(i)) p.SetValue(e, d.GetValue(i), null); }, _mapa.Count));
			}
		}

		protected virtual void Configurar() { }

		protected void Mapear<TEntidade, T>(Expression<Func<TEntidade, T>> exp, Action<TEntidade, IDataRecord, Int32> setFunc)
		{
			Mapear((exp.Body as MemberExpression).Member.Name, exp.Compile(), setFunc);
		}

		protected void Mapear<TEntidade, T>(String nomeDoCampoNoBanco, Func<TEntidade, T> getFunc, Action<TEntidade, IDataRecord, Int32> setFunc)
		{
			Add(new Mapping<TEntidade, T>(nomeDoCampoNoBanco, getFunc, setFunc, _mapa.Count));
		}

		private void Add(IMapping mapping)
		{
			_mapa.Add(mapping);
		}

		public override String ToString()
		{
			return String.Join(", ", _mapa);
		}

		public T Preencher<T>(T instancia, IDataRecord dr)
		{
			foreach (var m in _mapa)
				m.Preencher(instancia, dr);
			return instancia;
		}


		public Object Obter(String nomeDoParametroOuDoCampo, Object instancia)
		{
			return _mapa.FirstOrDefault(m => m.ConfirmarNome(nomeDoParametroOuDoCampo)).Obter(instancia);
		}
		public Boolean Existe(String nomeDoParametroOuDoCampo)
		{
			return _mapa.Exists(m => m.ConfirmarNome(nomeDoParametroOuDoCampo));
		}
	}

	public interface IMapping { void Preencher(Object instancia, IDataRecord dr); Boolean ConfirmarNome(String nome); Object Obter(Object instancia); }
	public class Mapping<TEntidade, T> : IMapping
	{
		public readonly String NomeDoCampoNoBanco;
		public readonly Int32 Indice;
		public readonly Func<TEntidade, T> Get;
		public readonly Action<TEntidade, IDataRecord, Int32> Set;

		public Mapping(String nomeDoCampoNoBanco, Func<TEntidade, T> getFunc, Action<TEntidade, IDataRecord, Int32> setFunc, Int32 indice)
		{
			NomeDoCampoNoBanco = nomeDoCampoNoBanco;
			Indice = indice;
			Get = getFunc;
			Set = setFunc;
		}

		public override String ToString()
		{
			return String.Format("{0}-{1}", Indice, NomeDoCampoNoBanco);
		}

		public void Preencher(Object instancia, IDataRecord dr)
		{
			Set((TEntidade)instancia, dr, Indice);
		}

		public Boolean ConfirmarNome(String nome)
		{
			return NomeDoCampoNoBanco.ToUpper() == nome.ToUpper();
		}

		public Object Obter(Object instancia)
		{
			return Get((TEntidade)instancia);
		}
	}

	public class Mapping : Mapping<Object, Object>
	{
		public Mapping(String nomeDoCampoNoBanco, Func<Object, Object> getFunc, Action<Object, IDataRecord, Int32> setFunc, Int32 indice)
			: base(nomeDoCampoNoBanco, getFunc, setFunc, indice) { }
	}
}
