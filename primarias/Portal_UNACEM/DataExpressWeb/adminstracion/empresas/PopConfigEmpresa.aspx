<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopConfigEmpresa.aspx.cs" Inherits="DataExpressWeb.adminstracion.empresas.PopConfigEmpresa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
 <script type = "text/javascript">
     function OnClose() {
         if (window.opener != null && !window.opener.closed) {
             window.opener.HideModalDiv();
         }
     }
     window.onunload = OnClose;
    </script>
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style5
        {}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%;">
        <tr>
            <td>
                <asp:Image ID="ImageLogo" runat="server"  Height="49px" Width="310px"/>
            </td>
        </tr>
        <tr>
            <td class="style7" colspan="5"  bgcolor="#4580A8" style="font-weight: 700; font-size: medium; text-align: center">
                <strong>CONFIGURACIONES</strong></td>
        </tr>
        <tr>
            <td colspan= "2" style="font-weight: 700; font-size: medium; text-align: center"> <strong>CORREO</strong></td>
            <td class="style4"> &nbsp;</td>
            <td colspan= "2"  style="font-weight: 700; font-size: medium; text-align: center"> 
                <strong>BASE DE DATOS</strong></td>
        </tr>
        <tr>
            <td class="style3">
                Servidor SMTP:</td>
            <td class="style2">
                <asp:TextBox ID="tbServidor" runat="server" CssClass="style5"  Width="386px" 
                    Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
            Base de datos:</td>
            <td class="style2">
                <asp:DropDownList ID="cboTipoconexion" runat="server" 
                    DataSourceID="SqlDataSourceBDl" DataTextField="descripcion" 
                    DataValueField="codigo" Enabled="False">
                </asp:DropDownList>
            </td>
        </tr>
       <tr>
            <td class="style3">
                Puerto:</td>
            <td class="style2">
                <asp:TextBox ID="tbPuerto" runat="server" CssClass="style5" 
                    onkeypress="return isNumberKey(event)" Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
            Cadena de Conexión:</td>
            <td class="style2" rowspan="5">
                <asp:TextBox ID="tbcadenaConexion" runat="server" CssClass="style5" 
                    TextMode="MultiLine" Height="121px" Width="376px" Enabled="False" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style3">
                Usuario:</td>
            <td class="style2">
               <asp:TextBox ID="tbusuario" runat="server" CssClass="style5" Width="386px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
        </tr>
         <tr>
            <td class="style3">
                Contraseña:</td>
            <td class="style2">
               <asp:TextBox ID="tbcontrasena" runat="server" TextMode="Password" CssClass="style5" Width="386px" Enabled="False" ></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                 SSL:</td>
            <td class="style2">
               <asp:CheckBox ID="cbSSL" runat="server" Enabled="False" />
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                E-mail de envio:</td>
            <td class="style2">
               <asp:TextBox ID="tbemailenvio" runat="server" CssClass="style5" Width="386px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
        </tr>
    <tr>
        <td class="style3">
            Configuración Recepción:</td>
        <td class="style2">
           <asp:RadioButtonList runat="server" ID="radioConfig" Enabled="false">
                <asp:ListItem Selected="True" Value="IMAP" Text="ImapClient"></asp:ListItem>
                <asp:ListItem Value = "POP3" Text="POP3"></asp:ListItem>
                <asp:ListItem Value = "SMTP" Text="SMTP"></asp:ListItem>
           </asp:RadioButtonList>
            
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
            <asp:Label runat="server" ID="lbConfigXMTP" Text="Configuracion SMTP"></asp:Label></td>
        <td class="style2">
            <asp:Label runat="server" ID="lbURL" Text="URL"></asp:Label>
            <asp:TextBox runat="server" ID="txtURL" Width="360px" Enabled="false"></asp:TextBox> <br />
            <asp:RadioButtonList runat="server" ID="RadioExchange" Enabled="false">
                <asp:ListItem Selected="True" Value="2007" Text="Exchange 2007"></asp:ListItem>
                <asp:ListItem Value = "2010" Text="Exchange 2010"></asp:ListItem>
                <asp:ListItem Value = "2013" Text="Exchange 2013"></asp:ListItem>
           </asp:RadioButtonList>
        </td>
    </tr>

        <tr>
            <td class="style3">
                E-mail de Recepción:</td>
            <td class="style2">
               <asp:TextBox ID="tbcorreoRecepcion" runat="server" CssClass="style5" Width="386px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                Contraseña Recepción:</td>
            <td class="style2">
               <asp:TextBox ID="tbcontrasenaRecepcion" TextMode="Password" runat="server" CssClass="style5" Width="386px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style3">
                Mensaje de Correo:</td>
            <td class="style2">
               <asp:TextBox ID="tbmensajecorreo" runat="server" CssClass="style5" Width="386px" 
                    TextMode="MultiLine" Height="113px" Enabled="False"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlDataSourceBDl" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:upsdataConnectionString %>" 
                    SelectCommand="select 0 codigo, 'Selecciona el Rol' descripcion  union select codigo, descripcion from Catalogo1_C where tipo= 'Base de datos' ">
                </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
