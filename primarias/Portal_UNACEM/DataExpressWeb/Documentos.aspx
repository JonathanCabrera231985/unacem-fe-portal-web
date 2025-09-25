<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Documentos.aspx.cs" Inherits="DataExpressWeb.Documentos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="DataFilter1.ascx" TagName="DataFilter1" TagPrefix="uc2" %>
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
        .style19
        {
            font-size: 9pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript">
       var popUpObj;
       function showModalPopUp(pagina) {
           popUpObj = window.open(pagina,
    "ModalPopUp",
    "toolbar=yes," +
    "scrollbars=yes," +
    "location=no," +
    "statusbar=no," +
    "menubar=no," +
    "resizable=0," +
    "width=800," +
    "height=500," +
    "left = 340," +
    "top=70"
    );
           popUpObj.focus();
           LoadModalDiv();
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

       function hideMessageAfterTimeout() {
           setTimeout(function () {
               document.getElementById('<%= LblSendSapInfo.ClientID %>').innerText = '';
        }, 15000); // 10000 milisegundos = 10 segundos
       }

   </script>
    <div id="divBackground" style="position: absolute; top: 0px; left: 0px; background-color: black;
        z-index: 100; opacity: 0.8; filter: alpha(opacity=60); -moz-opacity: 0.8; overflow: hidden;
        display: none">
    </div>
    <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
        vertical-align: top;">
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: medium; text-align: center">
                <span class="style16">Buscador</span>
            </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">
                <span class="style9"><strong><span class="style19">RUC:</span></strong> </span>
                <asp:TextBox ID="tbRFC" runat="server" CssClass="txt_gr2" MaxLength="13"></asp:TextBox>
            </td>
            <td class="style1" colspan="3">
                <span class="style9"><strong><span class="style19">Razón Social:</span></strong></span>
                <asp:TextBox ID="tbNombre" runat="server" Width="322px" CssClass="txt_gr2"></asp:TextBox>
                <br class="style9" />
            </td>
        </tr>
        <tr valign="top">
            <td class="style7" rowspan="2">
                <strong>No.Documento(s):</strong><br />
                <asp:TextBox ID="tbFolioAnterior" runat="server" Width="151px" 
                    CssClass="txt_gr2"></asp:TextBox>
                &nbsp;<span class="style13">Formato(1,2,3,4-8)</span><br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato Incorrecto"
                    ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
            </td>
            <td rowspan="2" colspan="2">
                <strong>Tipo:</strong><br />
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc"
                    DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                    AutoPostBack="True">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                Estado de documentos:<br />
                <asp:DropDownList ID="ddlAut" runat="server">
                    <asp:ListItem Value="0">Todos</asp:ListItem>
                    <asp:ListItem Value="1">Autorizados</asp:ListItem>
                    <asp:ListItem Value="2">Pendientes</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
            </td>
            <td>
                <strong>Fecha Inicial:</strong><br />
                <asp:TextBox ID="tbFechaInicial" runat="server" CssClass="txt_gr2"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaInicial_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaInicial" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
            <td class="style17">
                <strong>Fecha Final:<br />
                </strong>
                <asp:TextBox ID="tbFechaFinal" runat="server" CssClass="txt_gr2"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaFinal_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaFinal" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="2" align="center">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/configuracion/log/logError.aspx">Ver Historial Técnico</asp:HyperLink>          
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;           
                 <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/configuracion/log/log_usuario.aspx">Ver Historial de usuario</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Buscar" Width="87px"
                    Style="height: 26px" CssClass="botones" />
                &nbsp;&nbsp;
            </td>
            <td class="style2" colspan="2">
                <asp:Button ID="Button2" runat="server" OnClick="Button1_Click1" Text="Actualizar"
                    Width="87px" Style="height: 26px" CssClass="botones" />
                <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                <br />
            </td>
            <td class="style2">
                <asp:Button ID="Button3" runat="server" OnClick="SendSap" Text="Enviar SAP"
                    Width="87px" Style="height: 26px" CssClass="botones" />
                <asp:Label ID="LblErrors" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                <asp:Label ID="LblSendSapInfo" runat="server" ForeColor="Green"></asp:Label>
                <br />
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
                &nbsp;<asp:CheckBox ID="checkPDF" runat="server" Checked="True" Text="PDF" />
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
                        Width="106px" Style="height: 26px" ValidationGroup="email" />
                    <br />
                    <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    _______________________________________________________________________<br />
                    <br />
                    <asp:Button ID="btnZip" runat="server" OnClick="btnZip_Click" Text="Zip" />
                    <br />
                    <asp:Label ID="lbMsgZip" runat="server" ForeColor="Red"></asp:Label>
                </center>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <strong>E-mails</strong><br />
                <asp:TextBox ID="tbEmail" runat="server" Height="49px" Width="348px" 
                    ValidationGroup="email" CssClass="txt_gr2"></asp:TextBox><br>
                <span class="style10">Formato: <a href="mailto:email1@email.com,email2@email.com">email1@email.com,email2@email.com</a>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEmail"
                        ErrorMessage="Coloca un Email" ForeColor="Red" ValidationGroup="email"></asp:RequiredFieldValidator>
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
        <%--<tr>
            <td align="right">
                <asp:Button ID="bBuscar" runat="server" OnClick="Button1_Click" Text="Buscar" Width="87px"
                    Style="height: 26px" />
                &nbsp;&nbsp;
            </td>
            <td class="style2" colspan="4">
                <asp:Button ID="bActualizar" runat="server" OnClick="Button1_Click1" Text="Actualizar"
                    Width="87px" Style="height: 26px" />
                <asp:Label ID="lMensaje" runat="server" ForeColor="Red"></asp:Label>
                <br />
            </td>
        </tr>--%>
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: xx-small; text-align: center">
                &nbsp;
            </td>
        </tr>
    </table>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:DataFilter1 ID="DataFilter11" runat="server" />
            <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CellPadding="4"
                DataSourceID="SqlDataSource1" GridLines="Horizontal" Width="80%" BackColor="White"
                BorderColor="#DEDFDE" BorderWidth="3px" BorderStyle="Double" Style="margin-top: 0px"
                AllowPaging="True" AllowSorting="True" DataKeyNames="folfac" OnPageIndexChanged="gvFacturas_PageIndexChanged"
                OnSelectedIndexChanged="gvFacturas_SelectedIndexChanged" 
                OnSorting="gvFacturas_SelectedIndexChanged" PageSize="15">
                <Columns>
                    <asp:TemplateField HeaderText="RUC-CI" SortExpression="RFCREC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("RFCREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CLIENTE" SortExpression="NOMREC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("NOMREC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("NOMREC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DOCUMENTO" SortExpression="TIPODOC">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("TIPODOC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FOLFAC" HeaderText="NÚMERO" InsertVisible="False" ReadOnly="True"
                        SortExpression="FOLFAC" HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <%--<asp:TemplateField HeaderText="SECUENCIA" SortExpression="FOLFAC">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("FOLFAC") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="10%" />
            </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="TOTAL" SortExpression="TOTAL" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("FOLFAC") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="FECHA AUTORIZACIÓN" SortExpression="FECHA" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("FECHA") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FECHA") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="codDoc" SortExpression="codDoc" ItemStyle-HorizontalAlign="Right" Visible ="false">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("codDoc") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("codDoc") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:ImageField DataImageUrlField="EDOFAC" DataImageUrlFormatString="~/Imagenes/estado{0}.jpg"
                        HeaderText="ESTADO">
                        <ControlStyle Height="20px" Width="20px" />
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:ImageField>
                    <asp:TemplateField HeaderText="XML" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href='download.aspx?file=<%# Encrypt(Eval("XMLARC").ToString()) %>'>
                                <img src="imagenes/xml32x32.png" alt="xml" border="0" align="middle" height="22"
                                    width="22"></a>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PDF" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href='download.aspx?file=<%# Encrypt(Eval("PDFARC").ToString()) %>'>
                                <img src="imagenes/pdf32x32.png" alt="pdf" border="0" align="middle" height="22"
                                    width="22"></a>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HISTORIAL" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HiddenField ID="IdComprobante" runat="server" Value='<%#Eval("ID")%>' />
                            <a href="javascript://" onclick="showModalPopUp('../PopLogError.aspx?ID=<%# Eval("ID") %>')">
                                <img src="../imagenes/web_search.png" alt="Consulta" border="0" align="middle" height="22"
                                width="22">
                             </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MARCAR">
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server" 
                                oncheckedchanged="check_CheckedChanged" />
                            <asp:HiddenField ID="checkHdPDF" runat="server" Value='<%#Eval("PDFARC")%>' />
                            <asp:HiddenField ID="checkHdXML" runat="server" Value='<%#Eval("XMLARC")%>' />
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No existen datos.
                </EmptyDataTemplate>
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign= "Center" Font-Underline="True"  BorderColor="#003300" />
        <RowStyle BackColor="#DEE2EB" />
        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="PA_facturas_basico_2" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
            <asp:SessionParameter DefaultValue="S--X" Name="SUCURSAL" SessionField="sucursalUser"
                Type="String" />
            <asp:SessionParameter DefaultValue="R---" Name="RFC" SessionField="rfcCliente" Type="String" />
            <asp:SessionParameter DefaultValue="false" Name="ROL" SessionField="coFactTodas"
                Type="Boolean" />
            <asp:SessionParameter DefaultValue="0" Name="IDEMISOR" SessionField="RucEmpresa" Type="Int32" />
            <asp:SessionParameter DefaultValue="" Name="USUARIO" SessionField="idUser" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Comprobante')">
    </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

    <br />
</asp:Content>
