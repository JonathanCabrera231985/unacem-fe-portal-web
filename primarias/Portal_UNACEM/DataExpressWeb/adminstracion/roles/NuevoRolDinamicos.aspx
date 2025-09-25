<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoRolDinamicos.aspx.cs" MasterPageFile="~/Site.Master" Inherits="DataExpressWeb.adminstracion.roles.NuevoRolDinamicos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            font-size: large;
        }
        .style1
        {
            width: 300px;
        }
        .style4
        {
            width: 800px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><strong><asp:Label ID="lbtextocabecera" runat="server" Text="Crear Roles" /></strong></center><br />
    <table>
        <tr>
            <td class="style1">
                <asp:Panel ID="Panel1" runat="server" style="text-align: left" >
                    <asp:Label ID="Label1" runat="server" Text="Descripción del rol:" ForeColor="Black" /> <br />
                    <asp:TextBox ID="txtdescripcionRol" runat="server"></asp:TextBox>
                </asp:Panel>
            </td>
            <td class="style4" rowspan="9">
                <asp:Panel runat="server" ID="panel2" ScrollBars="Auto" Height="355px">
                    <asp:TreeView id="LinksTreeView" Font-Names= "Arial" ForeColor="Blue"  
                        EnableClientScript="false" runat="server" 
                        ontreenodecheckchanged="LinksTreeView_TreeNodeCheckChanged" >
                    </asp:TreeView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Button runat="server" ID="btoGuardar" Text="Guardar" 
                    OnClientClick="return confirm('Esta Seguro que desea guardar la información');" 
                    onclick="btoGuardar_Click" /> <br />
                    <asp:Label ID="lbmensaje" runat="server" Text="" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
        <tr>
            <td class="style1">
            </td>
        </tr>
    </table>
</asp:Content>