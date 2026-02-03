<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="RegisterDone.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Users.RegisterDone" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<title>Gracias por registrarte</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Gracias por registrarte</h1>
	<p>Te hemos enviado un email a la direccion <strong><%=User.Email %></strong> conteniendo un link en el que debes pinchar para verificar tu cuenta</p>
	<p>Deberías recibir el email en los próximos minutos.</p>
	<br /><br />
	<h3>¿No has recibido el email?</h3>
	<p>Razones mas comunes:</p>
	<ul>
		<li>En algunos casos, el email puede tardar en llegar.</li>
		<li>Revisa tu carpeta de correo no deseado / spam, el correo puede haber sido filtrado como spam.</li>
		<li>Verifica (arriba) si has escrito la dirección de email correctamente. Si es errónea, <%=Html.ActionLinkFormatted("regístrate nuevamente", "Edit", "Users") %>.</li>
		<li><%=Html.ActionLinkFormatted("Contáctanos", "Contact", "Feedback") %> si no lo puedes recibir y te reenviaremos el correo electrónico.</li>
	</ul>
	<br />
</asp:Content>
