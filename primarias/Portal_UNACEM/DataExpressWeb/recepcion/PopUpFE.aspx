<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopUpFE.aspx.cs" Inherits="DataExpressWeb.recepcion.PopUpFE" %>


<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Consulta Documentos</title>
    <meta http-equiv="X-UA-Compatible" content="IE=9,chrome=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
</head>

<body style="width: 1087px; height: 475px">
<script type = "text/javascript">
    function OnClose() {
        if (window.opener != null && !window.opener.closed) {
            window.opener.HideModalDiv();
        }
    }
    window.onunload = OnClose;
</script>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Label ID="lblMensaje" runat="server" Text="" Visible="false"></asp:Label>
    
        <CR:CrystalReportViewer ID="CRV_FE" runat="server" AutoDataBind="true" 
            EnableParameterPrompt="False" HasToggleGroupTreeButton="False" 
            HasCrystalLogo="False" EnableDatabaseLogonPrompt="False" HasToggleParameterPanelButton="False" ToolPanelView="None" />
    
        <asp:TextBox ID="txtXml" runat="server" Height="606px" ReadOnly="True" 
            TextMode="MultiLine" Visible="False" Width="863px"></asp:TextBox>
        <br />       
    </div>
    </form>
</body>
</html>
