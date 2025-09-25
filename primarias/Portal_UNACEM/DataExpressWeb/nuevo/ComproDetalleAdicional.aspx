<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComproDetalleAdicional.aspx.cs" Inherits="DataExpressWeb.nuevo.ComproDetalleAdicional" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle Adocionales</title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" Text="B/L Nº:" ID="Label1"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txt_bl"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="BUQUE:" ID="Label2"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txt_buque"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="VIAJE:" ID="Label3"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txt_viaje"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" Style="text-align: center">
                    <asp:Button runat="server" ID="bto_asignar" Text="Guardar" 
                        onclick="bto_asignar_Click"/>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
