<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="crear_rol.aspx.cs" Inherits="Administracion.crear_rol" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><strong style="font-size: large">CREAR ROL</strong></center><br />
    <table cellpadding="0" cellspacing="0" width="85%" 
           style="color:Black;background-color:White;border-color:#DEDFDE;border-width:1px;border-style:None;border-collapse:collapse;font-size: x-small">
		<tr align="center" style="color:White;height:30px;background-color:#5F6062;font-weight:bold;">
        <td align="center">
                Descripción</td>
        <td align="center">
                Crear Cliente</td>
        <td align="center">
                Crear Admin Sucursal</td>
        <td align="center">
                Facturas Propias</td>
        <td align="center">
                Todas las Facturas</td>
        <td align="center">
                Rep. Sucursales</td>
        <td align="center">
                Rep Globales</td>
        <td align="center">
                Modificar Empleado</td>
        <td align="center">
                Roles</td>
        <td align="center">
                Envio Facturas(mail)</td>
        <td align="center">
                Crear Factura</td>
		<td align="center">
               Validar Facturas</td>
        <td align="center">
               Aceptar Facturas</td>
		<td align="center">
               Ver Facturas Recibidas</td>
    </tr>
    <tr>
        <td align="center">
            <asp:TextBox ID="tbRol" runat="server" Width="143px" CssClass="txt_gr2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbRol" ErrorMessage="Requiere nombre del rol" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbCrear_cliente" runat="server" EnableViewState="true" 
                Checked='<%# Convert.ToBoolean(Eval("crear_admin_sucursal")) %>' />
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbCrear_admin" runat="server" />
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbConsulta_propias" runat="server" style="font-size: x-small" />
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbConsulta_todas" runat="server"  />
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbReportesSucursales" runat="server" />
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbReportesGlobales" runat="server" style="font-size: x-small" />
            
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbModificarEmpleado" runat="server" style="font-size: x-small" />
        </td>
        <td align="center" >
            
            <asp:CheckBox ID="cbAsignar_rol" runat="server" />
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbEnvio_fac" runat="server" style="font-size: x-small" />
           
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbAgregar_doc" runat="server" style="font-size: x-small" />
        </td>
		<td align="center" >
            <asp:CheckBox ID="cbValidarFacturas" runat="server" />
        </td>
		<td align="center" >
            <asp:CheckBox ID="cbAceptarFacturas" runat="server" style="font-size: x-small" />
           
        </td>
        <td align="center" >
            <asp:CheckBox ID="cbVerFacturasRecibidas" runat="server" style="font-size: x-small" />
        </td>

    </tr>
    <tr style="background-color:EAF0EF;">
        <td colspan="14">
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbRol" 
                ErrorMessage="Tienes que escribir la Descripción del Rol" ForeColor="Red"></asp:RequiredFieldValidator>
            &nbsp;<asp:Label ID="lMsj" runat="server" ForeColor="#F93200"></asp:Label>
            <br />
            <asp:Button ID="BCrear" runat="server" Text="Crear" style="text-align: center; height: 26px;" 
                Width="68px" onclick="BCrear_Click1" CssClass="botones" />
&nbsp;&nbsp;
            <br />
        </td>
        </tr>
</table>
</asp:Content>
