<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Registro.aspx.cs" Inherits="DataExpressWeb.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent2" runat="server">
    <style type="text/css">
        #Text1
        {
            margin-right: 1px;
        }
        .style1
        {
            color: #FF3300;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent2" runat="server">
    <br />
    <br />
    <br />
    <br />
    <fieldset style="width: 473px">
        <legend>Datos de Registro</legend>Usuario:<br />
        <asp:TextBox ID="tbRfcuser" runat="server" Width="160px" MaxLength="15" ValidationGroup="login"
            CssClass="txt_gr2" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfv_user" runat="server" ControlToValidate="tbRfcuser"
            ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="login"></asp:RequiredFieldValidator>
        <br />
        Contraseña Actual:<br />
        <asp:TextBox ID="tbClaveActual" runat="server" Width="160px" MaxLength="15" ValidationGroup="login"
            CssClass="txt_gr2" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfv_claveActual" runat="server" ControlToValidate="tbClaveActual"
            ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="login"></asp:RequiredFieldValidator>
        <asp:Label ID="lblKeyActual" runat="server" Font-Size="Smaller" ForeColor="Red"></asp:Label>
        <br />
        Nueva Contraseña:<br />
        <asp:TextBox ID="tbPass" runat="server" Width="160px" TextMode="Password" MaxLength="12"
            ValidationGroup="login" CssClass="txt_gr2"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfv_new" runat="server" ControlToValidate="tbPass"
            ErrorMessage="*" ForeColor="Red" SetFocusOnError="True" ValidationGroup="login"></asp:RequiredFieldValidator>
        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="tbPass" 
            ErrorMessage="No cumple requerimientos (1 mayúscula, 1 minúscula, 1 caracter especial, max 12 min 7)" 
            Font-Size="Smaller" ForeColor="Red" 
            ValidationExpression="(?=^.{7,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"></asp:RegularExpressionValidator>--%>
        <br />
        Confirmar Nueva Contraseña:<br />
        <asp:TextBox ID="tbPass0" runat="server" Width="160px" TextMode="Password" MaxLength="15"
            ValidationGroup="login" CssClass="txt_gr2"></asp:TextBox>
        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="tbPass"
            ControlToValidate="tbPass0" ErrorMessage="La contraseña no coincide" ForeColor="Red"
            ValidationGroup="login"></asp:CompareValidator>
        <br />
        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPass"
            ControlToValidate="tbPass0" ErrorMessage="Confirme la contraseña Correctamente"
            CssClass="style1" ValidationGroup="login"></asp:CompareValidator>
        <br />
        <asp:Button ID="bRegistrarse" runat="server" Text="Registrar" OnClick="bRegistrarse_Click"
            CssClass="botones" ValidationGroup="login" />
        <br />
        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:HyperLink ID="hlRegresar" runat="server" NavigateUrl="~/logout.aspx">Regresar</asp:HyperLink>
    </fieldset>
    <br />
    <br />
</asp:Content>
