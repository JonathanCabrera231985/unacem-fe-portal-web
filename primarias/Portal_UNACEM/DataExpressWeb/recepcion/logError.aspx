<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="logError.aspx.cs" Inherits="DataExpressWeb.recepcion.logError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Text1
        {
            width: 189px;
            margin-top: 0px;
        }
        #Text2
        {
            width: 474px;
        }
        #Text3
        {
            width: 164px;
        }
        #Text5
        {
            width: 139px;
        }
        #Text6
        {
            width: 138px;
        }
        .style1
        {
        }
        .style7
        {
            width: 146px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
        <tr>
            <td class="style7">
                No. Documento:
            </td>
            <td>
                <asp:TextBox ID="tbNoDoc" runat="server" CssClass="txt_gr2" Width="231px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style7">
                Ruc Proveedor:
            </td>
            <td>
                <asp:TextBox ID="tbRucProv" runat="server" Height="21px" Width="231px" 
                    CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style7">
                Tipo de Documento:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc"
                    DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                    AutoPostBack="True">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style7">
                Clave Acceso:
            </td>
            <td>
                <asp:TextBox ID="tbClaveAcceso" runat="server" Width="231px" CssClass="txt_gr2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style7">
                <asp:Button ID="bBuscarReg" runat="server" OnClick="bBuscarReg_Click" Text="Buscar"
                    CssClass="botones" />
            </td>
            <td>
                <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" Text="Actualizar"
                    CssClass="botones" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Width="904px"
        AllowPaging="True" DataKeyNames="idErrorFactura">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="idErrorFactura" HeaderText="idErrorFactura" SortExpression="idErrorFactura"
                InsertVisible="False" ReadOnly="True" Visible="False"></asp:BoundField>
            <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" />
            <asp:BoundField DataField="detalle" HeaderText="Detalle" SortExpression="detalle" />
            <asp:BoundField DataField="razonSocialProv" HeaderText="Proveedor" SortExpression="razonSocialProv" />
            <asp:BoundField DataField="codDocDesc" HeaderText="Documento" SortExpression="codDocDesc" />
            <asp:BoundField DataField="numeroDocumento" HeaderText="No. Documento" SortExpression="numeroDocumento" />
            <asp:BoundField DataField="claveAcceso" HeaderText="Clave de Acceso" SortExpression="claveAcceso" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <EmptyDataTemplate>
            No existen datos.
        </EmptyDataTemplate>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign= "Center" />
        <RowStyle BackColor="#DEE2EB" />
        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
        <SortedAscendingCellStyle BackColor="#F7F7F7"></SortedAscendingCellStyle>
        <SortedAscendingHeaderStyle BackColor="#487575"></SortedAscendingHeaderStyle>
        <SortedDescendingCellStyle BackColor="#E5E5E5"></SortedDescendingCellStyle>
        <SortedDescendingHeaderStyle BackColor="#275353"></SortedDescendingHeaderStyle>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SP_Recepcion" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="<INSTRUCCION><FILTRO><opcion>4</opcion><query>-</query></FILTRO></INSTRUCCION>"
                Name="documentoXML" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WHERE (tipo = 'Comprobante')">
    </asp:SqlDataSource>
    <br />
</asp:Content>
