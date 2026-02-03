<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Agencies.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%PageTitle = Model == null ? "Register agency" : "Edit agency information"; %>
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
		<div id="fullForm">
			<div id="divExternalSources"></div>
			<% Html.BeginForm("Edit", "Agencies", new{agencyTag=ViewData["AgencyTag"]}, FormMethod.Post, new {enctype="multipart/form-data",onsubmit="return true;",id="agencyForm"});%>
			<div class="validationErrors">
				<%=Html.ValidationSummary("<h3>Please check the following errors:</h3>", new Dictionary<string, object>
				{
					//You must indicate your full birthday 
					{"Name", "Agency name must not be blank."}
					,{"Url", "Website url must not be blank, and should be a website address."}
					,{"Email", "Email format is not valid."}
					,{"Description", "Description must not be blank."}
					,{"Logo", "Logo file is not valid. Upload a logo in jpg/gif/png format. Filesize must be smaller than 500kb."}
					,{"Tag", new Dictionary<ValidationErrorType, string>()
						{ 
							{ValidationErrorType.DuplicateNotAllowed, "Agency already exist."}
							,{ValidationErrorType.Format, ""},{ValidationErrorType.MinLength, ""},{ValidationErrorType.NullOrEmpty, ""}
						}
					}
				}, null)%>
			</div>
			<div class="item">
				<label for="Name">Agency name</label>
				<%=Html.TextBox("Name")%>
			</div>
			<div class="item">
				<label for="Url">Website url</label>
				<%=Html.TextBox("Url") %>
			</div>
			<div class="item">
				<label for="Email" class="optional">Agency public email</label>
				<%=Html.TextBox("Email")%>
			</div>
			<div class="item">
				<label for="Phone" class="optional">Phone</label>
				<%=Html.TextBox("Phone")%>
			</div>
			<%--
			<div class="item">
				<label for="Logo" class="optional">Logo</label>
				<input type="file" name="Logo" id="Logo" />
			</div>
			--%>
			<div class="content">
				<label for="Description">Description</label>
				<%=Html.TextArea("Description")%>
			</div>
			<div style="padding: 10px 100px 10px 0;">
				<p class="note">If checked, the agency will not be shown as submitter of the press release.</p>
				<div class="item check">
					<label for="AllowComments">Hide as author</label>
					<%=Html.CheckBox("HideAuthor")%>
				</div>
			</div>
			<input type="submit" value="Send &gt;&gt;" class="button" />
			
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>
