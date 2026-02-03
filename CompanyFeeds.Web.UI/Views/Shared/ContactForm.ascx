<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="fullForm">
	<% Html.BeginForm();%>
	<%=Html.AntiForgeryToken() %>
	<div class="validationErrors">
		<%=Html.ValidationSummary("<h3>Please check the following errors:</h3>", new Dictionary<string, object>
		{
			{"name", "Name must not be blank."}
			,{"email", "Email must be a valid email address."}
			,{"message", "Message must not be blank."}
		    ,{"captcha", "Captcha verification failed."}
		}, null)%>
	</div>
	<div class="item">
		<label for="name">Name</label>
		<%=Html.TextBox("name")%>
	</div>
	<div class="item">
		<label for="email">Email</label>
		<%=Html.TextBox("email")%>
	</div>
	<div class="itemTextarea floatContainer">
		<label for="message">Message</label>
		<%=Html.TextArea("message")%>
	</div>
    <div>
        <div class="g-recaptcha" data-sitekey="6Lc7F5gUAAAAAAG6HkeFhme6wp4SAuDJbWuMJy5P"></div>
    </div>
	<br />
	<div><input type="submit" class="button" value="Send &gt;&gt;" /></div>
	<% Html.EndForm();%>
</div>