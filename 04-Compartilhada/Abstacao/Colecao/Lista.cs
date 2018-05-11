using System;
using System.Collections;
using System.Collections.Generic;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Colecao
{
	public class Lista<TItem> : IEnumerable<TItem>
	{
		private List<TItem> _lista;
		private Action<TItem> _onAdicionar;

		public Lista(Action<TItem> onAdicionar)
		{
			_onAdicionar = onAdicionar;
			_lista = new List<TItem>();
		}

		public virtual void Adicionar(TItem item)
		{
			if (_onAdicionar != null)
				_onAdicionar(item);
			_lista.Add(item);
		}

		public virtual Int32 Count { get { return _lista.Count; } }

		IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
		{
			return _lista.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _lista.GetEnumerator();
		}
	}
}