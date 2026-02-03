<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListNames.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Companies.ListNames" %>
var companyNamesList = 
	[""
<% 
	
	var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
	foreach (CompanyFeeds.DataAccess.Queries.CompaniesQueries.CompanyNamesRow company in Model)
	{
		Response.Write("," + serializer.Serialize(company.CompanyName) + "\n");
	}
%>
	];