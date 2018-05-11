using System;

namespace MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Dominio
{
	public abstract class ClasseBase<TId> where TId : struct, IComparable, IFormattable, IConvertible, IComparable<TId>, IEquatable<TId>
	{
		public TId Id { get; set; }
		public virtual void EhValido() { }
	}

	public abstract class Tipo : ClasseBase<Int16>
	{
		public String Sigla { get; set; }
		public String Descricao { get; set; }
		public Int16 OrdemApresentacao { get; set; }
	}

	public abstract class Cadastro : ClasseBase<Int32>
	{
		public DateTime Inclusao { get; set; }
		public DateTime? Alteracao { get; set; }
		public String LoginInclusao { get; set; }
		public String LoginAlteracao { get; set; }
	}

	public abstract class Entidade : ClasseBase<Int64>
	{
		public DateTime Inclusao { get; set; }
		public DateTime? Alteracao { get; set; }
		public String LoginInclusao { get; set; }
		public String LoginAlteracao { get; set; }
	}
}