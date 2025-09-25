<%@ Page Title="Mostrar" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="servermail.aspx.cs" Inherits="DataExpressWeb.configuracion.email.servermail" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        <style type="text/css">
            .style1
            {
                width: 243px;
            }
            .style2
            {
                width: 243px;
                height: 26px;
            }
            .style3
            {
                font-size: 1em;
            }
            .style4
            {
                width: 97px;
            }
            .style5
            {
                width: 97px;
                height: 26px;
            }
        </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<body>
 <fieldset>
 <legend>  <strong><span class="style3">Servidor de Correos</span></strong> 
 </legend>
<table style="width: 80%; height: 157px;">
            <tr>
                <td class="style4">
                    Servidor SMTP:</td>
                <td>
                    <asp:TextBox ID="tbServidor" runat="server" Width="349px" ReadOnly="True" 
                        CssClass="txt_gr2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="tbServidor" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Puerto:
                </td>
                <td>
                    <asp:TextBox ID="tbPuerto" runat="server" Width="349px" ReadOnly="True" 
                        CssClass="txt_gr2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="tbPuerto" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Usuario:
                </td>
                <td>
                    <asp:TextBox ID="tbUsuario" runat="server" Width="347px" ReadOnly="True" 
                        CssClass="txt_gr2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="tbUsuario" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Contraseña</td>
                <td>
                    <asp:TextBox ID="tbPassword" runat="server" Width="346px" ReadOnly="True" 
                        TextMode="Password" CssClass="txt_gr2"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ControlToValidate="tbPassword" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        <asp:PasswordStrength
                        ID="PasswordStrength2" runat="server" TargetControlID="tbPassword">
                    </asp:PasswordStrength>
                </td>
            </tr>

        <tr>
            <td class="style4">
                SSL:
            </td>
            <td class="style2">
                <asp:CheckBox ID="cbSSL" runat="server" Enabled="False" />
            </td>
        </tr>
        <tr>
            <td class="style5">
                E-mail de envio:
            </td>
            <td>
                <asp:TextBox ID="tbEmailEnvio" runat="server" Width="348px" ReadOnly="True" 
                    CssClass="txt_gr2"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="tbEmailEnvio" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="tbEmailEnvio" ErrorMessage="Email Invalido" ForeColor="Red" 
                    ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        </table>
     <center>      
         <asp:Button ID="bModificar" runat="server" style="text-align: center" 
           Text="Modificar" onclick="bModificar_Click" />
         <asp:Button ID="bActualizar" runat="server" onclick="bActualizar_Click" 
             Text="Actualizar" Visible="False" />
</center>
</fieldset>
</body>

       

</asp:Content>