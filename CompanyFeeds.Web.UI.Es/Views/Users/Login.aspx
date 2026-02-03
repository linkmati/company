<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Users.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Login</h1>
    <uc:HtmlContainer runat="server" id="pnlForm">
		<p>
			Por favor, introduce tu <strong>email y contraseña</strong>.
			<br /><br />
			Si no tienes una cuenta aún, <%= Html.ActionLinkFormatted("regístrate aquí", "Edit") %>.
		</p>
		<div id="fullForm">
			<div class="validationErrors">
				<%=Html.ValidationSummary(new Dictionary<string, object>
					{
						{"Email", new Dictionary<ValidationErrorType, string>
							{ 
								{ValidationErrorType.CompareNotMatch, "Email y/o contraseña incorrectas."}
								,{ValidationErrorType.Format, "El formato del campo email no es válido."}
							}
						}
					}, null)%>
			</div>
			<% Html.BeginForm();%>
			<div class="item">
				<label for="Email">Email</label>
				<%=Html.TextBox("Email")%>
			</div>
			<div class="item">
				<label for="Password">Contraseña</label>
				<%=Html.Password("Password")%>
			</div>
			<br />
			<div><input type="submit" class="button" value="Login &gt;&gt;" /></div>
			<% Html.EndForm();%>
		</div>
	</uc:HtmlContainer>
	<uc:HtmlContainer runat="server" id="pnlValidateEmail" visible="false">
		<p><em>Necesitas validar tu dirección de email</em>.</p>
		<p>Te hemos enviado un email a <%=User.Email %> conteniendo un link en el que debes pinchar para verificar tu cuenta.</p>
	</uc:HtmlContainer>
</asp:Content>
