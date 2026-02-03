<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="fullForm">
	<% Html.BeginForm();%>
	<%=Html.AntiForgeryToken() %>
	<div class="validationErrors">
		<%=Html.ValidationSummary("<h3>Por favor, verifica los siguientes errores:</h3>", new Dictionary<string, object>
		{
			{"name", "El campo nombre no puede estar vacío."}
			,{"email", "El campo email es un dato obligatorio y debe contener un formato válido."}
			,{"message", "El campo mensaje no puede estar vacío."}
		}, null)%>
	</div>
	<div class="item">
		<label for="name">Nombre</label>
		<%=Html.TextBox("name")%>
	</div>
	<div class="item">
		<label for="email">Email</label>
		<%=Html.TextBox("email")%>
	</div>
	<div class="itemTextarea floatContainer">
		<label for="message">Mensaje</label>
		<%=Html.TextArea("message")%>
	</div>
	<br />
	<div><input type="submit" class="button" value="Enviar &gt;&gt;" /></div>
	<% Html.EndForm();%>
</div>