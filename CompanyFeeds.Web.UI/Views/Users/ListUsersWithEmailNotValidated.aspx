<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="CompanyFeeds.Web.UI.BaseViewPage<List<User>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Users</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Users with email address not validated</h1>

		<ul>
<%
		foreach (User user in Model)
		{
%>	
			<li><a href="<%=Html.Url("ValidateEmailByAdmin", "Users", new{id=user.Id,guid=user.Guid.ToString()}) %>">Validate manually</a>&nbsp;&nbsp;<%=user.Name%> (<%=user.Email%>) <%=user.AgencyId != null ? " - Agency " + user.AgencyId : ""%></li>
<%				
		}	
%>
		</ul>
</asp:Content>
