<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="DataExpressWeb._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Bienvenido a Invoicec </h2>    
    <asp:Image ID="Image2" runat="server" Height="48px" 
        ImageUrl="~/imagenes/logo.png" Width="156px" />
     <br />
     <br />
     <br />
        <asp:HyperLink ID="HLContraseña" runat="server" ForeColor = "Blue"
          NavigateUrl="~/cuenta/Registro.aspx" Font-Bold="True" 
        Font-Underline="True">Cambiar Contraseña</asp:HyperLink>
     <br />
     <br />
     <br />
    <p>
        Para obtener más información acerca de CimaIT, visite <a style="color: #0009FF; " href="http://www.cimait.com.ec/" title="Sitio web de CimaIT">http://www.cimait.com.ec</a>.
    </p>
    <p>
        ® CimaIT Derechos Reservados. Ecuador, Diciembre 2012.</p>

</asp:Content>
