<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Entry>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   <title>Unsubscription</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <h1>Unsubscription</h1>
   <p>You will not receive more notifications from the thread <em><%=Model.EntryTitle %></em>.</p>
   <br />
   <p><a href="/">Continue &gt;&gt;</a></p>
</asp:Content>