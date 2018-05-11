using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario
{
	public static class AssegureQue
	{
		private static Exception newException(String assertiva, Exception innerException, String mensagem, params Object[] args)
		{
			return new DomainException(String.Format(mensagem, args), innerException);
		}

		public static void EhNulo(Object target, String mensagem, params Object[] args)
		{
			if (target != null)
				throw newException("EhNulo", null, mensagem, args);
		}
		public static void NaoEhNulo(Object target, String mensagem, params Object[] args)
		{
			if (target == null)
				throw newException("NaoEhNulo", null, mensagem, args);
		}

		public static void EhVerdadeiro(Boolean target, String mensagem, params Object[] args)
		{
			if (!target)
				throw newException("EhVerdadeiro", null, mensagem, args);
		}

		public static void EhFalso(Boolean target, String mensagem, params Object[] args)
		{
			if (target)
				throw newException("EhFalso", null, mensagem, args);
		}

		public static void EhVazio(String target, String mensagem, params Object[] args)
		{
			if (!String.IsNullOrWhiteSpace(target))
				throw newException("NaoEhVazio", null, mensagem, args);
		}

		public static void NaoEhVazio(String target, String mensagem, params Object[] args)
		{
			if (String.IsNullOrWhiteSpace(target))
				throw newException("NaoEhVazio", null, mensagem, args);
		}

		public static void EhVazio<T>(IEnumerable<T> target, String mensagem, params Object[] args)
		{
			if ((target != null) && target.Any())
				throw newException("EhVazio", null, mensagem, args);
		}

		public static void NaoEhVazio<T>(IEnumerable<T> target, String mensagem, params Object[] args)
		{
			if ((target == null) || !target.Any())
				throw newException("NaoEhVazio", null, mensagem, args);
		}


		public static void EhIgual<T>(T obj1, T obj2, String mensagem, params Object[] args) where T : IComparable<T>
		{
			if (((obj1 == null) && (obj2 != null)) || ((obj1 != null) && (obj2 == null)))
				throw newException("EhIgual", null, mensagem, args);

			if (obj1.CompareTo(obj2) != 0)
				throw newException("EhIgual", null, mensagem, args);
		}

		public static void EhDiferente<T>(T obj1, T obj2, String mensagem, params Object[] args) where T : IComparable<T>
		{
			if ((obj1 == null) && (obj2 == null))
				throw newException("EhDiferente", null, mensagem, args);

			if (obj1.CompareTo(obj2) == 0)
				throw newException("EhDiferente", null, mensagem, args);
		}

		public static void NaoHouveErro(Execucao execucao, String mensagem, params Object[] args)
		{
			if (execucao.HouveErro || (execucao.Exception != null))
				throw newException("NaoHouveErro", execucao.Exception, mensagem, args);
		}
	}
}