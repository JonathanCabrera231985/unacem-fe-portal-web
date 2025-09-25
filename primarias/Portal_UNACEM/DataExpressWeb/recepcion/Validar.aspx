<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Validar.aspx.cs" Inherits="DataExpressWeb.Validar" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p style="font-size: medium; text-align: center">
        <strong>Validador Manual</strong></p>
    <p style="font-size: medium; text-align: center">
        <table style="width: 100%;">
            <tr>
                <td colspan="2" style="text-align: center">
                    <strong>Archivo XML: </strong>
                    <asp:UpdatePanel ID="updpnl3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:FileUpload ID="fuXML" runat="server" ClientIDMode="Static" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="bSubir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    &nbsp;
                    <asp:Button ID="bSubir" runat="server" OnClick="bSubir_Click" Text="Validar Factura" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lMsj" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="tbMsj" runat="server" ForeColor="#003151" Height="388px" ReadOnly="True"
                        TextMode="MultiLine" Width="96%" Font-Size="Small"></asp:TextBox>
                </td>
            </tr>
        </table>
    </p>
    <p style="font-size: medium; text-align: left">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </p>
</asp:Content>
