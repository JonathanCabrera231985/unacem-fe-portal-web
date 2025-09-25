<%@ Page Title="Mostrar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Mostrar.aspx.cs" Inherits="ups.Mostrar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 243px;
        }
        .style3
        {
            width: 168px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <body>
        <fieldset>
            <legend>Configuración General </legend>
            <table style="width: 84%; height: 184px;">
                <tr>
                    <td class="style3">
                        Directorio de Documentos:
                    </td>
                    <td>
                        <asp:TextBox ID="tbDirdocs" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="tbDirdocs"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Directorio de Txt:
                    </td>
                    <td>
                        <asp:TextBox ID="tbDirtxt" runat="server" Width="496px" ReadOnly="True" OnTextChanged="tbDirtxt_TextChanged"
                            CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="tbDirtxt"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Directorio de Respaldo:
                    </td>
                    <td>
                        <asp:TextBox ID="tbDirrespaldo" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="tbDirrespaldo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Directorio XML base:
                    </td>
                    <td>
                        <asp:TextBox ID="txtXmlBase" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="tbDirrespaldo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Directorio Firma Electrónica:
                    </td>
                    <td>
                        <asp:TextBox ID="tbDircerti" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="tbDircerti"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Password P12:
                    </td>
                    <td>
                        <asp:TextBox ID="tbDirllaves" runat="server" Width="497px" ReadOnly="True" TextMode="Password"
                            CssClass="txt_gr1"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirllaves"></asp:RequiredFieldValidator>--%>
                        <asp:PasswordStrength ID="PasswordStrength1" runat="server" TargetControlID="tbDirllaves">
                        </asp:PasswordStrength>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Periodo de Intentos:
                    </td>
                    <td>
                        <asp:TextBox ID="txtIntentos" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="txtIntentos"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Directorio de Recepción:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDirRecep" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                    ErrorMessage="*" ForeColor="Red" ControlToValidate="txtIntentos"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Mail / User Recepción:
                    </td>
                    <td>
                        <asp:TextBox ID="txtmailRecep" runat="server" Width="496px" ReadOnly="True" CssClass="txt_gr1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                            ForeColor="Red" ControlToValidate="txtmailRecep"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        Password Recepción:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPwdRecepcion" runat="server" Width="497px" ReadOnly="True" TextMode="Password"
                            CssClass="txt_gr1"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ErrorMessage="*" ForeColor="Red" ControlToValidate="tbDirllaves"></asp:RequiredFieldValidator>--%>
                        <asp:PasswordStrength ID="PasswordStrength2" runat="server" TargetControlID="txtPwdRecepcion">
                        </asp:PasswordStrength>
                    </td>
                </tr>
            </table>
            <center>
                <asp:Button ID="bModificar" runat="server" Style="text-align: center" Text="Modificar"
                    OnClick="bModificar_Click" CssClass="botones" />
                <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" Text="Actualizar"
                    Visible="False" CssClass="botones" />
            </center>
        </fieldset>
    </body>
</asp:Content>