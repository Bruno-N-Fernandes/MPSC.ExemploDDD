using MPSC.DomainDrivenDesign.Aplicacao.Seguranca.Autenticacao;
using MPSC.DomainDrivenDesign.Infra.Compartilhada.Abstacao.Utilitario;
using System;
using System.Windows.Forms;

namespace MPSC.DomainDrivenDesign.Apresentacao.WinForms.Seguranca.Autenticacao
{
	public partial class frmUsuario : Form
	{
		private String SenhaAntigaCriptografada = null;
		public frmUsuario()
		{
			InitializeComponent();
		}

		private void btConfirmar_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrWhiteSpace(SenhaAntigaCriptografada))
				CadastrarUsuario();
			else
				TrocarSenha();
		}

		private void btDesistir_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CadastrarUsuario()
		{
			var senhaCriptografada = Criptografia.Criptografar(txtSenha.Text);
			var confirmaSenhaCriptografada = Criptografia.Criptografar(txtConfirmacao.Text);
			try
			{
				var usuarioAPI = new UsuarioAPI();
				usuarioAPI.CadastrarUsuario(txtNome.Text, txtEMail.Text, txtCelular.Text, senhaCriptografada, confirmaSenhaCriptografada);
				MessageBox.Show("Operação realizada com sucesso!");
				Close();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Messages());
			}
		}

		private void TrocarSenha()
		{
			var senhaCriptografada = Criptografia.Criptografar(txtSenha.Text);
			var confirmaSenhaCriptografada = Criptografia.Criptografar(txtConfirmacao.Text);
			try
			{
				var usuarioAPI = new UsuarioAPI();
				usuarioAPI.TrocarSenha(txtEMail.Text, txtCelular.Text, SenhaAntigaCriptografada, senhaCriptografada, confirmaSenhaCriptografada);
				MessageBox.Show("Operação realizada com sucesso!");
				Close();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Messages());
			}
		}

		public static void TrocarSenha(String eMail, String senhaAtual)
		{
			var frmUsuario = new frmUsuario();
			frmUsuario.SenhaAntigaCriptografada = senhaAtual.Criptografar();
			frmUsuario.txtEMail.Text = eMail;
			frmUsuario.txtEMail.ReadOnly = true;
			frmUsuario.lblNome.Visible = false;
			frmUsuario.txtNome.Visible = false;
			frmUsuario.lblCelular.Visible = false;
			frmUsuario.txtCelular.Visible = false;
			frmUsuario.ShowDialog();
		}
	}
}