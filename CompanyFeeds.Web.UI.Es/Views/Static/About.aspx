<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Acerca prsync.es</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   	<h1>Acerca prsync.es</h1>
	<p><em>Prsync.es</em> es un sitio de distribución de noticias y notas de prensa empresariales.</p> 
	<p>El contenido de las notas de prensa y la información de las empresas en <em>prsync.es</em> es enviado y editado por los usuarios. Además, si se provee una dirección rss de las notas de prensa de una empresa, un robot visitará la dirección para buscar nuevas notas de prensa.</p> 
	<p>Enviar notas de prensa en <em>prsync.es</em> es - y siempre será - gratis.</p> 
	
	<p><%= Html.ActionLinkFormatted("Empieza aquí >>", "Edit", "Users") %></p>
	<h2>Logotipos</h2>
	<p>Formatos disponibles para el logo de prsync.es:</p>
	<ul>
		<li><a href="/images/logo_high.png">PNG (8 bits)</a></li>
		<li><a href="/images/logo.eps">EPS</a></li>
		<li><a href="/images/logo.svg">SVG</a></li>
	</ul>
	<p style="text-align: center;"><em>Vista preliminar</em></p>
	<div class="siteLogo" style="text-align: center;"><img src="/images/logowhite.gif" alt="white logo" /></div>
</asp:Content>
