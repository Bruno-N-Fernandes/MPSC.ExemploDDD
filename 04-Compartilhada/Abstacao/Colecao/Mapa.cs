using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Colecao
{
	public class Mapa<TKey, TValue>
	{
		private readonly Mapeador.KVP<TKey, TValue>[] _lista;
		private Int32 _ultimo = -1;
		internal Mapa(Mapeador.KVP<TKey, TValue>[] itens) { _lista = itens; }

		public TValue this[TKey key] { get { return Find(key, _lista.Length - 1); } }

		private TValue Find(TKey key, Int32 max)
		{
			_ultimo = (_ultimo + 1) % _lista.Length;
			var kvp = _lista[_ultimo];
			return kvp.Key.Equals(key) ? kvp.Value : ((max > 0) ? Find(key, max - 1) : default(TValue));
		}
	}

	public static class Mapeador
	{
		public static MapaBuilderGenerico<TKey, TValue> De<TKey, TValue>()
		{
			return new MapaBuilderGenerico<TKey, TValue>();
		}

		public static MapaBuilderDeCampos<String> DeCampos()
		{
			return new MapaBuilderDeCampos<String>();
		}

		internal class KVP<TKey, TValue>
		{
			public readonly TKey Key;
			public readonly TValue Value;
			public KVP(TKey key, TValue value)
			{
				Key = key;
				Value = value;
			}
		}

		public class MapaBuilderAbstrato<TKey, TValue>
		{
			internal readonly List<KVP<TKey, TValue>> lista = new List<KVP<TKey, TValue>>();

			public Mapa<TKey, TValue> Criar()
			{
				var itens = lista.Where(kvp => (kvp != null) && (kvp.Key != null)).ToArray();
				if (lista.Count > itens.Select(kvp => kvp.Key).Distinct().Count())
					throw new InvalidOperationException("Não mapeie chaves nulas");
				lista.RemoveAll(i => true);
				return new Mapa<TKey, TValue>(itens);
			}
		}

		public class MapaBuilderGenerico<TKey, TValue> : MapaBuilderAbstrato<TKey, TValue>
		{
			public MapaBuilderGenerico<TKey, TValue> Mapear(TKey key, TValue value)
			{
				lista.Add(new KVP<TKey, TValue>(key, value));
				return this;
			}
		}

		public class MapaBuilderDeCampos<TKey> : MapaBuilderAbstrato<TKey, Int32>
		{
			public MapaBuilderDeCampos<TKey> Mapear(params TKey[] keys)
			{
				var posicao = 0;
				foreach (var key in keys)
					lista.Add(new KVP<TKey, Int32>(key, posicao++));
				return this;
			}
		}
	}
}