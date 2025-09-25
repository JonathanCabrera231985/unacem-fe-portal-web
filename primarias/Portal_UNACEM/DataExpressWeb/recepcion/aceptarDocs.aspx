<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="aceptarDocs.aspx.cs" Inherits="DataExpressWeb.aceptarDocs" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Text1
        {
            width: 189px;
            margin-top: 0px;
        }
        .style1
        {
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
        .style2
        {
        }
        .style7
        {
            height: 56px;
            width: 272px;
        }
        .style9
        {
            color: #000000;
        }
        .style12
        {
            height: 17px;
        }
        .style13
        {
            font-size: xx-small;
        }
        .style16
        {
            color: #FFFFFF;
        }
        .style17
        {
            width: 268435456px;
        }
    .style19
    {
        font-size: 9pt;
    }
    </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><strong style="color: #006600">COMPROBANTES ACEPTADOS</strong></center><br />
    <table  style="border-style: none; border-color: inherit; border-width: 1px; width:691px; vertical-align:top; ">
        <tr>
        <td colspan="5" bgcolor="#008834" 
                style="font-weight: 700; font-size: medium; text-align: center"> 
            <span class="style16">Buscador</span> </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                <span class="style9">
                <strong>
                
                <span class="style19">RUC:</span></strong>
                </span>
                <asp:TextBox ID="tbRFC" runat="server" CssClass="style9" MaxLength="13"></asp:TextBox>
                </td>
            <td class="style1" colspan="3">
                <span class="style9"><span class="style19"><strong>Razón Social:</strong></span>
                </span>
                <asp:TextBox ID="tbNombre" runat="server" Width="518px" CssClass="style9"></asp:TextBox>
                <br class="style9" />
                </td>
        </tr>
        <tr valign="top">
            <td class="style7" rowspan="2">
                <strong>Escribe el Folio o Folios:</strong><br />
                <asp:TextBox ID="tbFolioAnterior" runat="server"></asp:TextBox>
                &nbsp;<span class="style13">Formato(1,2,3,4-8)</span><br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ErrorMessage="Formato Incorrecto" ForeColor="Red" 
                    ControlToValidate="tbFolioAnterior" 
                    ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
                </td>
            <td  rowspan="2" colspan="2">
                <strong>Tipo:</strong><br />
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" 
                    DataSourceID="SqlDataSourceTipoDoc" DataTextField="descripcion" 
                    DataValueField="codigo" AppendDataBoundItems="True" 
                    AutoPostBack="True">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
                <br />
            </td>
            <td>
                <strong>Fecha Inicial:</strong><br />
                <asp:TextBox ID="tbFechaInicial" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaInicial_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tbFechaInicial" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                </td>
            <td class="style17" >
                <strong>Fecha Final:<br />
                </strong>
                <asp:TextBox ID="tbFechaFinal" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaFinal_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="tbFechaFinal" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                </td>
        </tr>
        <tr>
            <td class="style12" colspan="2">
                
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="bBuscar" runat="server" onclick="Button1_Click" Text="Buscar" 
                    Width="87px" style="height: 26px"/>
            &nbsp;&nbsp;
                </td>
            <td class="style2" colspan="4">
                <asp:Button ID="bActualizar" runat="server" onclick="Button1_Click1" 
                    Text="Actualizar" />
                <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
        <td colspan="5" bgcolor="#008834" 
                style="font-weight: 700; font-size: xx-small; text-align: center"> 
            &nbsp;</td>
        </tr>
    </table>
    <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CellPadding="4" 
    DataSourceID="SqlDataSource1" GridLines="Horizontal" Width="80%" BackColor="White" 
    BorderColor="#336666" BorderWidth="3px" BorderStyle="Double" style="margin-top: 0px" 
    AllowPaging="True" AllowSorting="True" onrowcommand="gvFacturas_RowCommand" 
        DataKeyNames="ID" onselectedindexchanged="gvFacturas_SelectedIndexChanged"  >
    <Columns>
        <asp:TemplateField HeaderText="RUC" SortExpression="rucProveedor">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("rucProveedor") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("rucProveedor") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PROVEEDOR" SortExpression="razonSocialProv">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("razonSocialProv") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label3" runat="server" Text='<%# Bind("razonSocialProv") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="TIPODOC" SortExpression="codDoc">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("codDoc") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label13" runat="server" Text='<%# Bind("codDoc") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SECUENCIA" SortExpression="FOLFAC">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("FOLFAC") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle Width="10%" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle Width="10%" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="FECHA" SortExpression="FECHA">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("FECHA") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("FECHA") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle Width="15%" />
        </asp:TemplateField>
        <asp:ImageField DataImageUrlField="EDOFAC" DataImageUrlFormatString="~/Imagenes/estado{0}.jpg" HeaderText="Estado">
            <ControlStyle Height="20px" Width="20px" />
<ControlStyle Height="20px" Width="20px"></ControlStyle>

            <HeaderStyle Width="5%" />
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:ImageField>
        <asp:TemplateField HeaderText="XML">
            <ItemTemplate>
            <a href='../download.aspx?file=<%# Eval("XMLARC") %>'>
                <img  src="../imagenes/xml32x32.png" alt="xml" border="0" align="middle" 
                    height="22" width="22"></a>
            </ItemTemplate> 
            <HeaderStyle Width="5%" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PDF">
            <ItemTemplate>
            <a href='../download.aspx?file=<%# Eval("PDFARC") %>'>
                <img  src="../imagenes/pdf32x32.png" alt="pdf" border="0" align="middle" 
                    height="22" width="22"></a>
            </ItemTemplate>         
            <HeaderStyle Width="5%" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Acción">
            <ItemTemplate>
                &nbsp;
                <asp:ImageButton ID="ibAceptar" runat="server" CommandArgument="Aceptar" 
                    CommandName="Aceptar" Height="35px" ImageUrl="~/imagenes/Check-128.png" 
                    Width="35px" />
                <asp:ConfirmButtonExtender ID="ibAceptar_ConfirmButtonExtender" runat="server" 
                    ConfirmText="Estas seguro de aceptar esta factura se generaran sus retenciones automaticamente." 
                    Enabled="True" TargetControlID="ibAceptar">
                </asp:ConfirmButtonExtender>
                &nbsp;
                <asp:ImageButton ID="ibRechazar" runat="server" CommandArgument="Rechazar" 
                    CommandName="Rechazar" Height="35px" ImageUrl="~/imagenes/Delete-128.png" 
                    Width="35px" />
                <asp:ConfirmButtonExtender ID="ibRechazar_ConfirmButtonExtender" runat="server" 
                    ConfirmText="Si rechazas volvera para la revisión de la mercancia." 
                    Enabled="True" TargetControlID="ibRechazar">
                </asp:ConfirmButtonExtender>
            </ItemTemplate>
            <HeaderStyle Width="100px" />
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
        <EmptyDataTemplate>
            No existen datos.
        </EmptyDataTemplate>
    <FooterStyle BackColor="White" ForeColor="#008834" />
    <HeaderStyle BackColor="#008834" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#008834" ForeColor="White" 
            HorizontalAlign="Center" />
        <RowStyle BackColor="White" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#339966" ForeColor="White" Font-Bold="True" />
    <SortedAscendingCellStyle BackColor="#F7F7F7" />
    <SortedAscendingHeaderStyle BackColor="#487575" />
    <SortedDescendingCellStyle BackColor="#E5E5E5" />
    <SortedDescendingHeaderStyle BackColor="#275353" />

<SortedAscendingCellStyle BackColor="#F7F7F7"></SortedAscendingCellStyle>

<SortedAscendingHeaderStyle BackColor="#487575"></SortedAscendingHeaderStyle>

<SortedDescendingCellStyle BackColor="#E5E5E5"></SortedDescendingCellStyle>

<SortedDescendingHeaderStyle BackColor="#275353"></SortedDescendingHeaderStyle>
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="PA_Proceso_Aceptacion" SelectCommandType="StoredProcedure" 
        DeleteCommand="UPDATE GENERAL SET estado = '1' WHERE (idComprobante = @idComprobante)" 
        UpdateCommand="UPDATE GENERAL SET estado = '3' WHERE (idComprobante = @idComprobante)">
    <DeleteParameters>
        <asp:Parameter Name="idComprobante" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
        <asp:SessionParameter DefaultValue="S--X" Name="SUCURSAL" 
            SessionField="sucursalUser" Type="String" />
        <asp:SessionParameter DefaultValue="R---" Name="RFC" SessionField="rfcCliente" 
            Type="String" />
        <asp:SessionParameter DefaultValue="false" Name="ROL" SessionField="coFactTodas" 
            Type="Boolean" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="idComprobante" />
    </UpdateParameters>
</asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WHERE (tipo = 'Comprobante')">
                </asp:SqlDataSource>
<br />
</asp:Content>