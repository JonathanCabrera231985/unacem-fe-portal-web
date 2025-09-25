<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearRetencion.aspx.cs" Inherits="DataExpressWeb.crearRetencion" %>

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
        .style3
        {
            text-align: center;
            font-size: large;
        }
        .style4
        {
            font-size: large;
        }
        .style5
        {
            text-align: center;
            font-weight: bold;
            font-size: xx-small;
        }
        
        .style13
        {
            width: 100%;
        }
        .style15
        {
            height: 15px;
            width: 182px;
        }
        .style16
        {
            width: 182px;
        }
        .style17
        {
            height: 15px;
            width: 146px;
        }
        .style18
        {
            width: 146px;
        }
        .style21
        {
            text-align: center;
            font-size: x-small;
        }
        .CompletionListCssClass
        {
            font-size: 16px;
            color: #000;
            padding: 0px 0px;
            border: 1px solid #999;
            background: #fff;
            width: 300px;
            float: left;
            z-index: 1;
            position: absolute;
            margin-left: 0px;
            margin-top: -3px;
        }
        .style23
        {
            text-align: right;
        }
        .style24
        {
        }
        .style26
        {
            width: 173px;
            font-size: xx-small;
        }
        .style27
        {
            font-size: medium;
        }
        .style28
        {
            width: 128px;
        }
        .style10
        {
            font-size: x-small;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" BackColor="#F7F6F3" BorderColor="#CCCCCC"
        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
        Height="183px" Width="89%" Style="margin-bottom: 0px" OnActiveStepChanged="Wizard1_ActiveStepChanged"
        OnNextButtonClick="StepNextButton_Click">
        <FinishNavigationTemplate>
            <asp:Button ID="FinishPreviousButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious"
                Font-Names="Verdana" Font-Size="Small" ForeColor="#284775" Text="Anterior" />
            <asp:Button ID="FinishButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CommandName="MoveComplete" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#284775" Text="Crear Comprobante" OnClick="FinishButton_Click" />
        </FinishNavigationTemplate>
        <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" Font-Size="0.9em"
            ForeColor="White" HorizontalAlign="Left" />
        <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" ForeColor="#284775" />
        <NavigationStyle HorizontalAlign="Center" />
        <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
        <SideBarStyle BackColor="#5F6062" BorderWidth="0px" Font-Size="Small" VerticalAlign="Top"
            Width="20%" BorderColor="#3333CC" />
        <StepNavigationTemplate>
            <asp:Button ID="StepPreviousButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious"
                Font-Names="Verdana" Font-Size="Small" ForeColor="#284775" Text="Anterior" />
            <asp:Button ID="StepNextButton" runat="server" BackColor="#FFFBFF" BorderColor="#CCCCCC"
                BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana"
                Font-Size="Small" ForeColor="#284775" Text="Siguiente" ValidationGroup="Form" />
        </StepNavigationTemplate>
        <StepStyle BorderWidth="0px" ForeColor="#666666" />
        <WizardSteps>
            <asp:WizardStep runat="server" Title="Información Tributaria" ID="infoTributaria">
                <div class="style3">
                    <strong>Información Tributaria<br />
                    </strong>
                </div>
                <br />
                <table class="style13">
                    <tr>
                        <td class="style15">
                            AMBIENTE:<br />
                            <asp:DropDownList ID="ddlAmbiente" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataAmbiente"
                                DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                        <td class="style17">
                            EMISIÓN:<br />
                            <asp:DropDownList ID="ddlEmision" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataEmision"
                                DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                        <td>
                            COMPROBANTE:<br />
                            <asp:DropDownList ID="ddlComprobante" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataDocumento" DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style16">
                            SUCURSAL:<br />
                            <asp:DropDownList ID="ddlSucursal" runat="server" DataSourceID="SqlDataSucursal"
                                DataTextField="sucursal" DataValueField="clave">
                            </asp:DropDownList>
                        </td>
                        <td class="style18">
                            PUNTO DE EMISIÓN:<br />
                            <asp:DropDownList ID="ddlPtoEmi" runat="server" DataSourceID="SqlDataPtoEmision"
                                DataTextField="noEmpleado" DataValueField="noEmpleado">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lSecuencial" runat="server" Text="SECUENCIA:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbSecuencial" runat="server" Height="19px" Width="93px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lRuc" runat="server" Text="RUC:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRuc" runat="server" Height="19px" MaxLength="13" Width="163px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRuc"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lRazonSocial" runat="server" Text="RAZÓN SOCIAL:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocial" runat="server" Height="19px" Width="353px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbRazonSocial"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lNombreComercial" runat="server" Text="NOMBRE COMERCIAL"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbNombreComercial" runat="server" Height="19px" Width="293px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tbNombreComercial"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lDirMatriz" runat="server" Text="DIRECCIÓN MATRIZ"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirMatriz" runat="server" Height="19px" Width="293px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tbDirMatriz"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataPtoEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT noEmpleado, idEmpleado FROM Empleados WITH (NOLOCK)  WHERE (idEmpleado = @idEmpleado)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idEmpleado" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSucursal" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT clave, sucursal FROM Sucursales WITH (NOLOCK)  WHERE (idSucursal = @idSucursal)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idSucursal" SessionField="sucursalUser" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDocumento" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Comprobante') AND (codigo = '07')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Emision')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Ambiente')">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Retenciones" ID="infoDetalles">
                <strong><span class="style27">IMPUESTOS&nbsp;A RETENER</span>
                    <br />
                    <br />
                    <table width="60%">
                        <tr valign="top">
                            <td class="style28">
                                <strong>COMPROBANTE A MODIFICAR:</strong><br />
                                <asp:DropDownList ID="ddlcodDocSustento" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="SqlDataComprobanteMod" DataTextField="descripcion" DataValueField="codigo">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lGuiaRemision" runat="server" Style="font-weight: 700; font-size: x-small;"
                                    Text="NO. DE DOCUMENTO:"></asp:Label>
                                <asp:TextBox ID="tbNumDocModificado" runat="server" Height="19px" MaxLength="17"
                                    ValidationGroup="Imp" Width="126px" CssClass="txt_gr2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="tbNumDocModificado"
                                    ErrorMessage="*" Style="color: #FF0000; font-size: xx-small;" ValidationGroup="Imp"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                    ControlToValidate="tbNumDocModificado" ErrorMessage="*" Style="color: #FF0000;
                                    font-size: xx-small;" ValidationExpression="^([0-9]{3,3}-[0-9]{3,3}-[0-9]{9,9})?$"
                                    ValidationGroup="Imp"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="lGuiaRemision2" runat="server" Style="font-weight: 700; font-size: x-small;"
                                    Text="FECHA DEL DOCUMENTO:"></asp:Label>
                                <asp:TextBox ID="tbFechaEmisionDocSustento" runat="server" Height="19px" MaxLength="17"
                                    ValidationGroup="Imp" Width="90px" CssClass="txt_gr2"></asp:TextBox>
                                <asp:CalendarExtender ID="tbFechaEmisionDocSustento_CalendarExtender" runat="server"
                                    Enabled="True" Format="dd/MM/yyyy" TargetControlID="tbFechaEmisionDocSustento"
                                    TodaysDateFormat="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                    ControlToValidate="tbFechaEmisionDocSustento" CssClass="style26" ErrorMessage="*"
                                    Style="color: #FF0000" ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"
                                    ValidationGroup="Imp"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="70%">
                        <tr>
                            <td>
                                <b>BASE IMPONIBLE</b>
                            </td>
                            <td>
                                <b>TIPO</b>
                            </td>
                            <td>
                                <b style="text-align: center">%&nbsp; A RETENER<asp:TextBox ID="tbTarifa" runat="server"
                                    AutoPostBack="True" Height="17px" MaxLength="4" OnTextChanged="tbTarifa_TextChanged"
                                    ValidationGroup="Imp" Visible="False" Width="31px" CssClass="txt_gr2"></asp:TextBox>
                                </b>
                            </td>
                            <td>
                                <b>VALOR<br />
                                    RETENIDO</b>
                            </td>
                            <td>
                                <asp:TextBox ID="tbCodigoID" runat="server" Height="19px" MaxLength="1" ReadOnly="True"
                                    ValidationGroup="Imp" Visible="False" Width="20px" CssClass="txt_gr2"></asp:TextBox>
                                <asp:TextBox ID="tbCodigoIDP" runat="server" Height="16px" MaxLength="4" ValidationGroup="Imp"
                                    Visible="False" Width="16px" CssClass="txt_gr2"></asp:TextBox>
                            </td>
                        </tr>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:TextBox ID="tbBaseImponible" runat="server" AutoPostBack="True" Height="19px"
                                    MaxLength="14" OnTextChanged="tbBaseImponible_TextChanged" Style="margin-bottom: 0px"
                                    ValidationGroup="Deta" Width="70px" CssClass="txt_gr2">0.00</asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="tbBaseImponible_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbBaseImponible"
                                    ValidChars=",.">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbBaseImponible"
                                    CssClass="style20" ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                    ValidationGroup="Deta" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbBaseImponible"
                                    ErrorMessage="Debe ser mayor a 0." ForeColor="Red" ValidationGroup="Deta" InitialValue="0"
                                    SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator2" PopupPosition="Right">
                                </asp:ValidatorCalloutExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbBaseImponible"
                                    ErrorMessage="Debe ingresar valor." ForeColor="Red" ValidationGroup="Deta" InitialValue=""
                                    SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator3" PopupPosition="Right">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoImpuesto" runat="server" AppendDataBoundItems="True"
                                    AutoPostBack="True" DataSourceID="SqlDataCatImpuestos" DataTextField="descripcion"
                                    DataValueField="codigo" OnSelectedIndexChanged="ddlTipoImpuesto_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Impuesto a Retener</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTasa" runat="server" AutoPostBack="True" DataSourceID="SqlDataPorcentajeRetencion"
                                    DataTextField="descripcion" DataValueField="id" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Selecciona el Impuesto</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="tbTarifa"
                                    ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp" Visible="False"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="tbValor" runat="server" Height="19px" MaxLength="14" ReadOnly="True"
                                    ValidationGroup="Imp" Width="70px" CssClass="txt_gr2"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <tr valign="top">
                                <td class="style21" colspan="2">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <br />
                                    <asp:Button ID="bAgregarImpuesto" runat="server" Height="26px" OnClick="bAgregarImpuesto_Click"
                                        Text="Agregar Retención" ValidationGroup="Deta" Width="132px" />
                                </td>
                            </tr>
                        </tr>
                    </table>
                    <asp:Label ID="lMsjRetencion" runat="server" Style="color: #FF0000"></asp:Label>
                    <br />
                    <asp:GridView ID="gvTotalImpuestos" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="idTotalConImpuestosTemp"
                        DataSourceID="SqlDataImpuestosConceptos" ForeColor="Black" GridLines="Vertical"
                        Height="18px" Width="85%">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="codigo" HeaderText="COD." SortExpression="codigo" />
                            <asp:BoundField DataField="codigoRetencion" HeaderText="COD. RET" SortExpression="codigoRetencion" />
                            <asp:BoundField DataField="baseImponible" HeaderText="BASE IMPONIBLE" SortExpression="baseImponible" />
                            <asp:BoundField DataField="descripcionImpuesto" HeaderText="IMPUESTO" SortExpression="descripcionImpuesto" />
                            <asp:BoundField DataField="porcentajeRetener" HeaderText="RETENCIÓN(%)" SortExpression="porcentajeRetener" />
                            <asp:BoundField DataField="valorRetenido" HeaderText="VALOR" SortExpression="valorRetenido" />
                            <asp:BoundField DataField="codDocSustento" HeaderText="COD. DOC. " SortExpression="codDocSustento" />
                            <asp:BoundField DataField="numDocSustento" HeaderText="NO. DOC." SortExpression="numDocSustento" />
                            <asp:BoundField DataField="fechaEmisionDocSustento" HeaderText="FECHA EMISIÓN" SortExpression="fechaEmisionDocSustento" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        OnClientClick="return confirm('¿Desea eliminar el registro?');" Text="Eliminar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#DBE8F0" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#DEE2EB" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                        <SortedAscendingHeaderStyle BackColor="#848384" />
                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                        <SortedDescendingHeaderStyle BackColor="#575357" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataImpuestosConceptos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        DeleteCommand="DELETE FROM TotalConImpuestosTemp WHERE idTotalConImpuestosTemp = @idTotalConImpuestosTemp"
                        SelectCommand="SELECT t.idTotalConImpuestosTemp, t.codigo, t.codigoRetencion, t.baseImponible, t.porcentajeRetener, t.valorRetenido, t.codDocSustento, t.numDocSustento, t.fechaEmisionDocSustento, t.id_Empleado,
