<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="HistorialAutorizacion.aspx.cs" Inherits="DataExpressWeb.recepcion.HistorialAutorizacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Text1
        {
            width: 189px;
            margin-top: 0px;
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
        .style1
        {
        }
        .style9
        {
            width: 109px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" Text="Historial de Aprobaciones" CssClass="titulo_pagina"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style9">
                No. Documento:
                <br />
            </td>
            <td>
                <asp:TextBox ID="tbNumDoc" runat="server" Height="22px" Width="231px" 
                    CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                Usuario:
            </td>
            <td>
                <asp:TextBox ID="tbUsuario" runat="server" Height="22px" Width="231px" 
                    CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                Clave de Acceso:
            </td>
            <td>
                <asp:TextBox ID="tbCA" runat="server" Height="22px" Width="231px" 
                    CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                Ruc Proveedor:
            </td>
            <td>
                <asp:TextBox ID="tbRucProv" runat="server" Height="22px" Width="231px" 
                    CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                Proveedor:
            </td>
            <td>
                <asp:TextBox ID="tbProveedor" runat="server" Width="231px" Height="22px" 
                    CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                <asp:Button ID="bBuscarReg" runat="server" OnClick="bBuscarReg_Click" Text="Buscar"
                    Width="65px" CssClass="botones" />
            </td>
            <td>
                <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" Text="Actualizar"
                    Width="65px" CssClass="botones" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Width="1000px"
        AllowPaging="True" DataKeyNames="claveAcesso">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" />
            <asp:BoundField DataField="provDesc" HeaderText="Proveedor" SortExpression="provDesc" />
            <asp:BoundField DataField="claveAcesso" HeaderText="Clave de Acceso" SortExpression="claveAcesso" />
            <asp:BoundField DataField="codDocDesc" HeaderText="Documento" SortExpression="codDocDesc" />
            <asp:BoundField DataField="docDesc" HeaderText="No." SortExpression="docDesc" />
            <asp:BoundField DataField="usuarioDesc" HeaderText="Usuario" SortExpression="usuarioDesc" />
            <asp:ImageField DataImageUrlField="estado" DataImageUrlFormatString="~/Imagenes/estado{0}.jpg"
                HeaderText="Estado">
                <ControlStyle Height="20px" Width="20px" />
                <ControlStyle Height="20px" Width="20px"></ControlStyle>
                <HeaderStyle Width="5%" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:ImageField>
            <asp:BoundField DataField="estadoDesc" SortExpression="estadoDesc" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <EmptyDataTemplate>
            No existen datos.
        </EmptyDataTemplate>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign= "Center" />
        <RowStyle BackColor="#DEE2EB" />
        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SP_Recepcion" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="<INSTRUCCION><FILTRO><opcion>3</opcion><query>-</query></FILTRO></INSTRUCCION>"
                Name="documentoXML" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
</asp:Content>
