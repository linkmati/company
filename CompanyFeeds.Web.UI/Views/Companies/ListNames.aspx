<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListNames.aspx.cs" Inherits="CompanyFeeds.Web.UI.Views.Companies.ListNames" %>
var companyNamesList = 
	[""
<% 
	
	var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
	foreach (CompanyFeeds.DataAccess.Queries.CompaniesQueries.CompanyNamesRow company in Model)
	{
		//Response.Write(",\"" + System.Security.SecurityElement.Escape(company.CompanyName) + "\"\n");
		Response.Write("," + serializer.Serialize(company.CompanyName) + "\n");
	}
%>
	];