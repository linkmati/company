<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Entry>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   <title>Notificaciones</title>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <h1>Notificaciones</h1>
   <p>No recibirás mas notificaciones del tema <em><%=Model.EntryTitle %></em>.</p>
   <br />
   <p><a href="/">Continue &gt;&gt;</a></p>
</asp:Content>