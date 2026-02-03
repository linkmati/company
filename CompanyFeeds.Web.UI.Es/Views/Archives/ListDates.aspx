<%@ Page Title="Archives" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ListDates.aspx.cs" Inherits="CompanyFeeds.Web.UI.Es.Views.Archives.ListDates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<title>Archivo</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="archives">
		<ul class="path">
			<li><a href="/">Notas de prensa</a></li>
		</ul>
		<h1>Archivo</h1>
		<%
			DateTime date = Cache.SiteConfiguration.LaunchDate.Date;
			int currentMonth = 0;
			int currentYear = 0;
			DateTime today = DateTime.Now.Date;
			DateTime firstDay = date;	
			while (date < today)
			{
				if (date.Year != currentYear)
				{
					Write("<h2>" + date.Year + "</h2>");
					Write("<div class=\"year floatContainer\">");
					currentYear = date.Year;
				}
				if (date.Month != currentMonth)
				{
					Write("<div class=\"month\">");
					Write("<h3>" + date.ToString("MMMM") + "</h3>");
					currentMonth = date.Month;
					Write("<ul>");
					//print day of the week
					for (int i = 0; i < 7; i++)
					{	
						Write("<li class=\"dow\">" + ((DayOfWeek) i).ToString().Substring(0, 2) + "</li>");		
					}
					for (int j = 0; j < 7; j++)
					{
						if ((DayOfWeek) j == date.DayOfWeek)
						{
							break;
						}
						Write("<li class=\"empty\"></li>");
					}
				}
				
				//print date
				Write("<li>" + Html.ActionLinkFormatted(date.Day.ToString(), "Detail", "Archives", new{month=date.Month,year=date.Year, day=date.Day}, null) + "</li>");

				if (date.AddDays(1).Month != date.Month || date.AddDays(1) >= today)
				{
					//last day: close month
					Write("</ul>");
					Write("</div>");
					if (date.AddDays(1).Year != date.Year || date.AddDays(1) >= today)
					{
						//close year
						Write("</div>");
					}
				}
				
				date = date.AddDays(1);
			}
		%>
	</div>
</asp:Content>
