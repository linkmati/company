<%@ Page Title="Submit company" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="false" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Companies.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<% PageTitle = Model == null ? "Enviar empresa" : "Editar detalle de empresa"; %>
	<script type="text/javascript" src="/scripts/site.externalqueries.js"></script>
	<script src="//tinymce.cachefly.net/4.0/tinymce.min.js"></script>
	<script type="text/javascript">
		tinymce.init({
			selector: "#Description",
			content_css: "/styles/common.css?2014",
			statusbar: false,
			menubar : false,
			height: 220,
			plugins: "link"
		});
	</script>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="companyEdit">
		<h1><%=PageTitle %></h1>
		<div id="fullForm">
			<div id="divExternalSources"></div>
			<% Html.BeginForm("Edit", "CompaniesController", FormMethod.Post, new {enctype="multipart/form-data",onsubmit="return true;",id="companyForm"});%>
			<%= Html.AntiForgeryToken() %>
			<div class="validationErrors">
				<%=Html.ValidationSummary("<h3>Por favor, revisa los siguiente errores:</h3>", new Dictionary<string, object>
				{
					{"Name", "El campo nombre no puede estar vacío."}
					,{"Url", "El campo web no puede estar vacío."}
					,{"Description", "El campo descripción no puede estar vacío."}
					,{"FeedUrl", "El campo dirección rss no es válido."}
					,{"CategoryId", "El campo sector no puede estar vacío."}
					,{"Logo", "El fichero introducido en el campo Logo no es válido. Selecciona un logo en formato jpg/gif/png de un tamaño menor a 500kb."}
					,{"Tag", new Dictionary<ValidationErrorType, string>()
						{ 
							{ValidationErrorType.DuplicateNotAllowed, "La empresa ya existe."}
							,{ValidationErrorType.Format, ""},{ValidationErrorType.MinLength, ""},{ValidationErrorType.NullOrEmpty, ""}
						}
					}
				}, null)%>
			</div>
			<div class="item">
				<label for="Name">Nombre</label>
				<%=Html.TextBox("Name")%>
			</div>
			<div class="item">
				<label for="Url">Web</label>
				<%=Html.TextBox("Url")%>
			</div>
			<div class="item">
				<label for="CategoryId">Sector</label>
				<%=Html.DropDownListDefault("CategoryId", new SelectList(this.Cache.Categories, "CategoryId", "CategoryName", Model != null ? Model.CategoryId : 0), "", "")%>
			</div>
<% 
			if (User.Role == UserRole.Admin)
			{
%>
			<div class="item">
				<label for="FeedUrl" class="optional">Dirección rss</label>
				<%=Html.TextBox("FeedUrl")%>
			</div>
<%
			}
%>
			<div class="content">
				<label for="Description">Descripción</label>
				<%=Html.TextArea("Description")%>
			</div>
			<br />
			<input type="submit" value="Enviar &gt;&gt;" class="button" />
			
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>
