<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="enviaMail.aspx.cs" Inherits="DataExpressWeb.recepcion.enviaMail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Documentos</title>
</head>
<body style="width: 879px; height: 475px">
    <script type="text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                window.opener.HideModalDiv();
            }
        }
        window.onunload = OnClose;
    </script>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblMailProv" runat="server" Style="font-weight: 700" Text="Correo del Proveedor: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailProv" runat="server" Height="22px" Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAsunto" runat="server" Text="Asunto:" 
                        style="font-weight: 700; text-align: right"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAsunto" runat="server" Height="22px" Width="600px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:TextBox ID="txtMensaje" runat="server" Height="336px" TextMode="MultiLine" 
            Width="863px"></asp:TextBox>
        <br />
        <asp:Label ID="lblMensaje" runat="server" Visible="False" ForeColor="Red"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btnMail" runat="server" Text="Enviar Mail" 
            onclick="btnMail_Click" CausesValidation="False" 
            UseSubmitBehavior="False" />
        <%--<asp:ConfirmButtonExtender ID="btnMail_ConfirmButtonExtender" runat="server" ConfirmText="Mail Enviado"
                        Enabled="True" TargetControlID="btnMail">
                    </asp:ConfirmButtonExtender>--%>
    </div>
    </form>
</body>
</html>
