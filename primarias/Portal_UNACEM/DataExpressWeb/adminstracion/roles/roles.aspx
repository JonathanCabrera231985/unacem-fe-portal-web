<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="Administracion.roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><strong><span class="style2">ROLES</span></strong></center><br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" DataKeyNames="idRol" DataSourceID="SqlDataSource1" 
        ForeColor="Black" GridLines="Vertical" Height="144px" style="font-size: x-small" 
        Width="913px" BackColor="White" BorderColor="#DEDFDE" 
        BorderWidth="1px" BorderStyle="None"  >
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="descripcion" HeaderText="Descripción" 
                SortExpression="descripcion" >
            </asp:BoundField>
            <asp:CheckBoxField DataField="crear_cliente" HeaderText="Crear Cliente" 
                SortExpression="crear_cliente" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="crear_admin_sucursal" 
                HeaderText="Crear Admin Sucursal" SortExpression="crear_admin_sucursal" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="consultar_facturas_propias" 
                HeaderText="Facturas Propias" 
                SortExpression="consultar_facturas_propias" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="consultar_todas_facturas" 
                HeaderText="Todas las Facturas" 
                SortExpression="consultar_todas_facturas" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="reportesSucursales" 
                HeaderText="Rep. Sucursales" 
                SortExpression="reportesSucursales" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="reportesGlobales" 
                HeaderText="Rep Globales" 
                SortExpression="reportesGlobales" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="modificarEmpleado" 
                HeaderText="Modificar Empleado" 
                SortExpression="modificarEmpleado" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="asignacion_roles" HeaderText="Roles" 
                SortExpression="asignacion_roles" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="envio_facturas_email" 
                HeaderText="Envio Facturas(mail)" SortExpression="envio_facturas_email" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="agregar_documento" HeaderText="Crear Factura" 
                SortExpression="agregar_documento" >
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
			<asp:CheckBoxField DataField="validarFactura" HeaderText="Validar Facturas" 
                SortExpression="validarFactura" />
            <asp:CheckBoxField DataField="aceptarFactura" HeaderText="Aceptar Facturas" 
                SortExpression="aceptarFactura" />
            <asp:CheckBoxField DataField="Recepcion" HeaderText="Ver Facturas Recibidas" 
                SortExpression="Recepcion" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                <a href='modificar_roles.aspx?id=<%# Eval("idRol") %>'>Editar</a>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Si eliminas el Rol, se eliminaran los Empleados asociado a el. ¿Deseas eliminar el rol?');" CausesValidation="False" 
                        CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" Width="80px" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#CCCC99" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" HorizontalAlign="Center" 
            ForeColor="White" Font-Size="X-Small" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" 
            HorizontalAlign="Right" />
        <RowStyle BackColor="#DEE2EB" />
        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT * FROM roles WHERE eliminado='False'" 
        DeleteCommand="UPDATE Roles SET eliminado='true' WHERE (idRol = @idRol) UPDATE Empleados SET eliminado='true' WHERE (id_Rol=@idRol)">
    </asp:SqlDataSource>
    <br />
    </asp:Content>
