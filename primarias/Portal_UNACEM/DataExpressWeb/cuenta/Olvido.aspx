<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true"
    CodeBehind="Olvido.aspx.cs" Inherits="DataExpressWeb.Olvido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent2" runat="server">
    <style type="text/css">
        #Text1
        {
            margin-right: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent2" runat="server">
    <br />
    <br />
    <br />
    <br />
    <table>
        <tr>
            <td>
                <fieldset style="width: 287px">
                    <legend>Ingrese su identificación</legend>Número de Identificación del Usuario:<br />
                    <asp:TextBox ID="user" runat="server" CssClass="txt_gr2" Width="189px"></asp:TextBox>
                    <br />
                    La Contraseña se enviará a su email.<br />
                    <br />
                    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="bEnviar" runat="server" Text="Enviar" OnClick="bEnviar_Click" 
                        CssClass="botones" />
                    <br />
                    <br />
                    <asp:HyperLink ID="hlRegresar" runat="server" NavigateUrl="~/default.aspx">Regresar</asp:HyperLink>
                </fieldset>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
