<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Users.Edit" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<%PageTitle = Model != null && Model.Id > 0 ? "Account" : "Register"; %>
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
			<p class="statusMessage">Your profile have been updated.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlRegister">
			<p>Already registered or want to make changes to your account? <%=Html.ActionLinkFormatted("Sign in", "Login") %>.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlNoAgency" Visible="false">
			<p><strong>If you are from an agency please <%=Html.ActionLinkFormatted("start here", "Edit", "Agencies") %></strong>.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlAgency" Visible="false">
			<p>Fill in your user account to access to your Agency information.</p>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlPremium" Visible="false">
			<p>Your premium subscription will end on <%=Model.AgencyPremiumDate%></p>
			<p>To renew or cancel the subscription, go to your <a href="https://www.paypal.com">Paypal Account</a></p>
		</asp:Panel>
		<div id="fullForm">
			<% Html.BeginForm("Register", "UsersController", FormMethod.Post, new {onsubmit="return true;"});%>
			<%= Html.Hidden("AgencyId") %>
			<div class="validationErrors">
				<%=Html.ValidationSummary(new Dictionary<string, object>
				{
					{"Email", new Dictionary<ValidationErrorType, string>
						{ 
							{ValidationErrorType.NullOrEmpty, "Email is required."}
							,{ValidationErrorType.Format, "Email is invalid."}
							,{ValidationErrorType.DuplicateNotAllowed, "Email address is already in use."}  
						}
					}
					,{"Password", "Password is required and must be at least 4 characters."}
					,{"Name", "Name is required and must be at least 4 characters."}
					,{"passwordConfirm", "Passwords do not match."}
					,{"Birthday", "Birthday is invalid."}
				    ,{"captcha", "Captcha verification failed."}
				}, null)%>
			</div>
			<div class="item">
				<label for="email">Email address</label>
				<%=Html.TextBox("Email") %>
			</div>
			<p class="note" id="pnlPasswordChange" visible="false" runat="server">Fill in the password fields only if you want to change it.</p>
			<div class="item">
				<label for="password">Choose password</label>
				<%=Html.Password("Password") %>
			</div>
			<div class="item">
				<label for="passwordConfirm">Re-type password</label>
				<%=Html.Password("PasswordConfirm") %>
			</div>
			<div class="item">
				<label for="userName">Public name</label>
				<%=Html.TextBox("Name") %>
			</div>
			<div class="item">
				<label for="country">Country</label>
				<%=Html.DropDownList("CountryCode", new SelectList((IEnumerable) ViewData["Countries"], "CountryCode", "CountryName", Model.CountryCode!=null?Model.CountryCode:"US")) %>
			</div>
			<h2><a id="showAdditionalAnchor" href="#" onclick="return showAdditional(this);">(+) Additional info</a></h2>
			<div id="additionalInfo" style="display:none;">
				<div class="item">
					<label for="gender">Gender</label>
					<%=Html.DropDownList("Gender")%>
				</div>
				<p class="note">Date format: MM/DD/YYYY</p>
				<div class="item">
					<label for="birthDay">Birthday</label>
					<%=Html.TextBox("Birthday", "{0:d}", typeof(DateTime), null) %>
				</div>
			</div>
		    <div>
		        <div class="g-recaptcha" data-sitekey="6Lc7F5gUAAAAAAG6HkeFhme6wp4SAuDJbWuMJy5P"></div>
		    </div>
			<asp:Panel runat="server" ID="pnlTerms" Visible="false">
				<br />
				<p>By clicking on the button below, you agree to the <%=Html.ActionLinkFormatted("terms of service", "Terms", "Home") %>.</p>
			</asp:Panel>
			<br />
			<input type="submit" value="Send &gt;&gt;" class="button" />
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>