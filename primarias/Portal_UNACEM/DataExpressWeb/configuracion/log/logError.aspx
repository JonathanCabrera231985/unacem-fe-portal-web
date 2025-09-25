<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="logError.aspx.cs" Inherits="DataExpressWeb.configuracion.log.logError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Text1
        {
            width: 189px;
            margin-top: 0px;
        }
        .style1
        {
        }
        #Text2
        {
            width: 474px;
        }
        #Text3
        {
            width: 164px;
        }
        #Text5
        {
            width: 139px;
        }
        #Text6
        {
            width: 138px;
        }
        .style7
    {
        width: 100%;
    }
    .style8
    {
        width: 217px;
    }
    </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="style7">
    <tr>
        <td class="style8">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style8">
            Nº. Control:<br />
            <asp:TextBox ID="tbNoOrden" runat="server" Width="163px" CssClass="txt_gr2"></asp:TextBox>
        </td>
        <td class="style8">
            Nº. Documento:<br />
            <asp:TextBox ID="Txt_Cod_documento" runat="server" Width="163px" 
                CssClass="txt_gr2"></asp:TextBox>
        </td>
        <td>
            Nombre del Archivo:<br />
            <asp:TextBox ID="tbArchivo" runat="server" Width="269px" CssClass="txt_gr2"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style8">
            <br />
            <asp:Button ID="bBuscarReg" runat="server" onclick="bBuscarReg_Click" 
                Text="Buscar" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="bActualizar" runat="server" onclick="bActualizar_Click" 
                Text="Actualizar" />
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
<br />
    <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" CellPadding="4" 
    DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical" 
     Width="700px" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
        BorderWidth="1px" AllowPaging="True" AllowSorting="True" 
    DataKeyNames="idErrorFactura" >
    <AlternatingRowStyle BackColor="White" />
    <Columns >
        <asp:BoundField DataField="detalle" HeaderText="DETALLE" 
            SortExpression="detalle" />
        <asp:BoundField DataField="fecha" HeaderText="FECHA" SortExpression="fecha" />
        <asp:BoundField DataField="archivo" HeaderText="ARCHIVO" 
            SortExpression="archivo" />
        <asp:BoundField DataField="linea" HeaderText="LINEA" SortExpression="linea" />
         <asp:BoundField DataField="Comprobante" HeaderText="COMPROBANTE"  SortExpression="Comprobante" />
        <asp:BoundField DataField="nDocumento" HeaderText="NO DOCUMENTO" 
            SortExpression="nDocumento" />
        <asp:BoundField DataField="numeroDocumento" HeaderText="NO CONTROL" 
            SortExpression="numeroDocumento" />
        <asp:BoundField DataField="tipo" HeaderText="TIPO" SortExpression="tipo" />
        
    </Columns>
        <EmptyDataTemplate>
            No se encuentra el registro, dentro del registo.
        </EmptyDataTemplate>
    <FooterStyle BackColor="#CCCC99" />
    <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" 
            Font-Size="Small" />
    <PagerStyle BackColor="#5F6062" ForeColor="Black" 
            HorizontalAlign="Center" />
        <RowStyle BackColor="#DEE2EB" />
    <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
    <SortedAscendingCellStyle BackColor="#FBFBF2" />
    <SortedAscendingHeaderStyle BackColor="#848384" />
    <SortedDescendingCellStyle BackColor="#EAEAD3" />
    <SortedDescendingHeaderStyle BackColor="#575357" />
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        
        SelectCommand="PA_Log_basico" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
<br />
</asp:Content>