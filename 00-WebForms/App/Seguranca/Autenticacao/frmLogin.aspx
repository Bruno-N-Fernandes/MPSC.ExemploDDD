<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="MPSC.DomainDrivenDesign.App.WebForms.App.Seguranca.Autenticacao.frmLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="VBox">
		<div class="HBox">
			<label id="lblUsuario" class="LabelField" for="txtUsuario" >Usuario:</label>
			<asp:TextBox runat="server" ID="txtUsuario" CssClass="InputText" />
		</div>
		<div class="HBox">
			<label id="lblSenha" class="LabelField" for="txtSenha" >Senha:</label>
			<asp:TextBox runat="server" ID="txtSenha" TextMode="Password" CssClass="InputPwd" />
			<asp:Button runat="server" ID="btnLogin" Text="Login" OnClick="btnLogin_Click" />
		</div>
		<div class="HBox">
			<div class="HSpace100"></div>
			<asp:Button runat="server" ID="btnCadastrar" Text="Cadastre-se" />
			<asp:Button runat="server" ID="btnTrocarSenha" Text="Trocar Senha" />
		</div>
	</div>
</asp:Content>