<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modReglas.aspx.cs" Inherits="DataExpressWeb.modReglas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            width: 124px;
            height: 26px;
            text-align: right;
            font-size: small;
            font-weight: bold;
        }
        .style3
        {
            height: 26px;
        }
        .style5
        {
            text-align: left;
        }
    .style7
    {
            width: 124px;
            height: 5px;
            text-align: right;
        }
    .style8
    {
        height: 5px;
    }
        .style9
        {
            width: 124px;
            height: 5px;
            text-align: right;
            font-size: small;
            font-weight: bold;
        }
        .style10
        {
            font-size: x-small;
        }
        .style11
        {
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <div class="style5">
            <strong><center><span class="style11">MODIFICAR REGLA DE EMAIL<br />
                </span>
                <br />
            </center></strong></div>
        <table align="center" style="width: 69%;">
            <tr>
                <td class="style2">
                    Nombre:</td>
                <td class="style3">
                    <asp:TextBox ID="tbNombre" runat="server" Width="90%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbNombre" 
                        ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    RUC:
                </td>
                <td class="style3">
                    <asp:TextBox ID="tbRFC" runat="server" Width="150px" MaxLength="13" 
                        ReadOnly="True" CssClass="txt_gr2"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="tbRFC" ErrorMessage="RUC inválido" ForeColor="Red" 
                    
                        ValidationExpression="[0-9]{10}001"></asp:RegularExpressionValidator>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                    ControlToValidate="tbRFC" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style9">
                    Estado:</td>
                <td class="style8">
                    <asp:DropDownList ID="ddlEstado" runat="server" Width="126px" AppendDataBoundItems="True">
                        <asp:ListItem Value="1">Activa</asp:ListItem>
                        <asp:ListItem Value="0">Inactiva</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style9" valign="middle">
                    Emails:</td>
                <td class="style8">
                    <asp:TextBox ID="tbEmail" runat="server"    Height="46px" Width="500px" 
                        ></asp:TextBox>
                    <br />
                    <span class="style10"><em>email@dominio.com,email@domain.net</em></span><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbEmail"
                        ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ErrorMessage="El formato de Email no es válido" ForeColor="Red" ControlToValidate="tbEmail" 
                        ValidationExpression="^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="style7" valign="middle">
                    &nbsp;</td>
                <td class="style8">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2"><center>
                    <asp:Button ID="bActualizar" runat="server" style="text-align: center" 
                        Text="Actualizar Regla" onclick="bActualizar_Click" />
                        </center>
                </td>
            </tr>
        </table>
        <asp:Label ID="lMensaje" runat="server" ForeColor="#CC3300"></asp:Label>
        <br />
        
    </p>
</asp:Content>
