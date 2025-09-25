<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CargaContigencia.aspx.cs" Inherits="DataExpressWeb.CargaContigencia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style6
        {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p style="font-size: medium; text-align: center">
        <b>Carga de Claves de Contigencia</b></p>
    <table style="width: 100%;">
        <tr>
            <td colspan="1" style="text-align: center">
                <asp:Label ID="busca" runat="server" Text="1. Buscar Archivo" CssClass="texgridtitulo"></asp:Label>
            </td>
            <td>
                <asp:UpdatePanel ID="updpnl3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:FileUpload ID="exami" runat="server" ClientIDMode="Static"
                            Height="40px" Width="300px" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="bSubir" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Label ID="infor" runat="server" Text="2. Subir archivo para procesarlo" CssClass="texgridtitulo"></asp:Label>
            </td>
            <td>
                <asp:Button ID="bSubir" runat="server" OnClick="bSubir_Click2" Text="Subir Archivo"
                    Width="120px" CssClass="botones" Height="40px" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Label ID="infor0" runat="server" Text="3. Procesar Archivo" CssClass="texgridtitulo"></asp:Label>
            </td>
            <td>
                <asp:Button ID="process" runat="server" Width="120px" Enabled="False" Text="Procesar Archivo"
                    OnClick="process_Click" CssClass="botones" Height="40px" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center" colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center">
                <asp:Label ID="msj" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
