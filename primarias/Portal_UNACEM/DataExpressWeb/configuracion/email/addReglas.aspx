<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="addReglas.aspx.cs" Inherits="DataExpressWeb.addReglas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 113px;
            height: 26px;
            text-align: right;
            font-size: small;
            font-weight: bold;
        }
        .style3
        {
            height: 26px;
            width: 503px;
        }
        .style5
        {
            text-align: left;
        }
        .style7
    {
        width: 113px;
        height: 5px;
    }
    .style8
    {
        height: 5px;
            width: 503px;
        }
    .style9
    {
        width: 113px;
        height: 47px;
            text-align: right;
            font-size: small;
            font-weight: bold;
        }
    .style10
    {
        height: 47px;
            width: 503px;
        }
        .style11
        {
            width: 113px;
            height: 5px;
            text-align: right;
            font-size: small;
            font-weight: bold;
        }
        .style12
        {
            height: 47px;
            width: 503px;
            font-size: x-small;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <div class="style5">
            <strong><center>AGREGAR REGLA DE EMAIL<br />
                <br />
            </center></strong></div>
        <table align="center" style="width: 64%;">
            <tr>
                <td class="style2">
                    Nombre:</td>
                <td class="style3">
                    <asp:TextBox ID="tbNombre" runat="server" Width="90%" CssClass="txt_gr2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbNombre" 
                        ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style9">
                    RUC:
                </td>
                <td class="style10">
                    <asp:TextBox ID="tbRFC" runat="server" Width="150px" CssClass="txt_gr2"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="tbRFC" ErrorMessage="RUC inválido" ForeColor="Red" 
                    
                        
                        ValidationExpression="[0-9]{10}001"></asp:RegularExpressionValidator>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                    ControlToValidate="tbRFC" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style11">
                    Estado:</td>
                <td class="style8">
                    <asp:DropDownList ID="ddlEstado" runat="server" Width="126px">
                        <asp:ListItem Value="1">Activa</asp:ListItem>
                        <asp:ListItem Value="0">Inactiva</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style11">
                    Emails:</td>
                <td class="style8">
                    <asp:TextBox ID="tbEmail" runat="server" Height="46px" Width="500px" 
                        TextMode="MultiLine" CssClass="txt_gr2"></asp:TextBox>
                    <br />
                    <span class="style12"><em>email@dominio.com,email@domain.net</em></span><br />

 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEmail"
                        ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ErrorMessage="El formato de Email no es válido" ForeColor="Red" 
                        ControlToValidate="tbEmail" 
                        
                        ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="style7">
                    &nbsp;</td>
                <td class="style8">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
        <asp:Label ID="lMensaje" runat="server" ForeColor="#CC3300"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2"><center>
                    <asp:Button ID="bGuardar" runat="server" style="text-align: center" 
                        Text="Guardar Regla" onclick="bGuardar_Click" />
                        </center>
                </td>
            </tr>
        </table>
        <br />
        
    </p>
</asp:Content>
