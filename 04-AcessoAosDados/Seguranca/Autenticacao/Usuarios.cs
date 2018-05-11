using Dapper;
using MPSC.DomainDrivenDesign.Dominio.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Abstracao;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSC.DomainDrivenDesign.Infra.AcessoAosDados.Seguranca.Autenticacao
{
	public class Usuarios : Repositorio
	{
		public Usuario ObterPorIdComSenhas(Int32 usuarioId)
		{
			using (var multi = Conexao.QueryMultiple(cSelectUsuarioComSenhas, new { usuarioId = usuarioId }))
			{
				var usuario = multi.Read<Usuario>().Single();
				usuario.Preencher(multi.Read<Senha>());
				return usuario;
			}
		}

		public Usuario ObterPorId(Int32 usuarioId)
		{
			return Conexao.Query<Usuario>(cSelectUsuarioPorId, new { UsuarioId = usuarioId }).FirstOrDefault();
		}

		public Usuario ObterPorEMail(String eMail)
		{
			return Conexao.Query<Usuario>(cSelectUsuarioPorEMail, new { eMail = eMail }).FirstOrDefault();
		}

		public Usuario ObterPorCelular(String celular)
		{
			return Conexao.Query<Usuario>(cSelectUsuarioPorCelular, new { celular = celular }).FirstOrDefault();
		}

		public IEnumerable<Senha> BuscarSenhas(Int32 usuarioId)
		{
			return Conexao.Query<Senha>(cSelectSenhasDoUsuario, new { usuarioId = usuarioId }).ToArray();
		}

		public IEnumerable<Usuario> ObterPor(Usuario usuario)
		{
			return Conexao.Query<Usuario>(cSelectUsuarioPorEMail + " Union " + cSelectUsuarioPorCelular, usuario).ToArray();
		}

		public void Gravar(Usuario usuario)
		{
			usuario.EhValido();
			var execucao = Transacao.Proteger((transacao, log) =>
			{
				var senha = usuario.Senhas.FirstOrDefault();
				usuario.Id = Conexao.ExecuteScalar<Int32>(cInsertIntoUsuario + Conexao.RDBMS.CmdSqlUltimoIdGerado, usuario, transacao);
				senha.Id = Conexao.ExecuteScalar<Int64>(cInsertIntoSenha + Conexao.RDBMS.CmdSqlUltimoIdGerado, senha, transacao);
				return usuario;
			});

			AssegureQue.NaoHouveErro(execucao, "Houve um problema ao Gravar Usuario");
		}
		public void Gravar(Senha senha)
		{
			var execucao = Transacao.Proteger((transacao, log) =>
			{
				senha.Id = Conexao.ExecuteScalar<Int64>(cInsertIntoSenha, senha, transacao);
				return senha;
			});

			AssegureQue.NaoHouveErro(execucao, "Houve um problema ao Gravar Usuario");
		}

		private const String cSelectUsuarioPorEMail = @"
Select * From Usuario Usu Where (Usu.EMail = @eMail)";

		private const String cSelectUsuarioPorCelular = @"
Select * From Usuario Usu Where (Usu.Celular = @celular)";

		private const String cSelectUsuarioPorId = @"
Select * From Usuario Usu Where (Usu.Id = @usuarioId)";

		private const String cSelectSenhasDoUsuario = @"
Select * From Senha Sen Where (Sen.UsuarioId = @usuarioId)";

		private const String cSelectUsuarioComSenhas = cSelectUsuarioPorId + cSelectSenhasDoUsuario;

		private const String cInsertIntoUsuario = @"
Insert Into Usuario (Nome, EMail, Celular, Inclusao) Values (@nome, @eMail, @celular, @inclusao);";

		private const String cInsertIntoSenha = @"
Insert Into Senha(UsuarioId, SenhaCriptografada, Inclusao) Values (@usuarioId, @senhaCriptografada, @inclusao);";

	}
}