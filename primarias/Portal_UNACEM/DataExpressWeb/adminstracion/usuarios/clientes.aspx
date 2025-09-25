<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="clientes.aspx.cs" Inherits="Administracion.clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <strong><span class="style2">CLIENTES</span></strong></center>
            <br />
            <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
                vertical-align: top;">
                <tr>
                    <td class="style3">
                        Usuario:
                        <br />
                        <asp:TextBox ID="txt_identificacion" runat="server" CssClass="txt_gr1" Width="250px"></asp:TextBox>
                    </td>
                    <td class="style3">
                        Nombre:
                        <br />
                        <asp:TextBox ID="txt_nombre" runat="server" CssClass="txt_gr1" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3" colspan="2">
                        <asp:Button runat="server" ID="btoBuscar" Text="Buscar" OnClick="btoBuscar_Click" CssClass="botones" />&nbsp;
                        <asp:Button runat="server" ID="btoActualizar" Text="Actualizar" OnClick="btoActualizar_Click" CssClass="botones" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceEmpleados"
                CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="60%" DataKeyNames="idCliente"
                BackColor="White" BorderColor="#DEDFDE" BorderWidth="1px" BorderStyle="None"
                Height="5%" AllowPaging="True">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="NOMBRE" SortExpression="NOMBRE_COMPLETO" />
                    <asp:BoundField DataField="USERNAME" HeaderText="USUARIO" SortExpression="USERNAME" />
                    <asp:BoundField DataField="Estado" HeaderText="ACTIVO" SortExpression="Estado" ReadOnly="True" />
                    <asp:BoundField DataField="descripcion" HeaderText="ROL" SortExpression="descripcion" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <a href='modificar_usuario.aspx?idmbdi=<%# Eval("idCliente") %>'>Editar</a>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Deseas eliminar el Cliente?');"
                                CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No existen registros disponibles.
                </EmptyDataTemplate>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" Font-Size="Small" />
                <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#DEE2EB" />
                <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceEmpleados" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                SelectCommand="SELECT Clientes.idCliente, Clientes.nombreCliente AS NOMBRE_COMPLETO, Clientes.userCliente AS USERNAME, CASE WHEN Clientes.status = 0 THEN 'Inactivo' WHEN Clientes.status = 1 THEN 'Activo' END AS 'Estado', Roles.descripcion, RECEPTOR.RFCREC FROM Clientes INNER JOIN Roles ON Clientes.id_Rol = Roles.idRol INNER JOIN RECEPTOR ON Clientes.id_Receptor = RECEPTOR.IDEREC WHERE Clientes.eliminado = 'False'
            and (Clientes.nombreCliente like '%'+@nombre+'%' or @nombre = '-') and (Clientes.userCliente like '%'+@ruc+'%' or @ruc = '-')"
                DeleteCommand="UPDATE CLIENTES SET eliminado = 'True' where idCliente = @idCliente">
                <DeleteParameters>
                    <asp:Parameter Name="idCliente" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter DefaultValue="-" Name="nombre" Type="String" />
                    <asp:Parameter DefaultValue="-" Name="ruc" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
