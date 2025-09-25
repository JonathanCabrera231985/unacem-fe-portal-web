<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agregarEmpresa.aspx.cs" Inherits="DataExpressWeb.adminstracion.empresas.agregarEmpresa" MasterPageFile="~/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .style1
    {
        width: 111px;
    }
    .style2
    {
        width: 400px;
    }
    .style3
    {
        width: 150px;
    }
    .style4
    {
        width: 20px;
    }
    .style5
    {
        color: #000000;
    }
    .style7
    {
        text-align: center;
        font-size: large;
        color: #002F5A;
    }
        #btocargarImagen
        {
            width: 52px;
        }
        .style8
        {
            width: 302px;
        }
    p.MsoNormal
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:10.0pt;
	margin-left:0cm;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	}
        .style9
        {
            width: 150px;
            height: 27px;
        }
        .style10
        {
            width: 400px;
            height: 27px;
        }
        .style11
        {
            height: 27px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script  type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function validarContrasena() {
            var contrasena1 = document.getElementById('<%= tbcontrasena.ClientID %>').value;
            var contrasena2 = document.getElementById('<%= tbcontrasenaRecepcion.ClientID %>').value;
            var contrasena3 = document.getElementById('<%= tbClave.ClientID %>').value;
            if (contrasena1.trim() === "" || contrasena2.trim() === "" || contrasena3.trim() === "") {
                    alert("El campo contraseña no puede estar vacío.");
                    return false; // evita el postback
                }
                return true; // permite el postback
            }
    </script>
    <table style="width:100%;">
    <tr>
        <td class="style7" colspan="4"  bgcolor="e1ddd9" 
            style="font-weight: 700; font-size: medium; text-align: center">
            <strong>AGREGAR EMPRESA</strong></td>
    </tr>
    <tr>
        <td class="style3">
            RUC:</td>
        <td class="style2">
            <asp:TextBox ID="tbRUC" runat="server" CssClass="style5"  Width="250px" 
                MaxLength="13" ontextchanged="tbRUC_TextChanged"></asp:TextBox>
            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbRUC" ErrorMessage="Requiere RUC" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td class="style8">
            Logo Empresa</td>
         <td> <asp:FileUpload ID="FileUploadLogo" runat="server"/></td>
    </tr>
    <tr>
        <td class="style3">
            Razon Social:</td>
        <td class="style2">
            <asp:TextBox ID="tbRazonSocial" runat="server" CssClass="style5"  Width="386px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbRazonSocial" ErrorMessage="Requiere Razon Social" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td class="style8">
            <asp:Button runat="server" ID="btoImangen" Text="Visualizar logo" 
                onclick="btoImangen_Click"/></td>
        <td rowspan="3">
            <asp:Image ID="ImageLogo" runat="server"  Height="49px" Width="310px"/>
        </td>
    </tr>
    <tr>
        <td class="style3">
            Nombre Comercial:</td>
        <td class="style2">
            <asp:TextBox ID="tbNombreComercial" runat="server"  Width="386px" CssClass="style5"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            Matríz:</td>
        <td class="style2">
            <asp:TextBox ID="tbMatriz" runat="server"  Width="386px" CssClass="style5"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                ControlToValidate="tbMatriz" ErrorMessage="Requiere Matríz" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            Firma Electrónica</td>
        <td class="style2">
            <asp:TextBox ID="tbFirmaElectronica" runat="server"  Width="386px" CssClass="style5"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                ControlToValidate="tbFirmaElectronica" ErrorMessage="Requiere Firma Electrónica" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td class="style3">
            Pass Firma Electrónica</td>
        <td class="style8">
            <asp:TextBox ID="tbClave" runat="server" Width="150px" TextMode="Password"></asp:TextBox></td>
    </tr>
        <tr>
        <td class="style3">
            Contribuyente Especial No.</td>
        <td class="style2">
            &nbsp;
            <asp:TextBox ID="txtConEspecial" runat="server"></asp:TextBox>
        </td>
        <td class="style3">
            <p class="MsoNormal">
                <span style="font-size:9.5pt;line-height:115%;font-family:
&quot;Segoe UI&quot;,&quot;sans-serif&quot;;color:dimgray">Dirección de Documentos</span><o:p></o:p></p>
            </td>
        <td class="style8">
            <asp:TextBox ID="txtDirDoc" runat="server" Width="282px"></asp:TextBox>
            </td>
    </tr>
        <tr>
        <td class="style3">
            Obligado&nbsp; contabilidad</td>
        <td class="style2">
            <asp:CheckBox ID="chkContabilidad" runat="server" />
        </td>
        <td class="style3">
            Direccion de XML</td>
        <td class="style8">
            <asp:TextBox ID="txtDirXml" runat="server" Width="282px"></asp:TextBox>
            </td>
    </tr>
        <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style2">
            &nbsp;</td>
        <td class="style3">
            Teléfonos</td>
        <td class="style8">
            <asp:TextBox ID="txtTelefonos" runat="server"></asp:TextBox>
            </td>
    </tr>
        <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style2">
            &nbsp;</td>
        <td class="style8">
            &nbsp;</td>
        <td class="style8">
            &nbsp;</td>
    </tr>
</table>
<table style="width:100%;">
    <tr>
        <td class="style7" colspan="5"  bgcolor="#E1DDD9" 
            style="font-weight: 700; font-size: medium; text-align: center">
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
            <asp:TextBox ID="tbServidor" runat="server" CssClass="style5"  Width="386px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="tbServidor" ErrorMessage="Requiere Servidor SMTP" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
        Base de datos:</td>
        <td class="style2">
            <asp:DropDownList ID="cboTipoconexion" runat="server" DataSourceID="SqlDataSourceBDl" DataTextField="descripcion" DataValueField="codigo">
            </asp:DropDownList>
        </td>
    </tr>
   <tr>
        <td class="style3">
            Puerto:</td>
        <td class="style2">
            <asp:TextBox ID="tbPuerto" runat="server" CssClass="style5" onkeypress="return isNumberKey(event)"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="tbPuerto" ErrorMessage="Requiere puerto Servidor" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
        Cadena de Conexión:</td>
        <td class="style2" rowspan="5">
            <asp:TextBox ID="tbcadenaConexion" runat="server" CssClass="style5" 
                TextMode="MultiLine" Height="130px" Width="366px" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ControlToValidate="tbcadenaConexion" ErrorMessage="Requiere Cadena de Conexión" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style3">
            Usuario:</td>
        <td class="style2">
           <asp:TextBox ID="tbusuario" runat="server" CssClass="style5" Width="386px" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ControlToValidate="tbusuario" ErrorMessage="Requiere Usuario de correo" ForeColor="Red">*</asp:RequiredFieldValidator>
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
           <asp:TextBox ID="tbcontrasena" runat="server" CssClass="style5" Width="386px" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                ControlToValidate="tbcontrasena" ErrorMessage="Requiere contraseña de correo" ForeColor="Red">*</asp:RequiredFieldValidator>
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
           <asp:CheckBox ID="cbSSL" runat="server"/>
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            E-mail de envio:</td>
        <td class="style2">
           <asp:TextBox ID="tbemailenvio" runat="server" CssClass="style5" Width="386px" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                ControlToValidate="tbemailenvio" ErrorMessage="Requiere Email de envio" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            Configuración Recepción:</td>
        <td class="style2">
           <asp:RadioButtonList runat="server" ID="radioConfig" OnSelectedIndexChanged="checkConfig_SelectedIndexChanged" AutoPostBack="true">
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
            <asp:TextBox runat="server" ID="txtURL" Width="360px"></asp:TextBox> <br />
            <asp:RadioButtonList runat="server" ID="RadioExchange">
                <asp:ListItem Selected="True" Value="2007" Text="Exchange 2007"></asp:ListItem>
                <asp:ListItem Value = "2010" Text="Exchange 2010"></asp:ListItem>
                <asp:ListItem Value = "2013" Text="Exchange 2013"></asp:ListItem>
           </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td class="style9">
            Servidor<br />
            Recepción:</td>
        <td class="style10">
            <asp:TextBox ID="tbServidorR" runat="server" CssClass="style5"  Width="386px"></asp:TextBox>
            
        </td>
        <td class="style11">
            </td>
        <td class="style9">
            </td>
        <td class="style10">
        </td>
    </tr>
    <tr>
        <td class="style3">
            E-mail de Recepción:</td>
        <td class="style2">
           <asp:TextBox ID="tbcorreoRecepcion" runat="server" CssClass="style5" Width="386px" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                ControlToValidate="tbcorreoRecepcion" ErrorMessage="Requiere Email de recepción" ForeColor="Red">*</asp:RequiredFieldValidator>
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
           <asp:TextBox ID="tbcontrasenaRecepcion" runat="server" CssClass="style5" Width="386px" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                ControlToValidate="tbcontrasenaRecepcion" ErrorMessage="Requiere contraseña" ForeColor="Red">*</asp:RequiredFieldValidator>
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
                TextMode="MultiLine" Height="79px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                ControlToValidate="tbmensajecorreo" ErrorMessage="Requiere Mensaje del correo" ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
            &nbsp;</td>
        <td class="style2">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style2">
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" />
            <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
            <br />
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
            &nbsp;</td>
        <td class="style2">
            <asp:Button ID="bGuardar" runat="server" Text="Guardar"  OnClientClick="return validarContrasena();"
                onclick="bGuardar_Click" />
        </td>
        <td>
            &nbsp;</td>
        <td class="style3">
            &nbsp;</td>
         <td>
        &nbsp;</td>
    </tr>
</table>
 <asp:SqlDataSource ID="SqlDataSourceBDl" runat="server" 
                ConnectionString="<%$ ConnectionStrings:upsdataConnectionString %>" 
                SelectCommand="select 0 codigo, 'Selecciona el Rol' descripcion  union select codigo, descripcion from Catalogo1_C where tipo= 'Base de datos' ">
            </asp:SqlDataSource>
</asp:Content>