<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificarSucursal.aspx.cs" Inherits="Administracion.modificarSucursal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            color: #000000;
        }
        .style2
        {
            height: 21px;
        }
        .style3
        {
            height: 21px;
            text-align: center;
        }
        .style4
        {
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td  colspan="3" >
               <center class="style4"><strong>MODIFICAR SUCURSAL</strong></center> 
            </td>
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
                Width="387px" CssClass="txt_gr2" MaxLength="50"></asp:TextBox> <br />
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
            <td class="style1">
                &nbsp;</td>
            <td>
                <asp:Button ID="bModificar" runat="server" Text="Actualizar" 
                    onclick="bModificar_Click" />
                &nbsp;
                <asp:Button ID="bCancelar" runat="server" onclick="bCancelar_Click" 
                    Text="Cancelar" />
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
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
