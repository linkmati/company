<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Entries.Edit" %>
<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
	<% PageTitle = Model == null ? "Enviar nota de prensa" : "Editar nota de prensa"; %>
	<script src="/scripts/jquery-ui-1.8.13.core-all.min.js" type="text/javascript"></script>
	<script src="/scripts/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
	<script src="/scripts/resources.js" type="text/javascript"></script>
	<title><%=PageTitle %></title>
	<meta name="description" content="Envía una nota de prensa de tu empresa gratis." />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div id="entryEdit">
		<% Html.RenderPartial("EditScripts"); %>
		<h1><%=PageTitle %></h1>
		<div id="fullForm">
			<% Html.BeginForm("Edit", "EntriesController", FormMethod.Post, new {id="entryForm"});%>
			<%= Html.AntiForgeryToken() %>
			<div class="validationErrors">
				<%=Html.ValidationSummary("<h3>Por favor, revisa los siguientes errores:</h3>", new Dictionary<string, object>
				{
					{"EntryTitle", "El campo título no puede estar vacío y debe contener por lo menos una palabra."}
					,{"CompanyName", "El campo empresa no puede estar vacío."}
					,{"Content", "El campo contenido no puede estar vacío."}
					,{"Teaser", Model!=null?"El campo descripción breve no puede estar vacío.":""}
					,{"Tag", ""}
				}, null)%>
			</div>
			<div class="item title">
				<label for="EntryTitle">Título</label>
				<%=Html.TextBox("EntryTitle") %>
			</div>
			<div class="item">
				<label for="CompanyName">Empresa</label>
				<%=Html.TextBox("CompanyName", null, Model!=null?new{disabled="disabled"}:null ) %>
				<%=Html.Hidden("CompanyId") %>
			</div>
			<div class="content">
				<label for="Content">Contenido</label>
				<%=Html.TextArea("Content") %>
			</div>
			<div class="itemTextarea">
				<label for="Teaser">Descripción breve</label>
<%				
				if (Model != null || (ModelState["Teaser"] != null && ModelState["Teaser"].Value.RawValue != ""))
				{
%>
					<%=Html.TextArea("Teaser") %>
<%
				}
				else
				{
%>
					<%=Html.TextArea("Teaser", "(auto-generado)", new {disabled="disabled"}) %>
					<span class="enable"><a href="#" onclick="$('#Teaser').prop('disabled',false).focus().val('');$(this).hide();return false;">Añadir descripción breve</a></span>
<%
				}
%>
			</div>
			<div class="itemTextarea">
				<label for="ContactInfo" class="optional">Información de contacto</label>
				<%=Html.TextArea("ContactInfo") %>
			</div>
			<UC:HtmlContainer runat="server" id="pnlAllowComments">
				<p class="note">Determina si los usuarios pueden añadir comentarios a este comunicado.</p>
				<div class="item check">
					<label for="AllowComments">Permitir comentarios</label>
					<%=Html.CheckBox("AllowComments", Model != null ? Model.AllowComments.GetValueOrDefault(true) : true)%>
				</div>
			</UC:HtmlContainer>
			<br />
			<input type="submit" value="Enviar &gt;&gt;" class="button" />
				
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>
