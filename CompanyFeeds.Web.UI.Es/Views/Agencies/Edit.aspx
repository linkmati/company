<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Agencies.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%PageTitle = Model == null ? "Agencias de comunicación y medios" : "Editar información de la agencia"; %>
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
	<title><%=PageTitle%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
		<div id="agencyEdit">
		<h1><%=PageTitle%></h1>
		<asp:Panel id="pnlNoMember" runat="server">
			<p>Registra tu agencia de comunicación en prsync.es y podrás enviar notas de prensa de tus clientes</p>
			<br /><br />
			<h2>Registrar agencia</h2>
		</asp:Panel>

		<div id="fullForm">
			<div id="divExternalSources"></div>
			<% Html.BeginForm("Edit", "Agencies", new{agencyTag=ViewData["AgencyTag"]}, FormMethod.Post, new {enctype="multipart/form-data",onsubmit="return true;",id="agencyForm"});%>
			<div class="validationErrors"><%=Html.ValidationSummary("<h3>Por favor, verifica los siguientes errores:</h3>", new Dictionary<string, object>
				{
					{"Name", "El campo nombre de la agencia no puede estar vacío."}
					,{"Url", "El campo Web no puede estar vacío y debe contener una dirección válida."}
					,{"Email", "El formato del campo email no es válido."}
					,{"Description", "El campo descripción no puede estar vacío."}
					,{"Logo", "El fichero introducido en el campo Logo no es válido. Selecciona un logo en formato jpg/gif/png de un tamaño menor a 500kb."}
					,{"Tag", new Dictionary<ValidationErrorType, string>()
						{ 
							{ValidationErrorType.DuplicateNotAllowed, "La agencia ya existe."}
							,{ValidationErrorType.Format, ""},{ValidationErrorType.MinLength, ""},{ValidationErrorType.NullOrEmpty, ""}
						}
					}
				}, null)%></div>
			<div class="item">
				<label for="Name">Nombre de la agencia</label>
				<%=Html.TextBox("Name")%>
			</div>
			<div class="item">
				<label for="Url">Web</label>
				<%=Html.TextBox("Url") %>
			</div>
			<div class="item">
				<label for="Email" class="optional">Email público</label>
				<%=Html.TextBox("Email")%>
			</div>
			<div class="item">
				<label for="Phone" class="optional">Teléfono</label>
				<%=Html.TextBox("Phone")%>
			</div>
			<%--
			<div class="item">
				<label for="Logo" class="optional">Logo</label>
				<input type="file" name="Logo" id="Logo" />
			</div>
			--%>
			<div class="content">
				<label for="Description">Descripción</label>
				<%=Html.TextArea("Description")%>
			</div>			
			<div style="padding: 10px 100px 10px 0;">
				<p class="note">Si lo marca, el nombre de la Agencia no se mostrará como autor.</p>
				<div class="item check">
					<label for="AllowComments">Ocultar como autor</label>
					<%=Html.CheckBox("HideAuthor")%>
				</div>
			</div>
			<input type="submit" value="Enviar &gt;&gt;" class="button" />
			
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>
