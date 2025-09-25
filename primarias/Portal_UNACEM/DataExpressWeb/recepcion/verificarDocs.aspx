<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="verificarDocs.aspx.cs" Inherits="DataExpressWeb.verificarDocs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">  </asp:ScriptManager>--%>
    <script type="text/javascript">
        var popUpObj;
        function showModalPopUp(pagina) {
            popUpObj = window.open(pagina,
    "ModalPopUp",
    "toolbar=no," +
    "scrollbars=no," +
    "location=no," +
    "statusbar=no," +
    "menubar=no," +
    "resizable=0," +
    "width=800," +
    "height=600," +
    "left = 340," +
    "top=70"
    );
            popUpObj.focus();
            LoadModalDiv();
        }

        function showModalPopUp2(pagina) {
            popUpObj = window.open(pagina,
    "ModalPopUp",
    "toolbar=no," +
    "scrollbars=no," +
    "location=no," +
    "statusbar=no," +
    "menubar=no," +
    "resizable=0," +
    "width=800," +
    "height=600," +
    "left = 340," +
    "top=70"
    );
        }


        function LoadModalDiv() {
            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "block";
            if (bcgDiv != null) {
                if (document.body.clientHeight > document.body.scrollHeight) {
                    bcgDiv.style.height = document.body.clientHeight + "px";
                }
                else {
                    bcgDiv.style.height = document.body.scrollHeight + "px";
                }
                bcgDiv.style.width = "100%";
            }
        }

        function HideModalDiv() {
            var bcgDiv = document.getElementById("divBackground");
            bcgDiv.style.display = "none";
        }

    </script>
    <div id="divBackground" style="position: absolute; top: 0px; left: 0px; background-color: black;
        z-index: 100; opacity: 0.8; filter: alpha(opacity=60); -moz-opacity: 0.8; overflow: hidden;
        display: none">
    </div>
    <center>
        <strong>RECEPCIÓN DE DOCUMENTOS</strong></center>
    <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
        vertical-align: top;">
        <tr>
            <td colspan="4" bgcolor="#5F6062" style="font-weight: 700; font-size: medium; text-align: center">
                <span class="style16">Buscador</span>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Tipo de documento: </strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc"
                    DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                    AutoPostBack="True">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="style1" colspan="2">
                <br class="style9" />
            </td>
        </tr>
        <tr valign="top">
            <td >
                <strong>Número de Documento:</strong>
            </td>
            <td>
                <asp:TextBox ID="tbFolioAnterior" runat="server" Width="150px" Height="15px"></asp:TextBox>
                &nbsp;<span class="style13">Formato(1,2,3,4-8)</span><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato Incorrecto"
                    ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
            </td>
            <td>
                <strong>Fecha Inicial:</strong>
            </td>
            <td>
                
                <asp:TextBox ID="tbFechaInicial" runat="server" Height="15px"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaInicial_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaInicial" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
            
        </tr>
        <tr>
            <td>
                <strong>RUC:</strong></td>
            <td>
                <asp:TextBox ID="tbRFC" runat="server" CssClass="style9" MaxLength="13" 
                    Width="150px" Height="15px"></asp:TextBox>
            </td>
            <td>
            <strong>Fecha Final:</strong>
            </td>
            <td>                
                <asp:TextBox ID="tbFechaFinal" runat="server" Height="15px"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaFinal_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaFinal" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td >
                <strong>Razón Social:</strong>
            </td>
            <td  colspan="2">
                <asp:TextBox ID="tbNombre" runat="server" Width="350px" CssClass="style9" 
                    Height="15px"></asp:TextBox>
            </td>
            <td>
            </td>
           
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="bBuscar" runat="server" OnClick="Button1_Click" Text="Buscar" Width="87px"
                    Style="height: 26px" CssClass="botones" />
                
            </td>
            <td colspan="3">
                <asp:Button ID="bActualizar" runat="server" OnClick="Button1_Click1" 
                    Text="Actualizar" Width="87px"
                    Style="height: 26px" CssClass="botones" />
                <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label><br />
            </td>
        </tr>
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: xx-small; text-align: center">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDataSource1" GridLines="Horizontal" Width="90%" BackColor="White"
        BorderColor="#336666" BorderWidth="3px" BorderStyle="Double" Style="margin-top: 0px"
        AllowPaging="True" AllowSorting="True" OnRowCommand="gvFacturas_RowCommand" DataKeyNames="rucReceptor,rucProveedor,claveAcceso,numeroAutorizacion">
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
                    <asp:Label ID="Label13" runat="server" Text='<%# leeCodDoc(Eval("codDoc").ToString()) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="NO. DOCUMENTO" SortExpression="secuencial">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox5" runat="server" Text='<%# Eval("estab").ToString() + Eval("ptoEmi").ToString() + Eval("secuencial").ToString() %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("estab").ToString() + Eval("ptoEmi").ToString() + Eval("secuencial").ToString() %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="10%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="TOTAL" SortExpression="total">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("total") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("total") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="FECHA" SortExpression="fecha">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("fecha") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("fecha") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="15%" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:ImageField DataImageUrlField="estado" DataImageUrlFormatString="~/Imagenes/estado{0}.jpg"
                HeaderText="Estado">
                <ControlStyle Height="20px" Width="20px" />
                <ControlStyle Height="20px" Width="20px"></ControlStyle>
                <HeaderStyle Width="5%" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:ImageField>
            <asp:BoundField DataField="estadoDesc" SortExpression="estadoDesc" />
            <asp:TemplateField HeaderText="XML">
                <ItemTemplate>
                    <a href="javascript://" onclick="showModalPopUp('../recepcion/PopUpFE.aspx?CA=<%# Eval("claveAcceso") %>&tipo=1')">
                        <%--<a href='../download.aspx?file=<%# Eval("xmlDoc") %>'>--%>
                        <img src="../imagenes/xml32x32.png" alt="xml" border="0" align="middle" height="22"
                            width="22"></a>
                </ItemTemplate>
                <HeaderStyle Width="5%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PDF">
                <ItemTemplate>
                    <a href="javascript://" onclick="showModalPopUp2('../recepcion/PopUpFE.aspx?CA=<%# Eval("claveAcceso") %>&tipo=2')">
                        <%--<a href='../recepcion/PopUpFE.aspx?CA=<%# Eval("claveAcceso") %>'>--%>
                        <img src="../imagenes/pdf32x32.png" alt="pdf" border="0" align="middle" height="22"
                            width="22"></a>
                </ItemTemplate>
                <HeaderStyle Width="5%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    &nbsp;
                    <asp:ImageButton ID="ibAceptar" runat="server" CommandName="Aceptar" Height="35px"
                        ImageUrl="~/imagenes/Check-128.png" Width="35px" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" />
                    <asp:ConfirmButtonExtender ID="ibAceptar_ConfirmButtonExtender" runat="server" ConfirmText="Estas seguro que los datos de la factura son &quot;Correctos&quot;"
                        Enabled="True" TargetControlID="ibAceptar">
                    </asp:ConfirmButtonExtender>
                    &nbsp; <a href="javascript://" onclick="showModalPopUp('../recepcion/enviaMail.aspx?CA=<%# Eval("claveAcceso") %>')">
                        <img src="../imagenes/mail.jpg" border="0" height="35" width="35"></a> &nbsp;
                    <asp:ImageButton ID="ibRechazar" runat="server" CommandName="Rechazar" Height="35px"
                        ImageUrl="~/imagenes/Delete-128.png" Width="35px" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" />
                    <asp:ConfirmButtonExtender ID="ibRechazar_ConfirmButtonExtender" runat="server" ConfirmText="Estas seguro que deseas rechazar esta factura, se eliminará de los registros."
                        Enabled="True" TargetControlID="ibRechazar">
                    </asp:ConfirmButtonExtender>
                </ItemTemplate>
                <HeaderStyle Width="150px" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
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
        SelectCommand="SP_Recepcion" SelectCommandType="StoredProcedure" DeleteCommand="SP_Recepcion"
        DeleteCommandType="StoredProcedure" UpdateCommand="SP_Recepcion" UpdateCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter DefaultValue="<INSTRUCCION><FILTRO><opcion>2</opcion><query>-</query></FILTRO></INSTRUCCION>"
                Name="documentoXML" Type="String" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="<INSTRUCCION><FILTRO><opcion>1</opcion><query>ETR</query></FILTRO></INSTRUCCION>"
                Name="documentoXML" Type="String" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter DefaultValue="<INSTRUCCION><FILTRO><opcion>2</opcion><query>-</query></FILTRO></INSTRUCCION>"
                Name="documentoXML" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WHERE (tipo = 'Comprobante')">
    </asp:SqlDataSource>
    <br />
</asp:Content>
