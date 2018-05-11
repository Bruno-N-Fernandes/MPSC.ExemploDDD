using MPSC.DomainDrivenDesign.Aplicacao.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;
using System.Windows.Forms;

namespace MPSC.DomainDrivenDesign.Apresentacao.WinForms.Seguranca.Autenticacao
{
	public partial class frmLogin : Form
	{
		public frmLogin()
		{
			InitializeComponent();
		}

		private void btCadastrar_Click(object sender, System.EventArgs e)
		{
			new frmUsuario().ShowDialog();
		}

		private void btTrocarSenha_Click(object sender, System.EventArgs e)
		{
			if (String.IsNullOrWhiteSpace(txtUsuario.Text))
				MessageBox.Show("Informe seu E-Mail antes de prosseguir");
			else if (String.IsNullOrWhiteSpace(txtSenha.Text))
				MessageBox.Show("Informe sua senha atual antes de prosseguir");
			else
				frmUsuario.TrocarSenha(txtUsuario.Text, txtSenha.Text);
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