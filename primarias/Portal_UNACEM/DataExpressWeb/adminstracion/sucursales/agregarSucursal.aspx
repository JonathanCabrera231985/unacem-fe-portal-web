<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="agregarSucursal.aspx.cs" Inherits="Administracion.agregarSucursal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .style1
    {
        width: 111px;
    }
    .style2
    {
        width: 423px;
    }
        .style3
        {
            width: 111px;
        }
        .style5
        {
            color: #000000;
        }
        .style7
        {
            text-align: center;
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
    <tr>
        <td class="style7" colspan="3">
            <strong>AGREGAR SUCURSAL</strong></td>
    </tr>
    <tr>
        <td class="style3">
            Clave:</td>
        <td class="style2">
            <asp:TextBox ID="tbClave" runat="server" CssClass="txt_gr2" MaxLength="3"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbClave" ErrorMessage="Requiere clave" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            Sucursal:</td>
        <td class="style2">
            <asp:TextBox ID="tbSucursal" runat="server" CssClass="txt_gr2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbSucursal" ErrorMessage="Requiere sucursal" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            Dirección:</td>
        <td class="style2">
            <asp:TextBox ID="tbDireccion" runat="server" Height="49px" TextMode="MultiLine" 
                Width="386px" CssClass="txt_gr2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="tbDireccion" ErrorMessage="Requiere dirección" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            Correo Electrónico:</td>
        <td class="style2">
            <asp:TextBox ID="txtCorreos" runat="server" Height="40px" TextMode="MultiLine" 
                Width="387px" CssClass="txt_gr2"></asp:TextBox> <br />
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                        ControlToValidate="txtCorreos" ErrorMessage="El formato de Email no es válido" ForeColor="Red"
                                        ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                                        ValidationGroup="email">
                  </asp:RegularExpressionValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style2">
            <asp:Button ID="bGuardar" runat="server" Text="Guardar" 
                onclick="bGuardar_Click" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style2">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
            <asp:Label ID="lMsj" runat="server" ForeColor="Red"></asp:Label>
            <br />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style2">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