ISNULL((SELECT descripcion FROM CatImpuestos_C WITH (NOLOCK)  WHERE (tipo = 'Retencion') AND (codigo = t.codigo)), '') AS descripcionImpuesto  
FROM TotalConImpuestosTemp t  WITH (NOLOCK) WHERE (t.id_Empleado = @idUser)">
                        <DeleteParameters>
                            <asp:Parameter Name="idTotalConImpuestosTemp" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:SessionParameter Name="idUser" SessionField="idUser" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataCatImpuestos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        SelectCommand="SELECT [descripcion], [codigo] FROM [CatImpuestos_C] WITH (NOLOCK)  WHERE ([tipo] = @tipo) and descripcion&lt;&gt;'ISD'">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="Retencion" Name="tipo" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataPorcentajeRetencion" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        SelectCommand="SELECT id,descripcion, valor, codigo, tipo FROM CatImpuestos_C WITH (NOLOCK)  WHERE (tipo = @tipo)">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="-" Name="tipo" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="SqlDataComprobanteMod" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (codigo = '01') AND (tipo = 'Comprobante')">
                    </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Documento" ID="infoDocumentos">
                <div class="style5">
                    <span class="style4">Información Retenciones<br />
                    </span>
                </div>
                <table class="style13">
                    <tr>
                        <td class="style24">
                            PERIODO FISCAL<br />
                            <asp:DropDownList ID="ddlPeriodoFiscal" runat="server" Width="121px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Label ID="lFechaEmision" runat="server" Text="FECHA DE EMISIÓN:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbFechaEmision" runat="server" Height="19px" Width="185px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:Label ID="ldirEstablecimiento" runat="server" Text="ESTABLECIMIENTO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirEstablecimiento" runat="server" Height="47px" Width="293px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:CheckBox ID="cbContribuyenteEspecial" runat="server" Text="CONTRIBUYENTE ESPECIAL" />
                            &nbsp;<asp:TextBox ID="tbContribuyenteEspecial" runat="server" MaxLength="4" Width="51px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbContribuyenteEspecial_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbContribuyenteEspecial">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <asp:CheckBox ID="cbObligado" runat="server" Text="OBLIGADO A CONTABILIDAD" Checked="True" />
                        </td>
                        <td rowspan="2" class="style23">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                            <asp:Label ID="lidentificacionComprador" runat="server" Text="IDENTIFICACIÓN DEL SUJETO RETENIDO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbIdentificacionComprador" runat="server" Height="19px" MaxLength="13"
                                Width="183px" AutoPostBack="True" OnTextChanged="tbIdentificacionComprador_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbIdentificacionComprador_FilteredTextBoxExtender"
                                runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbIdentificacionComprador">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="tbIdentificacionComprador"
                                CssClass="style28" ErrorMessage="Ingresa el RUC" Style="color: #FF0000"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                ControlToValidate="tbIdentificacionComprador" CssClass="style28" ErrorMessage="RUC Invalido"
                                ValidationExpression="^[0-9a-zA-Z]{0,13}?$" Style="color: #FF0000"></asp:RegularExpressionValidator>
                            <asp:AutoCompleteExtender ID="tbIC_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getRuc" ServicePath="../autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True"
                                TargetControlID="tbIdentificacionComprador" UseContextKey="True">
                            </asp:AutoCompleteExtender>
                            
                                <Services>
                                    <asp:ServiceReference Path="../autoRuc.asmx" />
                                </Services>
                            
                            <br />
                            <asp:Label ID="lTipoIdentificacionComprador" runat="server" Text="IDENTIFICACION"></asp:Label>
                            :<br />&nbsp;&nbsp;<asp:DropDownList ID="ddlTipoIdentificacion" runat="server" DataSourceID="SqlDataTipoIdentificacion"
                                DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True">
                                <asp:ListItem>Selecciona el Tipo</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label ID="lRazonSocialComprador" runat="server" Text="RAZÓN SOCIAL DEL SUJETO RETENIDO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocialComprador" runat="server" Height="46px" Width="347px"
                                TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbRazonSocialComprador"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lbl_dir_cli" runat="server" Text="DOMICILIO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txt_dir_cli" runat="server" Height="46px" TextMode="MultiLine" Width="291px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            Email:
                            <asp:TextBox ID="tbEmail" runat="server" Height="19px" Width="413px" ValidationGroup="email"></asp:TextBox>
                            <br />
                            <span class="style10">Formato: <a href="mailto:email1@email.com,email2@email.com">email1@email.com,email2@email.com</a>
                            </span>
                            <br />
                            <span class="style10">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbEmail"
                                    ErrorMessage="El formato de Email no es válido" ForeColor="Red" ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$"
                                    ValidationGroup="email"></asp:RegularExpressionValidator>
                            </span>
                            <br />
                            <asp:Label ID="lMsjDocumento" runat="server" Style="color: #FF0000"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataTipoIdentificacion" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Identificacion')">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <%--<asp:WizardStep runat="server" Title="Informacion Adicional" ID="infoAdicional">
            <table style="width:100%;">
                <tr>
                    <td class="style6" colspan="2">
                        <strong>Agregar Campo Adicional.</strong></td>
                </tr>
                <tr>
                    <td class="style8">
                        NOMBRE:</td>
                    <td>
                        <b>VALOR:</b></td>
                </tr>
                <tr valign="top">
                    <td >
                        <asp:TextBox ID="tbNombreCA" runat="server" Height="19px" Width="334px" 
                            ValidationGroup="InfoAdic"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                            ValidationGroup="InfoAdic" ControlToValidate="tbNombreCA" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="tbValorCA" runat="server" Height="19px" Width="334px" 
                            ValidationGroup="InfoAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                             ValidationGroup="InfoAdic" ControlToValidate="tbValorCA" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <asp:Button ID="bAgregarCA" runat="server" Text="Agregar Campo" Width="125px" 
                ValidationGroup="InfoAdic" OnClick="bAgregarCA_Click" />
            <br />
            <br />
            <asp:GridView ID="gvInfoAdic" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                CellPadding="4" DataSourceID="SqlDataInfoAdic" ForeColor="Black" DataKeyNames="idInfoAdicionalTemp" 
                GridLines="Vertical" Width="90%">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="nombre" HeaderText="NOMBRE" 
                        SortExpression="nombre" />
                    <asp:BoundField DataField="valor" HeaderText="VALOR" SortExpression="valor" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');" CausesValidation="False" 
                                CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="10%" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCC99" />
                <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <RowStyle BackColor="#DEE2EB" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataInfoAdic" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                DeleteCommand="DELETE FROM InfoAdicionalTemp WHERE (idInfoAdicionalTemp = @idInfoAdicionalTemp)" 
                SelectCommand="SELECT idInfoAdicionalTemp,nombre, valor, id_Empleado FROM InfoAdicionalTemp WITH (NOLOCK)  WHERE (id_Empleado = @id_Empleado)">
                <DeleteParameters>
                    <asp:Parameter Name="idInfoAdicionalTemp" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:SessionParameter Name="id_Empleado" SessionField="idUser" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:WizardStep>--%>
        </WizardSteps>
    </asp:Wizard>
    <br />
</asp:Content>
