<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Users.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Login</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Login</h1>
    <uc:HtmlContainer runat="server" id="pnlForm">
		<p>
			Please enter your email and password below. If you don't have an account,
			please <%= Html.ActionLinkFormatted("register", "Edit") %>.
		</p>
		<div id="fullForm">
			<div class="validationErrors">
				<%=Html.ValidationSummary(new Dictionary<string, object>
					{
						{"Email", new Dictionary<ValidationErrorType, string>
							{ 
								{ValidationErrorType.CompareNotMatch, "Your email or password was incorrect."}
								,{ValidationErrorType.Format, "Email format is invalid."}
							}
						}
					}, null)%>
			</div>
			<% Html.BeginForm();%>
			<div class="item">
				<label for="Email">Email</label>
				<%=Html.TextBox("Email")%>
			</div>
			<div class="item">
				<label for="Password">Password</label>
				<%=Html.Password("Password")%>
			</div>
			<br />
			<div><input type="submit" class="button" value="Login &gt;&gt;" /></div>
			<% Html.EndForm();%>
		</div>
	</uc:HtmlContainer>
	<uc:HtmlContainer runat="server" id="pnlValidateEmail" visible="false">
		<p><em>In order to do this action you must verify your email address</em>.</p>
		
		<p>We've sent an email to <%=User.Email %> containing a URL you'll need to follow to verify your account. </p>
	</uc:HtmlContainer>
</asp:Content>
