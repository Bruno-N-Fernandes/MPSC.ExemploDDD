namespace MPSC.DomainDrivenDesign.Apresentacao.WinForms.Seguranca.Autenticacao
{
	partial class frmLogin
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
			this.txtUsuario = new System.Windows.Forms.TextBox();
			this.txtSenha = new System.Windows.Forms.TextBox();
			this.lblUsuario = new System.Windows.Forms.Label();
			this.btTrocarSenha = new System.Windows.Forms.Button();
			this.btnLogin = new System.Windows.Forms.Button();
			this.lblSenha = new System.Windows.Forms.Label();
			this.btCadastrar = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtUsuario
			// 
			this.txtUsuario.Location = new System.Drawing.Point(52, 30);
			this.txtUsuario.Name = "txtUsuario";
			this.txtUsuario.Size = new System.Drawing.Size(179, 20);
			this.txtUsuario.TabIndex = 0;
			// 
			// txtSenha
			// 
			this.txtSenha.Location = new System.Drawing.Point(52, 56);
			this.txtSenha.Name = "txtSenha";
			this.txtSenha.PasswordChar = '*';
			this.txtSenha.Size = new System.Drawing.Size(114, 20);
			this.txtSenha.TabIndex = 1;
			// 
			// lblUsuario
			// 
			this.lblUsuario.AutoSize = true;
			this.lblUsuario.Location = new System.Drawing.Point(2, 33);
			this.lblUsuario.Name = "lblUsuario";
			this.lblUsuario.Size = new System.Drawing.Size(43, 13);
			this.lblUsuario.TabIndex = 2;
			this.lblUsuario.Text = "Usuario";
			// 
			// btTrocarSenha
			// 
			this.btTrocarSenha.Location = new System.Drawing.Point(142, 83);
			this.btTrocarSenha.Name = "btTrocarSenha";
			this.btTrocarSenha.Size = new System.Drawing.Size(89, 23);
			this.btTrocarSenha.TabIndex = 3;
			this.btTrocarSenha.Text = "Trocar Senha";
			this.btTrocarSenha.UseVisualStyleBackColor = true;
			this.btTrocarSenha.Click += new System.EventHandler(this.btTrocarSenha_Click);
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(172, 54);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(59, 23);
			this.btnLogin.TabIndex = 4;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// lblSenha
			// 
			this.lblSenha.AutoSize = true;
			this.lblSenha.Location = new System.Drawing.Point(2, 59);
			this.lblSenha.Name = "lblSenha";
			this.lblSenha.Size = new System.Drawing.Size(41, 13);
			this.lblSenha.TabIndex = 5;
			this.lblSenha.Text = "Senha:";
			// 
			// btCadastrar
			// 
			this.btCadastrar.Location = new System.Drawing.Point(52, 83);
			this.btCadastrar.Name = "btCadastrar";
			this.btCadastrar.Size = new System.Drawing.Size(84, 23);
			this.btCadastrar.TabIndex = 6;
			this.btCadastrar.Text = "Cadastre-se";
			this.btCadastrar.UseVisualStyleBackColor = true;
			this.btCadastrar.Click += new System.EventHandler(this.btCadastrar_Click);
			// 
			// frmLogin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(240, 118);
			this.Controls.Add(this.btCadastrar);
			this.Controls.Add(this.lblSenha);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.btTrocarSenha);
			this.Controls.Add(this.lblUsuario);
			this.Controls.Add(this.txtSenha);
			this.Controls.Add(this.txtUsuario);
			this.Name = "frmLogin";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtUsuario;
		private System.Windows.Forms.TextBox txtSenha;
		private System.Windows.Forms.Label lblUsuario;
		private System.Windows.Forms.Button btTrocarSenha;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Label lblSenha;
		private System.Windows.Forms.Button btCadastrar;
	}
}

