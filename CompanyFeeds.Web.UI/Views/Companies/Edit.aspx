<%@ Page Title="Submit company" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="false" CodeBehind="Edit.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Companies.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<% PageTitle = Model == null ? "Submit company" : "Edit company"; %>
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
				<%=Html.ValidationSummary("<h3>Please check the following errors:</h3>", new Dictionary<string, object>
				{
					{"Name", "Company name must not be blank."}
					,{"Url", "Website url must not be blank."}
					,{"Description", "Description must not be blank."}
					,{"FeedUrl", "Rss feed url format is not valid."}
					,{"CategoryId", "Select a sector/industry."}
					,{"Logo", "Logo file is not valid. Upload a logo in jpg/gif/png format. Filesize must be smaller than 500kb."}
					,{"Tag", new Dictionary<ValidationErrorType, string>()
						{ 
							{ValidationErrorType.DuplicateNotAllowed, "Company already exist."}
							,{ValidationErrorType.Format, ""},{ValidationErrorType.MinLength, ""},{ValidationErrorType.NullOrEmpty, ""}
						}
					}
				}, null)%>
			</div>
			<div class="item">
				<label for="Name">Company name</label>
				<%=Html.TextBox("Name")%>
			</div>
			<div class="item">
				<label for="Url">Website url</label>
				<%=Html.TextBox("Url")%>
			</div>
			<div class="item">
				<label for="CategoryId">Sector/Industry</label>
				<%=Html.DropDownListDefault("CategoryId", new SelectList(this.Cache.Categories, "CategoryId", "CategoryName", Model != null ? Model.CategoryId : 0), "", "")%>
			</div>
<% 
			if (User.Role == UserRole.Admin)
			{
%>
			<div class="item">
				<label for="FeedUrl" class="optional">Rss feed url</label>
				<%=Html.TextBox("FeedUrl")%>
			</div>
<% 
			}
%>			
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
			<br />
			<input type="submit" value="Send &gt;&gt;" class="button" />
			
			<% Html.EndForm();%>
		</div>
	</div>
</asp:Content>
