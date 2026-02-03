<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Users.Edit" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<%PageTitle = Model != null && Model.Id > 0 ? "Mi cuenta" : "Registro de usuario"; %>
	<script type="text/javascript">
		function showAdditional(anchor)
		{
			anchor = $(anchor);
			$('#additionalInfo').slideDown();
			anchor.html(anchor.html().substr(4));
			anchor.unbind("click");
			return false;
		}
	</script>
	<title><%=PageTitle %></title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<asp:Panel runat="server" ID="pnlShowAdditionalOnLoad" Visible="false">
		<script type="text/javascript">
			$(document).ready(function(){showAdditional($("#showAdditionalAnchor"))})
		</script>
	</asp:Panel>
	<div id="register">
		<h1><%=PageTitle %></h1>
		<asp:Panel runat="server" ID="pnlUpdate" Visible="false">
			<p class="statusMessage">Tu perfil ha sido actualizado.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlRegister">
			<p>¿Ya estás registrado? <%=Html.ActionLinkFormatted("Haz login", "Login") %>.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlNoAgency" Visible="false">
			<p>Si eres de una <strong>agencia de medios</strong>, por favor <%=Html.ActionLinkFormatted("empieza aquí", "Edit", "Agencies") %>.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlAgency" Visible="false">
			<p><strong>Rellena tus datos de usuario perteneciente a la agencia</strong>.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlPremium" Visible="false">
			<p>Tu suscripción Premium vence en <%=Model.AgencyPremiumDate%></p>
			<p>Para renovar o cancelar tu suscripción, entra en <a href="https://www.paypal.com">Paypal</a></p>
		</asp:Panel>
		<div id="fullForm">
			<% Html.BeginForm("Register", "UsersController", FormMethod.Post, new {onsubmit="return true;"});%>
			<%= Html.Hidden("AgencyId") %>
			<div class="validationErrors">
				<%=Html.ValidationSummary(new Dictionary<string, object>
				{
					{"Email", new Dictionary<ValidationErrorType, string>
						{ 
							{ValidationErrorType.NullOrEmpty, "El campo email no puede estar vacío."}
							,{ValidationErrorType.Format, "El formato del campo email no es válido."}
							,{ValidationErrorType.DuplicateNotAllowed, "La dirección de email ya está siendo usada."}  
						}
					}
					,{"Password", "El campo contraseña no puede estar vacío y debe contener un mínimo de 4 caracteres."}
					,{"Name", "El campo nombre no puede estar vacío y debe contener un mínimo de 4 caracteres."}
					,{"passwordConfirm", "Las contraseñas no concuerdan."}
					,{"Birthday", "La fecha de nacimiento no es válida."}
				}, null)%>
			</div>
			<div class="item">
				<label for="email">Email</label>
				<%=Html.TextBox("Email") %>
			</div>
			<p class="note" id="pnlPasswordChange" visible="false" runat="server">Rellena los campos de contraseña solo si deseas modificarla.</p>
			<div class="item">
				<label for="password">Contraseña</label>
				<%=Html.Password("Password") %>
			</div>
			<div class="item">
				<label for="passwordConfirm">Confirma contraseña</label>
				<%=Html.Password("PasswordConfirm") %>
			</div>
			<div class="item">
				<label for="userName">Nombre público</label>
				<%=Html.TextBox("Name") %>
			</div>
			<div class="item">
				<label for="country">País</label>
				<%=Html.DropDownList("CountryCode", new SelectList((IEnumerable) ViewData["Countries"], "CountryCode", "CountryName", Model.CountryCode!=null?Model.CountryCode:"ES")) %>
			</div>
			<h2><a id="showAdditionalAnchor" href="#" onclick="return showAdditional(this);">(+) Información adicional</a></h2>
			<div id="additionalInfo" style="display:none;">
				<div class="item">
					<label for="gender">Sexo</label>
					<%=Html.DropDownList("Gender")%>
				</div>
				<p class="note">Formato: DD/MM/YYYY</p>
				<div class="item">
					<label for="birthDay">Fecha de nacimiento</label>
					<%=Html.TextBox("Birthday", "{0:d}", typeof(DateTime), null) %>
				</div>
			</div>
			<asp:Panel runat="server" ID="pnlTerms" Visible="false">
				<br />
				<p>Al enviar el formulario, acepta los <%=Html.ActionLinkFormatted("términos del servicio", "Terms", "Home") %>.</p>
			</asp:Panel>
			<br />
			<input type="submit" value="Enviar &gt;&gt;" class="button" />
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>