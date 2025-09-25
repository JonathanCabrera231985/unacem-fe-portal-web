<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataFilter1.ascx.cs" Inherits="DataExpressWeb.DataFilter1" %>
<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlNewFilter" runat="server">
        </asp:Panel>
        <asp:Panel ID="pnlToolbar" runat="server" >
            <asp:Button ID="btnAddNewFilter" runat="server" OnClick="btnAddNewFilter_Click" Text="Añadir Filtro" CssClass="buttons" />
            <asp:Button ID="btnAndNewFilter" runat="server" CssClass="buttons" Text="AND" OnClick="btnAndNewFilter_Click" Visible="False" />
            <asp:Button ID="btnOrNewFilter" runat="server" CssClass="buttons" Text="OR" OnClick="btnOrNewFilter_Click" Visible="False" /></asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
