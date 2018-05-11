namespace MPSC.DomainDrivenDesign.Apresentacao.WinForms.Seguranca.Autenticacao
{
	partial class frmUsuario
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblNome = new System.Windows.Forms.Label();
			this.lblEMail = new System.Windows.Forms.Label();
			this.lblCelular = new System.Windows.Forms.Label();
			this.lblSenha = new System.Windows.Forms.Label();
			this.lblConfirmacaoSenha = new System.Windows.Forms.Label();
			this.txtNome = new System.Windows.Forms.TextBox();
			this.txtEMail = new System.Windows.Forms.TextBox();
			this.txtCelular = new System.Windows.Forms.TextBox();
			this.txtSenha = new System.Windows.Forms.TextBox();
			this.txtConfirmacao = new System.Windows.Forms.TextBox();
			this.btDesistir = new System.Windows.Forms.Button();
			this.btConfirmar = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblNome
			// 
			this.lblNome.AutoSize = true;
			this.lblNome.Location = new System.Drawing.Point(16, 28);
			this.lblNome.Name = "lblNome";
			this.lblNome.Size = new System.Drawing.Size(38, 13);
			this.lblNome.TabIndex = 0;
			this.lblNome.Text = "Nome:";
			// 
			// lblEMail
			// 
			this.lblEMail.AutoSize = true;
			this.lblEMail.Location = new System.Drawing.Point(15, 54);
			this.lblEMail.Name = "lblEMail";
			this.lblEMail.Size = new System.Drawing.Size(39, 13);
			this.lblEMail.TabIndex = 1;
			this.lblEMail.Text = "E-Mail:";
			// 
			// lblCelular
			// 
			this.lblCelular.AutoSize = true;
			this.lblCelular.Location = new System.Drawing.Point(12, 78);
			this.lblCelular.Name = "lblCelular";
			this.lblCelular.Size = new System.Drawing.Size(42, 13);
			this.lblCelular.TabIndex = 2;
			this.lblCelular.Text = "Celular:";
			// 
			// lblSenha
			// 
			this.lblSenha.AutoSize = true;
			this.lblSenha.Location = new System.Drawing.Point(11, 106);
			this.lblSenha.Name = "lblSenha";
			this.lblSenha.Size = new System.Drawing.Size(41, 13);
			this.lblSenha.TabIndex = 3;
			this.lblSenha.Text = "Senha:";
			// 
			// lblConfirmacaoSenha
			// 
			this.lblConfirmacaoSenha.AutoSize = true;
			this.lblConfirmacaoSenha.Location = new System.Drawing.Point(134, 106);
			this.lblConfirmacaoSenha.Name = "lblConfirmacaoSenha";
			this.lblConfirmacaoSenha.Size = new System.Drawing.Size(69, 13);
			this.lblConfirmacaoSenha.TabIndex = 4;
			this.lblConfirmacaoSenha.Text = "Confirmação:";
			// 
			// txtNome
			// 
			this.txtNome.Location = new System.Drawing.Point(58, 25);
			this.txtNome.Name = "txtNome";
			this.txtNome.Size = new System.Drawing.Size(214, 20);
			this.txtNome.TabIndex = 5;
			// 
			// txtEMail
			// 
			this.txtEMail.Location = new System.Drawing.Point(58, 51);
			this.txtEMail.Name = "txtEMail";
			this.txtEMail.Size = new System.Drawing.Size(214, 20);
			this.txtEMail.TabIndex = 6;
			// 
			// txtCelular
			// 
			this.txtCelular.Location = new System.Drawing.Point(58, 77);
			this.txtCelular.Name = "txtCelular";
			this.txtCelular.Size = new System.Drawing.Size(214, 20);
			this.txtCelular.TabIndex = 7;
			// 
			// txtSenha
			// 
			this.txtSenha.Location = new System.Drawing.Point(58, 103);
			this.txtSenha.Name = "txtSenha";
			this.txtSenha.PasswordChar = '*';
			this.txtSenha.Size = new System.Drawing.Size(70, 20);
			this.txtSenha.TabIndex = 8;
			// 
			// txtConfirmacao
			// 
			this.txtConfirmacao.Location = new System.Drawing.Point(209, 103);
			this.txtConfirmacao.Name = "txtConfirmacao";
			this.txtConfirmacao.PasswordChar = '*';
			this.txtConfirmacao.Size = new System.Drawing.Size(63, 20);
			this.txtConfirmacao.TabIndex = 9;
			// 
			// btDesistir
			// 
			this.btDesistir.Location = new System.Drawing.Point(115, 129);
			this.btDesistir.Name = "btDesistir";
			this.btDesistir.Size = new System.Drawing.Size(75, 23);
			this.btDesistir.TabIndex = 10;
			this.btDesistir.Text = "Desistir";
			this.btDesistir.UseVisualStyleBackColor = true;
			this.btDesistir.Click += new System.EventHandler(this.btDesistir_Click);
			// 
			// btConfirmar
			// 
			this.btConfirmar.Location = new System.Drawing.Point(196, 129);
			this.btConfirmar.Name = "btConfirmar";
			this.btConfirmar.Size = new System.Drawing.Size(75, 23);
			this.btConfirmar.TabIndex = 11;
			this.btConfirmar.Text = "Confirmar";
			this.btConfirmar.UseVisualStyleBackColor = true;
			this.btConfirmar.Click += new System.EventHandler(this.btConfirmar_Click);
			// 
			// frmUsuario
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 165);
			this.Controls.Add(this.btConfirmar);
			this.Controls.Add(this.btDesistir);
			this.Controls.Add(this.txtConfirmacao);
			this.Controls.Add(this.txtSenha);
			this.Controls.Add(this.txtCelular);
			this.Controls.Add(this.txtEMail);
			this.Controls.Add(this.txtNome);
			this.Controls.Add(this.lblConfirmacaoSenha);
			this.Controls.Add(this.lblSenha);
			this.Controls.Add(this.lblCelular);
			this.Controls.Add(this.lblEMail);
			this.Controls.Add(this.lblNome);
			this.Name = "frmUsuario";
			this.Text = "Usuario";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblNome;
		private System.Windows.Forms.Label lblEMail;
		private System.Windows.Forms.Label lblCelular;
		private System.Windows.Forms.Label lblSenha;
		private System.Windows.Forms.Label lblConfirmacaoSenha;
		private System.Windows.Forms.TextBox txtNome;
		private System.Windows.Forms.TextBox txtEMail;
		private System.Windows.Forms.TextBox txtCelular;
		private System.Windows.Forms.TextBox txtSenha;
		private System.Windows.Forms.TextBox txtConfirmacao;
		private System.Windows.Forms.Button btDesistir;
		private System.Windows.Forms.Button btConfirmar;
	}
}