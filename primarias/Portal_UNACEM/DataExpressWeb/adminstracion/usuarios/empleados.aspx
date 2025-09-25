<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="empleados.aspx.cs" Inherits="Administracion.empleados" %>
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

    <center><strong><span class="style2">EMPLEADOS</span></strong></center><br />
    <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
        vertical-align: top;">
            <tr>
                <td class="style3">
                        Nombre: 
                        <br />
                        <asp:TextBox ID="txt_nombre" runat="server" CssClass="txt_gr1"  Width="250px"></asp:TextBox>
                </td>
                <td class="style3">
                     Usuario: 
                        <br />
                        <asp:TextBox ID="txt_usuario" runat="server" CssClass="txt_gr1"  Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3" colspan="2">
                    <asp:Button runat="server" ID="btoBuscar" Text="Buscar" 
                        onclick="btoBuscar_Click" CssClass="botones" />&nbsp;
                    <asp:Button runat="server" ID="btoActualizar" Text="Actualizar" 
                        onclick="btoActualizar_Click" CssClass="botones"  />
                </td>
            </tr>
    </table>
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSourceEmpleados" CellPadding="4" ForeColor="Black" 
        GridLines="Vertical" Width="898px" 
        DataKeyNames="idEmpleado" BackColor="White" 
        BorderColor="#DEDFDE" BorderWidth="1px" BorderStyle="None" 
        AllowPaging="True" AllowSorting="True">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="NOMBRE_COMPLETO" HeaderText="NOMBRE" 
                SortExpression="NOMBRE_COMPLETO" />
            <asp:BoundField DataField="USERNAME" HeaderText="USUARIO" 
                SortExpression="USERNAME" />
            <asp:BoundField DataField="Estado" HeaderText="ACTIVO" 
                SortExpression="Estado" ReadOnly="True" />
            <asp:BoundField DataField="descripcion" HeaderText="ROL" 
                SortExpression="descripcion" />
            <asp:BoundField DataField="Sucursal" HeaderText="SUCURSAL" 
                SortExpression="Sucursal" />
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <a href='modificar_usuario.aspx?idmrdxbdi=<%# Eval("idEmpleado") %>'>Editar</a>
                <asp:LinkButton ID="LinkButton1" runat="server"  OnClientClick="return confirm('¿Deseas eliminar el Empleados?');" CausesValidation="False" 
                    CommandName="Delete" Text="Eliminar"></asp:LinkButton>
            
            </ItemTemplate> 
            <HeaderStyle Width="100px" />
        </asp:TemplateField>
        </Columns>
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
    <asp:SqlDataSource ID="SqlDataSourceEmpleados" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT Empleados.idEmpleado, Empleados.nombreEmpleado AS NOMBRE_COMPLETO, Empleados.userEmpleado AS USERNAME, CASE WHEN Empleados.status = 0 THEN 'Inactivo' WHEN Empleados.status = 1 THEN 'Activo' END AS 'Estado', Roles.descripcion, Sucursales.idSucursal, Sucursales.clave AS Sucursal FROM Empleados INNER JOIN Roles ON Empleados.id_Rol = Roles.idRol INNER JOIN Sucursales ON Empleados.id_Sucursal = Sucursales.idSucursal WHERE Empleados.eliminado='False'
            and (Empleados.nombreEmpleado like '%'+@nombre+'%' or @nombre = '-') and (Empleados.userEmpleado like '%'+@userEmpleado+'%' or @userEmpleado = '-')" 
        DeleteCommand="UPDATE empleados set eliminado='True' WHERE idEmpleado=@idEmpleado">
    <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="nombre" Type="String" />
            <asp:Parameter DefaultValue="-" Name="userEmpleado" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="idEmpleado" />
    </DeleteParameters>
    </asp:SqlDataSource>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
