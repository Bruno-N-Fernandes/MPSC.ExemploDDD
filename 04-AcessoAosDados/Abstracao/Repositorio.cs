using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.AcessoAosDados;

namespace MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Abstracao
{
	public class Repositorio
	{
		protected readonly Conexao Conexao;
		public Repositorio()
		{
			Conexao = Conexao.Atual;
		}
	}
}