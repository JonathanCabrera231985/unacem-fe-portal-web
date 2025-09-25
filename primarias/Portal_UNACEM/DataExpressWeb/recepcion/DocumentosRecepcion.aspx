<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DocumentosRecepcion.aspx.cs" Inherits="DataExpressWeb.DocumentosRecepcion" %>

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
        .style10
        {
            font-size: x-small;
        }
        .style12
        {
            height: 17px;
        }
        .style13
        {
            font-size: xx-small;
        }
        .style14
        {
            width: 272px;
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
        <strong style="color: #006600">COMPROBANTES RECIBIDOS</strong></center>
    <br />
    <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
        vertical-align: top;">
        <tr>
            <td colspan="4" bgcolor="#5F6062" style="font-weight: 700; font-size: medium; text-align: center">
                <span class="style16">Buscador</span>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Tipo de documento:</strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc"
                    DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                    AutoPostBack="True">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Número de documento:</strong><br />
            </td>
            <td>
                <asp:TextBox ID="tbFolioAnterior" runat="server"></asp:TextBox>
                &nbsp;<span class="style13">Formato(1,2,3,4-8)</span><br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato Incorrecto"
                    ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
            </td>
            <td>
                <strong>Fecha Inicial:</strong>
            </td>
            <td>
                <asp:TextBox ID="tbFechaInicial" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaInicial_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaInicial" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td>
                <span class="style9"><strong>RUC:</strong><br />
                </span>
            </td>
            <td>
                <asp:TextBox ID="tbRFC" runat="server" CssClass="style9" MaxLength="13"></asp:TextBox>
            </td>
            <td>
                <strong>Fecha Final:</strong>
            </td>
            <td>
                <asp:TextBox ID="tbFechaFinal" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaFinal_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaFinal" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td>
                <span class="style9"><strong>Razón Social:</strong><br />
                </span>
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbNombre" runat="server" Width="400px" CssClass="style9"></asp:TextBox>
            </td>
            <td>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/recepcion/logError.aspx">Ver Historial</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="bBuscar" runat="server" OnClick="Button1_Click" Text="Buscar" Width="87px"
                    CssClass="botones" />
                
            </td>
            <td colspan="3">
                <asp:Button ID="bActualizar" runat="server" OnClick="Button1_Click1" Text="Actualizar"
                    CssClass="botones" />
            </td>
        </tr>
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: medium; text-align: center">
                <span class="style16">Envios de Correos</span>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: center">
                <strong>Seleccionar archivo:</strong><br />
                <%--&nbsp;<asp:CheckBox ID="checkPDF" runat="server" Checked="True" Text="PDF" />--%>
                &nbsp;<asp:CheckBox ID="checkXML" runat="server" Checked="True" Text="XML" />
                <%  if (Convert.ToBoolean(Session["coFactTodas"]))
                    {  %>
                <!--   <asp:Label ID="lSucursal" runat="server" Text="Clave sucursal:"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlSucursal" runat="server" 
                    DataSourceID="SqlDataSourceSucursales" DataTextField="Sucursal" 
                    DataValueField="idSucursal">
                    <asp:ListItem>-----</asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceSucursales" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:upsdataConnectionString %>" 
                    
                    SelectCommand="SELECT idSucursal, sucursal + ':' + clave AS Sucursal FROM Sucursales ORDER BY clave ASC">
                </asp:SqlDataSource>
                 <%  }  %> -->
            </td>
            <td rowspan="3" colspan="2">
                <center>
                    <asp:CheckBox ID="chkReglas" runat="server" Text="Reglas" />
                    <br />
                    &nbsp;<asp:Button ID="bMail" runat="server" OnClick="Button1_Click2" Text="Enviar E-mail"
                        Width="106px" Style="height: 26px" ValidationGroup="email" CssClass="botones" />
                    <br />
                    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    _______________________________________________________________________<br />
                    <br />
                    <asp:Button ID="btnZip" runat="server" OnClick="btnZip_Click" Text="Zip" CssClass="botones" />
                    <br />
                    <asp:Label ID="lbMsgZip" runat="server" ForeColor="Red"></asp:Label>
                </center>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <strong>E-mails</strong><br />
                <asp:TextBox ID="tbEmail" runat="server" Height="49px" Width="348px" 
                    ValidationGroup="email" CssClass="txt_gr2"></asp:TextBox>
                <span class="style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEmail"
                        ErrorMessage="*" ForeColor="Red" ValidationGroup="email"></asp:RequiredFieldValidator>
                </span><br>
                <span class="style10">Formato: <a href="mailto:email1@email.com,email2@email.com">email1@email.com,email2@email.com</a>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato de Email no es válido"
                        ForeColor="Red" ControlToValidate="tbEmail" ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                        ValidationGroup="email"></asp:RegularExpressionValidator>
                </span>
            </td>
        </tr>
        <tr>
            <td class="style14">
                &nbsp;
            </td>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
<%--        <tr>
            <td align="right">
                <asp:Button ID="bBuscar" runat="server" OnClick="Button1_Click" Text="Buscar" Width="87px" />
                &nbsp;&nbsp;
            </td>
            <td class="style2" colspan="4">
                <asp:Button ID="bActualizar" runat="server" OnClick="Button1_Click1" Text="Actualizar" />
                <br />
            </td>
        </tr>--%>
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: xx-small; text-align: center">
                &nbsp;
            </td>
        </tr>
    </table>
    .<asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical" Width="90%"
        BackColor="White" BorderColor="#DEDFDE" BorderWidth="1px" BorderStyle="None"
        Style="margin-top: 0px" AllowPaging="True" AllowSorting="True">
        <AlternatingRowStyle BackColor="White" />
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
            <asp:TemplateField HeaderText="codDoc" SortExpression="codDoc" Visible = "false">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("codDoc") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label10" runat="server" Text='<%# Bind("codDoc") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="10%" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:ImageField DataImageUrlField="estado" DataImageUrlFormatString="~/Imagenes/estado{0}.jpg"
                HeaderText="Estado" AlternateText="amarillo: Recibido">
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
            <asp:TemplateField HeaderText="Marcar">
                <ItemTemplate>
                    <asp:CheckBox ID="check" runat="server" />
                    <asp:HiddenField ID="checkHdPDF" runat="server" Value='<%#Eval("claveAcceso")%>' />
                    <asp:HiddenField ID="checkHdXML" runat="server" Value='<%#Eval("claveAcceso")%>' />
                </ItemTemplate>
                <HeaderStyle Width="5%" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No existen datos.
        </EmptyDataTemplate>
        <%--<FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#307C34" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <RowStyle BackColor="White" />
        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
        <SortedAscendingCellStyle BackColor="#FBFBF2"></SortedAscendingCellStyle>
        <SortedAscendingHeaderStyle BackColor="#848384"></SortedAscendingHeaderStyle>
        <SortedDescendingCellStyle BackColor="#EAEAD3"></SortedDescendingCellStyle>
        <SortedDescendingHeaderStyle BackColor="#575357"></SortedDescendingHeaderStyle>--%>

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
            <asp:Parameter DefaultValue="<INSTRUCCION><FILTRO><opcion>1</opcion><query>-</query></FILTRO></INSTRUCCION>"
                Name="documentoXML" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Comprobante')">
    </asp:SqlDataSource>
    <br />
</asp:Content>
