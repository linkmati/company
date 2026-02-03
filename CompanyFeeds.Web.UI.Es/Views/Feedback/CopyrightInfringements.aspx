<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.Es.Views.Feedback.Contact" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Reporting Copyright Infringements</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Infracciones de derechos de autor</h1>
	<div class="copyright">
		<p>Prsync ha creado este formulario para recibir una notificación de presunta infracción del copyright que ocurre en el dominio prsync.es. Si usted cree que su trabajo protegido es objeto de infracción, notifique a nuestro equipo rellenando el siguiente formulario. </p>
		<p>Incluya la siguiente información: </p>
		<ul>
			<li> Una firma física o electrónica del propietario del copyright o la persona autorizada para actuar en su nombre; </li>
			<li> Una descripción del trabajo protegido por copyright afirmaron haber sido violadas; </li>
			<li> Una descripción del material infractor y la información razonablemente suficiente para permitirnos localizar el material; </li>
			<li> su información de contacto, incluyendo su dirección, número de teléfono y correo electrónico; </li>
			<li> Una declaración suya de que usted cree de buena fe que el uso del material descrito en la reclamación de no está autorizado por el propietario del copyright, su agente o la ley; y </li>
			<li> Una declaración que la información en la notificación es exacta y, bajo las penas y sanciones de perjurio, que usted está autorizado para actuar en nombre del propietario del copyright. </li>
		</ul>
	</div>
	<div id="contact">
		<% Html.RenderPartial("ContactForm"); %>
	</div>
</asp:Content>