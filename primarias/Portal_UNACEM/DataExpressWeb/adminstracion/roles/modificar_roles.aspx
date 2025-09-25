<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificar_roles.aspx.cs" Inherits="Administracion.modificar_roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">



        </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table  cellpadding="0" cellspacing="0" width="85%" style="color:Black;background-color:White;
        border-color:#DEDFDE;border-width:1px;border-style:None;height:144px;border-collapse:collapse;font-size: x-small">
    <tr style="background-color:#5F6062; height:30px;" class="style1">
        <td align="center">
                <b>Descripción</b></td>
        <td align="center">
                <b>Crear Cliente</b></td>
        <td align="center">
                <b>Crear Admin Sucursal</b></td>
        <td align="center">
                <b>Facturas Propias</b></td>
        <td align="center">
                <b>Todas las Facturas</b></td>
        <td align="center">
                <b>Rep. Sucursales</b></td>
        <td align="center">
                <b>Rep Globales</b></td>
        <td align="center">
                <b>Modificar Empleado</b></td>
        <td align="center">
                <b>Roles</b></td>
        <td align="center">
                <b>Envio Facturas(mail)</b></td>
        <td align="center">
                <b>Crear Factura</b></td>
		<td align="center">
               Validar Facturas</td>
        <td align="center">
               Aceptar Facturas</td>
        <td align="center">
               Ver Facturas Recibidas</td>
    </tr>
    <tr>
        <td >
            <asp:TextBox ID="tbRol" runat="server" Width="95px" Height="24px" 
                CssClass="txt_gr2"></asp:TextBox>
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
    <tr>
        <td colspan="14">
&nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbRol" 
                ErrorMessage="Tienes que escribir la Descripción del Rol" ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lMsj" runat="server" ForeColor="#F93200"></asp:Label>
            <br />
            <asp:Button ID="bModificar" runat="server" onclick="bModificar_Click" 
                Text="Actualizar" CssClass="botones" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="bCancelar" runat="server" onclick="bCancelar_Click" 
                Text="Cancelar" CssClass="botones" />
        </td>
    </tr>
</table>
</asp:Content>
