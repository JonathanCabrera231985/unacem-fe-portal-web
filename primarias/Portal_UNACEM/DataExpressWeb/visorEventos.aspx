<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="visorEventos.aspx.cs" Inherits="DataExpressWeb.visorEventos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
        CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="Black" 
        GridLines="Vertical" Font-Size="Small">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" />
            <asp:BoundField DataField="detalle" HeaderText="Detalle" 
                SortExpression="detalle" />
            <asp:BoundField DataField="archivo" HeaderText="Archivo" 
                SortExpression="archivo" />
            <asp:BoundField DataField="tipo" HeaderText="Tipo" SortExpression="tipo" />
            <asp:BoundField DataField="numeroDocumento" HeaderText="No. Documento" 
                SortExpression="numeroDocumento" />
            <asp:BoundField DataField="linea" HeaderText="Linea" SortExpression="linea" />
        </Columns>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="False" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="#F7F7DE" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT TOP (4) detalle, fecha, archivo, tipo, numeroDocumento, linea FROM LogErrorFacturas WHERE (CONVERT (char(10), fecha, 103) = CONVERT (char(10), GETDATE(), 103)) ORDER BY fecha DESC">
    </asp:SqlDataSource>
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Documentos.aspx">Regresar</asp:HyperLink>
    <br />
</asp:Content>
