<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearNotaDebito.aspx.cs" Inherits="DataExpressWeb.crearNotaDebito" %>

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
        .style6
        {
            height: 6px;
            text-align: center;
            font-size: medium;
        }
        .style9
        {
            width: 77%;
            height: 37px;
        }
        .style10
        {
            font-size: small;
        }
        
        .style11
        {
            font-size: small;
            text-decoration: underline;
        }
        .style12
        {
            text-decoration: underline;
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
            width: 86px;
        }
        .style21
        {
            text-align: center;
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
            width: 287px;
        }
        .style27
        {
            width: 231px;
        }
        .style30
        {
        }
        .style32
        {
            width: 349px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="1" BackColor="#F7F6F3" BorderColor="#CCCCCC"
        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em"
        Height="183px" Width="95%" Style="margin-bottom: 0px" OnActiveStepChanged="Wizard1_ActiveStepChanged"
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
                    Información Tributaria<br />
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
                            <asp:TextBox ID="tbRuc" runat="server" Height="19px" MaxLength="13" Width="163px"
                                ReadOnly="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbRuc"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lRazonSocial" runat="server" Text="RAZÓN SOCIAL:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocial" runat="server" Height="19px" Width="353px" ReadOnly="True"></asp:TextBox>
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
                    SelectCommand="SELECT noEmpleado, idEmpleado FROM Empleados WITH (NOLOCK) WHERE (idEmpleado = @idEmpleado)">
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
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Comprobante') AND (codigo = '05')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Emision')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Ambiente')">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Motivos" ID="infoMotivos">
                <span class="style11"><strong>MOTIVOS</strong></span><br />
                <table class="style9">
                    <tr class="style10">
                        <td style="text-align: center; font-weight: 700" class="style30">
                            MOTIVO
                        </td>
                    </tr>
                    <tr>
                        <td class="style30">
                            <asp:TextBox ID="tbRazon" runat="server" Height="56px" ValidationGroup="Deta" Width="475px"
                                TextMode="MultiLine" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbRazon"
                                ErrorMessage="*" CssClass="style20" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br class="style12" />
                <strong><span class="style11">IMPUESTOS</span></strong><br class="style12" />
                <table class="style9">
                    <tr>
                        <td class="style21">
                            <b>TIPO<br /></b>
                        </td>
                        <td class="style21">
                            <b>CÓDIGO<br /></b>
                        </td>
                        <td class="style21">
                            <b>CÓDIGO %</b>
                        </td>
                        <td class="style21">
                            <b>TARIFA<br /></b>
                        </td>
                        <td class="style21">
                            <b>BASE IMPONIBLE</b>
                        </td>
                        <td class="style21">
                            <b>IMPUESTO<br /></b>
                        </td>
                        <td rowspan="2">
                            <asp:Button ID="bAgregarMotivo" runat="server" Text="Agregar Motivo" Width="132px"
                                ValidationGroup="Imp" OnClick="bAgregarMotivo_Click" />
                        </td>
                        <td rowspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <asp:DropDownList ID="ddlTipoImpuesto" runat="server" AutoPostBack="True" DataSourceID="SqlDataCatImpuestos"
                                DataTextField="descripcion" DataValueField="codigo" OnSelectedIndexChanged="ddlTipoImpuesto_SelectedIndexChanged"
                                AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Seleccionar</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCodigoID" runat="server" Height="19px" Width="20px" ValidationGroup="Imp"
                                MaxLength="1" ReadOnly="True" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[0-9]{1}?$"
                                ControlToValidate="tbCodigoID" CssClass="style20" ErrorMessage="*" Style="color: #FF0000"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbCodigoID"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCodigoIDP" runat="server" Height="19px" Width="60px" ValidationGroup="Imp"
                                MaxLength="4" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[0-9]{1,4}?$"
                                ControlToValidate="tbCodigoIDP" CssClass="style20" ErrorMessage="*" Style="color: #FF0000"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="tbCodigoIDP"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbTarifa" runat="server" Height="19px" Width="60px" ValidationGroup="Imp"
                                AutoPostBack="True" OnTextChanged="tbTarifa_TextChanged" MaxLength="4" 
                                Visible="False" CssClass="txt_gr2"></asp:TextBox>
                            <asp:DropDownList ID="ddlTasaIVA" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>0</asp:ListItem>
                                <asp:ListItem>No Sujeto</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="tbTarifa"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp" Visible="False"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbBaseImponible" runat="server" Height="19px" Width="70px" ValidationGroup="Imp"
                                AutoPostBack="True" MaxLength="14" 
                                OnTextChanged="tbBaseImponible_TextChanged" CssClass="txt_gr2">0</asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="tbBaseImponible"
                                CssClass="style20" ErrorMessage="*" Style="color: #FF0000" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="tbBaseImponible"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbValor" runat="server" Height="19px" Width="70px" ValidationGroup="Imp"
                                ReadOnly="True" MaxLength="14" CssClass="txt_gr2"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lMsjMotivos" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
                <br />
                <br />
                <asp:GridView ID="gvMotivos" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataMotivos"
                    ForeColor="Black" DataKeyNames="idMotivoTemp" GridLines="Vertical" Width="98%">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="razon" HeaderText="RAZÓN" SortExpression="razon">
                            <HeaderStyle Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="codigo" HeaderText="CODIGO" SortExpression="codigo" />
                        <asp:BoundField DataField="tipo" HeaderText="TIPO" SortExpression="tipo" />
                        <asp:BoundField DataField="codigoPorcentaje" HeaderText="CODIGO %" SortExpression="codigoPorcentaje" />
                        <asp:BoundField DataField="baseImponible" HeaderText="BASE IMPONIBLE" SortExpression="baseImponible" />
                        <asp:BoundField DataField="tarifa" HeaderText="TARIFA" SortExpression="tarifa" />
                        <asp:BoundField DataField="valor" HeaderText="VALOR" SortExpression="valor" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#DEE2EB" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#848384"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#EAEAD3"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#575357"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataMotivos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="DELETE FROM MotivosDebitoTemp WHERE (idMotivoTemp = @idMotivoTemp)"
                    SelectCommand="SELECT idMotivoTemp, razon, codigo, codigoPorcentaje, baseImponible, tarifa, (baseImponible +valor) as valor, id_Empleado, tipo FROM MotivosDebitoTemp WITH (NOLOCK)  WHERE (id_Empleado = @idUser)">
                    <DeleteParameters>
                        <asp:Parameter Name="idMotivoTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataCatImpuestos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT [descripcion], [codigo] FROM [CatImpuestos_C] WITH (NOLOCK) WHERE ([tipo] = @tipo)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Impuesto" Name="tipo" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Documento" ID="infoDocumentos">
                <div class="style5">
                    <span class="style4">Información de la Nota de Débito<br />
                    </span>
                </div>
                <table class="style13">
                    <tr style="vertical-align: top">
                        <td class="style27">
                            COMPROBANTE A MODIFICAR:<br />
                            <asp:DropDownList ID="ddlComprobanteMod" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataComprobanteMod" DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                        </td>
                        <td class="style18">
                            <asp:Label ID="lGuiaRemision" runat="server" Text="DOCUMENTO A MODIFICAR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbNumDocModificado" runat="server" Height="19px" Width="136px" MaxLength="17"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                ControlToValidate="tbNumDocModificado" CssClass="style25" ErrorMessage="*" Style="color: #FF0000"
                                ValidationExpression="^([0-9]{3,3}-[0-9]{3,3}-[0-9]{9,9})?$" ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="tbNumDocModificado"
                                CssClass="style25" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                        </td>
                        <td style="vertical-align: top" class="style26">
                            <asp:Label ID="lFechaEmision" runat="server" Text="FECHA DE EMISIÓN:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbFechaEmision" runat="server" Height="19px" Width="185px" ReadOnly="True"></asp:TextBox>
                            <br />
                            FECHA DE DOCUMENTO A SUSTENTAR:<br />
                            <asp:TextBox ID="tbFechaEmisionDocSustento" runat="server" Height="19px" Width="185px"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:CalendarExtender ID="tbFechaEmisionDocSustento_CalendarExtender" runat="server"
                                Enabled="True" Format="yyyy-MM-ddTHH:mm:ss" TargetControlID="tbFechaEmisionDocSustento">
                            </asp:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                ControlToValidate="tbFechaEmisionDocSustento" CssClass="style25" ErrorMessage="*"
                                Style="color: #FF0000" ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"
                                ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                                ControlToValidate="tbFechaEmisionDocSustento" CssClass="style25" ErrorMessage="*"
                                Style="color: #FF0000" ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"
                                ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                        </td>
                        <td style="vertical-align: top">
                            <asp:Label ID="lMoneda" runat="server" Text="MONEDA:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbMoneda" runat="server" Height="19px" MaxLength="15" Width="70px">DOLAR</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            RISE:<br />
                            <asp:TextBox ID="tbRise" runat="server" Height="34px" Width="355px" TextMode="MultiLine"></asp:TextBox>
                            <br />
                        </td>
                        <td colspan="2">
                            <asp:Label ID="ldirEstablecimiento" runat="server" Text="ESTABLECIMIENTO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirEstablecimiento" runat="server" Height="47px" TextMode="MultiLine"
                                Width="293px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            <asp:CheckBox ID="cbContribuyenteEspecial" runat="server" Text="CONTRIBUYENTE ESPECIAL" />
                            &nbsp;<asp:TextBox ID="tbContribuyenteEspecial" runat="server" MaxLength="4" Width="51px"></asp:TextBox>
                            <br />
                            <asp:CheckBox ID="cbObligado" runat="server" Text="OBLIGADO A CONTABILIDAD" Checked="True" />
                        </td>
                        <td rowspan="2" class="style23" colspan="2">
                            <asp:Label ID="lSubtotal12" runat="server" Text="SUBTOTAL 12%:"></asp:Label>
                            <asp:TextBox ID="tbSubtotal12" runat="server" Height="19px" Width="120px" ReadOnly="True"
                                Style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lSubtotal0" runat="server" Text="SUBTOTAL 0%:"></asp:Label>
                            <asp:TextBox ID="tbSubtotal0" runat="server" Height="19px" ReadOnly="True" Width="120px"
                                Style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lSubtotalNoSujeto" runat="server" Text="SUBTOTAL No sujeto de IVA:"></asp:Label>
                            <asp:TextBox ID="tbSubtotalNoSujeto" runat="server" Height="19px" ReadOnly="True"
                                Width="120px" Style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lTotalSinImpuestos" runat="server" Text="SUBTOTAL SIN IMPUESTOS:"></asp:Label>
                            <asp:TextBox ID="tbTotalSinImpuestos" runat="server" Height="19px" ReadOnly="True"
                                Width="120px" Style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lICE" runat="server" Text="ICE:"></asp:Label>
                            <asp:TextBox ID="tbICE" runat="server" Height="19px" ReadOnly="True" Style="text-align: right"
                                Width="120px">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lIVA12" runat="server" Text="IVA 12%:"></asp:Label>
                            <asp:TextBox ID="tbIVA12" runat="server" Height="19px" ReadOnly="True" Style="text-align: right"
                                Width="120px">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lImporteTotal" runat="server" Text="IMPORTE TOTAL:"></asp:Label>
                            <asp:TextBox ID="tbImporteTotal" runat="server" Height="19px" Width="120px" ReadOnly="True"
                                Style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lImporteaPagar" runat="server" Text="IMPORTE A PAGAR:" Visible="False"></asp:Label>
                            <asp:TextBox ID="tbImporteaPagar" runat="server" Height="19px" ReadOnly="True" Width="120px"
                                Style="text-align: right" Visible="False">0</asp:TextBox>
                            <br style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            <asp:Label ID="lidentificacionComprador" runat="server" Text="IDENTIFICACIÓN DEL COMPRADOR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbIdentificacionComprador" runat="server" Height="19px" MaxLength="13"
                                Width="183px" AutoPostBack="True" OnTextChanged="tbIdentificacionComprador_TextChanged"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="tbIdentificacionComprador"
                                ErrorMessage="Ingresa el RUC" ValidationGroup="Receptor" CssClass="style31" Style="color: #FF0000"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                ControlToValidate="tbIdentificacionComprador" ErrorMessage="RUC Invalido" ValidationExpression="^[0-9a-zA-Z]{0,13}?$"
                                ValidationGroup="Receptor" CssClass="style31" Style="color: #FF0000"></asp:RegularExpressionValidator>
                            <asp:AutoCompleteExtender ID="tbIC_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getRuc" ServicePath="../autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True"
                                TargetControlID="tbIdentificacionComprador" UseContextKey="True">
                            </asp:AutoCompleteExtender>
                            
                                <Services>
                                    <asp:ServiceReference Path="../autoRuc.asmx" />
                                </Services>
                            
                            <br />
                            <br />
                            <asp:Label ID="lTipoIdentificacionComprador" runat="server" Text="IDENTIFICACION"></asp:Label>
                            :<br />&nbsp;&nbsp;<asp:DropDownList ID="ddlTipoIdentificacion" runat="server" DataSourceID="SqlDataTipoIdentificacion"
                                DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True">
                                <asp:ListItem>Selecciona el Tipo</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label ID="lRazonSocialComprador" runat="server" Text="RAZÓN SOCIAL DEL COMPRADOR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocialComprador" runat="server" Height="46px" Width="347px"
                                TextMode="MultiLine" ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbRazonSocialComprador"
                                ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="4">
                            Email:
                            <asp:TextBox ID="tbEmail" runat="server" Height="19px" Width="413px"></asp:TextBox>
                            <br />
                            <asp:Label ID="lMsjDocumento" runat="server" Style="color: #FF0000"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataTipoIdentificacion" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Identificacion')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataComprobanteMod" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (codigo = '05' OR codigo = '01') AND (tipo = 'Comprobante')">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <%--<asp:WizardStep runat="server" Title="Informacion Adicional" ID="infoAdicional">
            <table style="width:100%;">
                <tr>
                    <td class="style6" colspan="2">
                        <strong>Agregar Campo Adicional.</strong></td>
                </tr>
                <tr>
                    <td class="style32">
                        NOMBRE:</td>
                    <td>
                        <b>VALOR:</b></td>
                </tr>
                <tr valign="top">
                    <td>
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

<SortedAscendingCellStyle BackColor="#FBFBF2"></SortedAscendingCellStyle>

<SortedAscendingHeaderStyle BackColor="#848384"></SortedAscendingHeaderStyle>

<SortedDescendingCellStyle BackColor="#EAEAD3"></SortedDescendingCellStyle>

<SortedDescendingHeaderStyle BackColor="#575357"></SortedDescendingHeaderStyle>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataInfoAdic" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                DeleteCommand="DELETE FROM InfoAdicionalTemp WHERE (idInfoAdicionalTemp = @idInfoAdicionalTemp)" 
                SelectCommand="SELECT idInfoAdicionalTemp,nombre, valor, id_Empleado FROM InfoAdicionalTemp WITH (NOLOCK) WHERE (id_Empleado = @id_Empleado)">
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
