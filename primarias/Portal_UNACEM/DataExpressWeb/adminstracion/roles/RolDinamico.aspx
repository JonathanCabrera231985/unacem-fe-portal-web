<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolDinamico.aspx.cs" MasterPageFile="~/Site.Master" Inherits="DataExpressWeb.adminstracion.roles.RolDinamico" %>

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
    <center><strong><span class="style2">Roles</span></strong></center><br />
    <table>
        <tr>
            <td class="style1">
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="357px">
                    <asp:ListBox ID="ListBox1" runat="server" Height="218px" Width="312px" 
                        DataTextField= "descripcion" DataValueField="idRol"  
                        DataSourceID="SqlDataSource1" AutoPostBack="true" 
                        onselectedindexchanged="ListBox1_SelectedIndexChanged">
                    </asp:ListBox>                     
                 </asp:Panel>
                 <asp:Button id="btoEditar" runat="server" Text="Editar" onclick="btoEditar_Click"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button id="btoEliminar" runat="server" Text="Eliminar"  OnClientClick="return confirm('Si eliminas el Rol, se eliminaran los Empleados asociado a el. ¿Deseas eliminar el rol?');"
                    onclick="btoEliminar_Click"/>
                    <asp:Label ID="lbmensaje" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
            <td class="style4">
              <asp:Panel runat="server" ID="panel2" ScrollBars="Auto" Height="355px">
                <asp:TreeView id="LinksTreeView" Font-Names= "Arial" ForeColor="Blue"  EnableClientScript="false" runat="server" >
                  </asp:TreeView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="select idRol, descripcion from Roles WHERE eliminado='False'" >
    </asp:SqlDataSource>
</asp:Content>