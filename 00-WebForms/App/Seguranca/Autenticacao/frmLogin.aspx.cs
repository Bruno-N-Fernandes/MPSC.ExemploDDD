using MPSC.DomainDrivenDesign.Aplicacao.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Apresentacao.WebForms.App.SharedControls;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;

namespace MPSC.DomainDrivenDesign.Apresentacao.WebForms.App.Seguranca.Autenticacao
{
	public partial class frmLogin : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			var senhaCriptografada = Criptografia.Criptografar(txtSenha.Text);
			try
			{
				var usuarioAPI = new UsuarioAPI();
				usuarioAPI.Autenticar(txtUsuario.Text, senhaCriptografada);
				MessageBox.Show("Usuário autenticado com sucesso");
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Messages());
			}
		}
	}
}