<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="crearNotaCredito.aspx.cs" Inherits="DataExpressWeb.crearNotaCredito" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" language="javascript">
    function AgregarDetalleAdicional(cod_principal) {
        var code = cod_principal;

        if (cod_principal.toString().length == 1) {
            code = "00000000" + cod_principal;
        }
        else if (cod_principal.toString().length == 2) {
            code = "0000000" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 3) {
            code = "000000" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 4) {
            code = "00000" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 5) {
            code = "0000" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 6) {
            code = "000" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 7) {
            code = "00" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 8) {
            code = "0" + cod_principal.toString();
        }
        else if (cod_principal.toString().length == 9) {
            code = cod_principal.toString();
        }
        document.getElementById("MainContent_Wizard1_txt_codig_temp").value = code;
    }
     </script>
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
        .style10
        {
            font-size: small;
        }

        .style11
        {
            font-size: small;
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
            width: 155px;
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
    position:absolute;
    margin-left:0px;
    margin-top:-3px;
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
            width: 283px;
        }
           
        </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="1" BackColor="#F7F6F3" 
    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
    Font-Names="Verdana" Font-Size="0.8em" Height="183px" Width="95%" 
        style="margin-bottom: 0px" onactivestepchanged="Wizard1_ActiveStepChanged" 
        OnNextButtonClick="StepNextButton_Click" >
        <FinishNavigationTemplate>
            <asp:Button ID="FinishPreviousButton" runat="server" BackColor="#FFFBFF" 
                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                CausesValidation="False" CommandName="MovePrevious" Font-Names="Verdana" 
                Font-Size="Small" ForeColor="#284775" Text="Anterior" />
            <asp:Button ID="FinishButton" runat="server" BackColor="#FFFBFF" 
                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                CommandName="MoveComplete" Font-Names="Verdana" Font-Size="Small" 
                ForeColor="#284775" Text="Crear Comprobante" onclick="FinishButton_Click" />
        </FinishNavigationTemplate>
    <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" 
        Font-Size="0.9em" ForeColor="White" HorizontalAlign="Left" />
    <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" 
        ForeColor="#284775" />
    <NavigationStyle HorizontalAlign="Center" />
    <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
    <SideBarStyle BackColor="#5F6062" BorderWidth="0px" Font-Size="Small" 
        VerticalAlign="Top" Width="20%" BorderColor="#3333CC" />
        <StepNavigationTemplate>
            <asp:Button ID="StepPreviousButton" runat="server" BackColor="#FFFBFF" 
                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                CausesValidation="False" CommandName="MovePrevious" Font-Names="Verdana" 
                Font-Size="Small" ForeColor="#284775" Text="Anterior" />
            <asp:Button ID="StepNextButton" runat="server" BackColor="#FFFBFF" 
                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                CommandName="MoveNext" Font-Names="Verdana" Font-Size="Small" 
                ForeColor="#284775" Text="Siguiente" ValidationGroup="Form" />
        </StepNavigationTemplate>
    <StepStyle BorderWidth="0px" ForeColor="#666666" />
    <WizardSteps>
        <asp:WizardStep runat="server" title="Información Tributaria" 
            ID="infoTributaria">
            <div class="style3">
                Información Tributaria<br /> </div>
                <br />
            <table class="style13">
                <tr>
                    <td class="style15">
                        AMBIENTE:<br />
                        <asp:DropDownList ID="ddlAmbiente" runat="server" AppendDataBoundItems="True" 
                            DataSourceID="SqlDataAmbiente" DataTextField="descripcion" 
                            DataValueField="codigo">
                            
                        </asp:DropDownList>
                    </td>
                    <td class="style17">
                        EMISIÓN:<br />
                        <asp:DropDownList ID="ddlEmision" runat="server" AppendDataBoundItems="True" 
                            DataSourceID="SqlDataEmision" DataTextField="descripcion" 
                            DataValueField="codigo">
                        
                        </asp:DropDownList>
                    </td>
                    <td>
                        COMPROBANTE:<br />
                        <asp:DropDownList ID="ddlComprobante" runat="server" 
                            AppendDataBoundItems="True" DataSourceID="SqlDataDocumento" 
                            DataTextField="descripcion" DataValueField="codigo">
                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style16">
                        SUCURSAL:<br />
                        <asp:DropDownList ID="ddlSucursal" runat="server" 
                            DataSourceID="SqlDataSucursal" DataTextField="sucursal" 
                            DataValueField="clave">
                        </asp:DropDownList>
                    </td>
                    <td class="style18">
                        PUNTO DE EMISIÓN:<br />
                        <asp:DropDownList ID="ddlPtoEmi" runat="server" 
                            DataSourceID="SqlDataPtoEmision" DataTextField="noEmpleado" 
                            DataValueField="noEmpleado">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lSecuencial" runat="server" Text="SECUENCIA:"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbSecuencial" runat="server" Height="19px" Width="93px" 
                            CssClass="txt_gr2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lRuc" runat="server" Text="RUC:"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbRuc" runat="server" Height="19px" MaxLength="13" 
                            Width="163px" CssClass="txt_gr2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                            ControlToValidate="tbRuc" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                    </td>
                    <td  colspan="2">
                        <asp:Label ID="lRazonSocial" runat="server" Text="RAZÓN SOCIAL:"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbRazonSocial" runat="server" Height="19px" Width="353px" 
                            CssClass="txt_gr2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                            ControlToValidate="tbRazonSocial" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lNombreComercial" runat="server" Text="NOMBRE COMERCIAL"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbNombreComercial" runat="server" Height="19px" Width="293px" 
                            CssClass="txt_gr2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                            ControlToValidate="tbNombreComercial" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lDirMatriz" runat="server" Text="DIRECCIÓN MATRIZ"></asp:Label>
                        <br />
                        <asp:TextBox ID="tbDirMatriz" runat="server" Height="19px" Width="293px" 
                            CssClass="txt_gr2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                            ControlToValidate="tbDirMatriz" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataPtoEmision" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT noEmpleado, idEmpleado FROM Empleados WITH (NOLOCK) WHERE (idEmpleado = @idEmpleado)">
                <SelectParameters>
                    <asp:SessionParameter Name="idEmpleado" SessionField="idUser" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSucursal" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT clave, sucursal FROM Sucursales WITH (NOLOCK) WHERE (idSucursal = @idSucursal)">
                <SelectParameters>
                   <asp:SessionParameter Name="idSucursal" SessionField="sucursalUser" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataDocumento" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                
                SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Comprobante') AND (codigo = '04')">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataEmision" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Emision')">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Ambiente')">
            </asp:SqlDataSource>
            
        </asp:WizardStep>
        <asp:WizardStep runat="server" Title="Detalles" ID="infoDetalles" EnableTheming="True">
                <br />
                <span class="style11"><strong>DETALLES DE NOTA DE CRÉDITO</strong></span><br />
                <br />
                <table>
                    <tr>
                        <td style="font-size: 9px; width: 95px; text-align: center;">
                            <asp:Label ID="lbl_cantidad" runat="server" Text="CANTIDAD:" CssClass="style24"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCantidad" runat="server" Height="19px" Width="80px" Style="text-align: right"
                                        ValidationGroup="Deta" AutoPostBack="True" OnTextChanged="tbCantidad_TextChanged"
                                        CausesValidation="True">1</asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbCantidad_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbCantidad" ValidChars=",.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                        ControlToValidate="tbCantidad" CssClass="style20" ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                        ValidationGroup="Deta" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbCantidad"
                                        ErrorMessage="Debe ser mayor a 0." ForeColor="Red" ValidationGroup="Deta"
                                        InitialValue="0" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator23_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator23" PopupPosition="Right">
                                    </asp:ValidatorCalloutExtender>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbCantidad"
                                        ErrorMessage="Debe ingresar valor." ForeColor="Red" ValidationGroup="Deta"
                                        InitialValue="" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="CustomValidator1_ValidatorCalloutExtender" 
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1" PopupPosition="Right">
                                    </asp:ValidatorCalloutExtender>
                        </td>
                        <td style="font-size: 9px; width: 95px; text-align: center;">
                            <asp:Label ID="lbl_descripcion" runat="server" Text="DESCRIPCIÓN:" CssClass="style24"></asp:Label>
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="tbCodigoA0" runat="server" Height="80px" ValidationGroup="Deta"
                                Width="400px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbCodigoA0"
                                ErrorMessage="*" CssClass="style20" ValidationGroup="Deta"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="tbCodigoP" runat="server" Height="19px" Width="90px" ValidationGroup="Deta"
                                CssClass="txt_gr2" Visible="false"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbCodigoP"
                                ErrorMessage="*" CssClass="style20" ValidationGroup="Deta"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 9px; width: 95px; text-align: center;">
                            <asp:Label ID="lbl_precio" runat="server" Text="PRECIO UNITARIO:" CssClass="style24"></asp:Label>
                        </td>
                        <td>
                             <asp:TextBox ID="tbPU" runat="server" Width="80px" AutoPostBack="True" Height="19px"
                                        OnTextChanged="tbPU_TextChanged" Style="text-align: right" ValidationGroup="Deta"
                                        CausesValidation="True">0</asp:TextBox>
                             <asp:FilteredTextBoxExtender ID="tbPU_FilteredTextBoxExtender" runat="server" Enabled="True"
                                        FilterType="Custom, Numbers" TargetControlID="tbPU" ValidChars=",.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="tbPU" CssClass="style20" ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                        ValidationGroup="Deta" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPU"
                                        ErrorMessage="Debe ser mayor a 0." ForeColor="Red" ValidationGroup="Deta"
                                        InitialValue="0" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2" PopupPosition="Right">
                                    </asp:ValidatorCalloutExtender>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbPU"
                                        ErrorMessage="Debe ingresar valor." ForeColor="Red" ValidationGroup="Deta"
                                        InitialValue="" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3" PopupPosition="Right">
                                    </asp:ValidatorCalloutExtender>
                        </td>
                        <td style="font-size: 9px; width: 95px; text-align: center;" align="right">
                            <asp:Label ID="lbl_descuento" runat="server" Text="DESCUENTO:" CssClass="style24"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbDescuento" runat="server" Width="80px" AutoPostBack="True" Height="19px"
                                        OnTextChanged="tbDescuento_TextChanged" Style="text-align: right; margin-bottom: 0px"
                                        ValidationGroup="Deta" CausesValidation="True">0</asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbDescuento_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbDescuento" ValidChars=",.">
                                    </asp:FilteredTextBoxExtender>
                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ControlToValidate="tbDescuento" CssClass="style20" ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                        ValidationGroup="Deta" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                   
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbDescuento"
                                        ErrorMessage="Debe ingresar valor." ForeColor="Red" ValidationGroup="Deta"
                                        InitialValue="" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" 
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator6" PopupPosition="Right">
                                    </asp:ValidatorCalloutExtender>
                        </td>
                        <td align="right" style="font-size: 9px; width: 95px; text-align: center;">
                            <asp:Label ID="lbl_Subtotal" runat="server" Text="SUBTOTAL:" CssClass="style24"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbImporteConcepto" runat="server" Height="19px" Width="80px" Style="text-align: right"
                                ValidationGroup="Deta" ReadOnly="True" CausesValidation="True" >0</asp:TextBox>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="tbImporteConcepto"
                                CssClass="style20" ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"
                                ValidationGroup="Deta"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td style="font-size: 9px; text-align: center; width: 75px;" align="right">
                            <asp:Label ID="lbl_iva" runat="server" Text="IVA:" CssClass="style24"></asp:Label>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlTasaIVA" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                Height="25px" Width="60px">
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>0</asp:ListItem>
                                <asp:ListItem>No Sujeto</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="tbValor" runat="server" Height="19px" Width="80px" ValidationGroup="Imp"
                                ReadOnly="True" MaxLength="14"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9" align="center">
                            <asp:Button ID="bAgregarImpuesto" runat="server" Text="Agregar Detalle" OnClick="bAgregarImpuesto_Click"
                                Width="132px" Height="30px" ValidationGroup="Imp" CssClass="botones" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlTipoImpuesto" runat="server" AutoPostBack="True" DataSourceID="SqlDataCatImpuestos"
                                DataTextField="descripcion" DataValueField="codigo" OnSelectedIndexChanged="ddlTipoImpuesto_SelectedIndexChanged"
                                AppendDataBoundItems="True" Visible="false">
                                <asp:ListItem Selected="True" Text="IVA" Value="2">IVA</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCodigoID" runat="server" Height="19px" Width="20px" ValidationGroup="Imp"
                                MaxLength="1" Visible="False" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[0-9]{1}?$"
                                ControlToValidate="tbCodigoID" CssClass="style20" ErrorMessage="*"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="tbCodigoID"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbCodigoIDP" runat="server" Height="19px" Width="60px" ValidationGroup="Imp"
                                MaxLength="4" Visible="False" CssClass="txt_gr2"></asp:TextBox>
                            <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[0-9]{1,4}?$"
                                ControlToValidate="tbCodigoIDP" CssClass="style20" ErrorMessage="*"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="tbCodigoIDP"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>
                            <asp:TextBox ID="tbTarifa" runat="server" Height="19px" Width="60px" ValidationGroup="Imp"
                                AutoPostBack="True" OnTextChanged="tbTarifa_TextChanged" MaxLength="4" Visible="False"
                                CssClass="txt_gr2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="tbTarifa"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Imp" Visible="False"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbBaseImponible" runat="server" Height="30px" Width="70px" ValidationGroup="Imp"
                                AutoPostBack="True" ReadOnly="True" MaxLength="14" CssClass="txt_gr2" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <%--<strong><span class="style11">IMPUESTO</span></strong><br class="style12" />--%>
                <asp:Label ID="lMsjImpuestos" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
                <%-- <strong><span class="style11">DETALLE ADICIONAL</span></strong><br class="style12" />
                <table class="style9">
                    <tr>
                        <td>
                            <b>NOMBRE</b>
                        </td>
                        <td>
                            <b>VALOR</b>
                        </td>
                        <td rowspan="2">
                            <asp:Button ID="bAgregarDetalle" runat="server" Text="Agregar Det. Adic." Height="25px"
                                OnClick="bAgregarDetalleAdic_Click" Width="119px" ValidationGroup="detallesAdic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbNombreConcepto" runat="server" Height="34px" Width="254px" MaxLength="300"
                                TextMode="MultiLine" ValidationGroup="detallesAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNombreConcepto"
                                CssClass="style20" ErrorMessage="*" ValidationGroup="detallesAdic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbValorConcepto" runat="server" Height="34px" Width="254px" MaxLength="300"
                                TextMode="MultiLine" ValidationGroup="detallesAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbValorConcepto"
                                CssClass="style20" ErrorMessage="*" ValidationGroup="detallesAdic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>--%>
                <asp:TextBox ID="txt_codig_temp" runat="server" CssClass="txt_gr2" 
                    Height="1px" Width="1px"></asp:TextBox>
                <br />
                <br />
                <%-- <asp:Button ID="bAgregarConcepto" runat="server" Text="Agregar Detalle" Width="118px"
                    OnClick="bAgregarConcepto_Click" ValidationGroup="Deta" />--%>
                <asp:Label ID="lMsjDetalles" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
                <br />
                <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataDetalles"
                    ForeColor="Black" DataKeyNames="codigoPrincipal" GridLines="Vertical" Width="98%" OnSelectedIndexChanged="gvDetalles_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="cantidad" HeaderText="CANTIDAD" SortExpression="cantidad" />
                        <%--<asp:BoundField DataField="codigoPrincipal" HeaderText="COD. PRINC." SortExpression="codigoPrincipal" />
                        <asp:BoundField DataField="codigoAuxiliar" HeaderText="COD. AUX." SortExpression="codigoAuxiliar" />--%>
                        <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" SortExpression="descripcion" />
                        <asp:BoundField DataField="precioUnitario" HeaderText="PRECIO UNITARIO" SortExpression="precioUnitario" />
                        <asp:BoundField DataField="descuento" HeaderText="DESCUENTO" SortExpression="descuento" />
                        <asp:BoundField DataField="precioTotalSinImpuestos" HeaderText="SUBTOTAL" SortExpression="precioTotalSinImpuestos" />
                         <asp:CommandField SelectText="Asignar" HeaderText="Detalle Adicional" ShowSelectButton="true"  FooterStyle-VerticalAlign="NotSet" />
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkDetaAdicional" runat="server" OnClientClick='<%# Eval("codigoPrincipal", "javascript:AgregarDetalleAdicional({0});") %>' Text="Ver"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                <br />
                <asp:GridView ID="gvImpuestosDetalles" runat="server" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                    CellPadding="4" DataSourceID="SqlDataImpuestosConceptos" ForeColor="Black" DataKeyNames="idImpuestosDetallesTemp"
                    GridLines="Vertical" Height="18px" Width="90%" Visible="false">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="CODIGO" SortExpression="codigo">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("codigo") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("codigo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CODIGO %" SortExpression="codigoPorcentaje">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("codigoPorcentaje") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("codigoPorcentaje") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TARIFA" SortExpression="tarifa">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("tarifa") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("tarifa") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BASE IMPONIBLE" SortExpression="baseImponible">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("baseImponible") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("baseImponible") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VALOR" SortExpression="valor">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("valor") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("valor") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar."></asp:LinkButton>
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
                <asp:GridView ID="gvDetAdic" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataDetAdicionalesDetalles"
                    ForeColor="Black" GridLines="Vertical" Width="80%" DataKeyNames="idDetallesAdicionalesTemp">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="NOMBRE" SortExpression="nombre">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("nombre") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VALOR" SortExpression="valor">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("valor") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("valor") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
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
                <asp:SqlDataSource ID="SqlDataDetalles" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="PA_eliminar_DetallesTemp" SelectCommand="SELECT cantidad, codigoPrincipal, codigoAuxiliar, descripcion, precioUnitario, descuento, precioTotalSinImpuestos, id_Empleado FROM DetallesTemp WITH (NOLOCK)  WHERE (id_Empleado = @idUser)"
                    DeleteCommandType="StoredProcedure">
                    <DeleteParameters>
                        <asp:Parameter Name="codigoPrincipal" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataImpuestosConceptos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT idImpuestosDetallesTemp,codigo, codigoPorcentaje, tarifa, baseImponible, valor, id_Empleado, codigoTemp FROM ImpuestosDetallesTemp WITH (NOLOCK)  WHERE (id_Empleado = @idUser) AND (codigoTemp = @codigoTemp)"
                    DeleteCommand="DELETE FROM ImpuestosDetallesTemp WHERE (idImpuestosDetallesTemp = @idImpuestosDetallesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idImpuestosDetallesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                        <asp:ControlParameter ControlID="tbCodigoP" Name="codigoTemp" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDetAdicionalesDetalles" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT idDetallesAdicionalesTemp,nombre, valor, codigoTemp, id_Empleado FROM DetallesAdicionalesTemp WITH (NOLOCK)  WHERE (codigoTemp = @codigoTemp) AND (id_Empleado = @idUser)"
                    DeleteCommand="DELETE FROM DetallesAdicionalesTemp WHERE (idDetallesAdicionalesTemp = @idDetallesAdicionalesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDetallesAdicionalesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txt_codig_temp" Name="codigoTemp" PropertyName="Text" />
                       <%-- <asp:ControlParameter ControlID="tbCodigoP" Name="codigoTemp" PropertyName="Text" />--%>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataCatImpuestos" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT [descripcion], [codigo] FROM [CatImpuestos_C] WITH (NOLOCK)  WHERE ([tipo] = @tipo)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Impuesto" Name="tipo" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
        <asp:WizardStep runat="server" title="Documento" ID="infoDocumentos">
            <div class="style5">
                <span class="style4">Información de la Nota de Crédito<br /> </span></div>
                <table class="style13">
                    <tr style="vertical-align:top">
                        <td class="style27">

                            COMPROBANTE A MODIFICAR:<br />
                            <asp:DropDownList ID="ddlComprobanteMod" runat="server" 
                                AppendDataBoundItems="True" DataSourceID="SqlDataComprobanteMod" 
                                DataTextField="descripcion" DataValueField="codigo">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataComprobanteMod" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                                SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (codigo = '01') AND (tipo = 'Comprobante')">
                            </asp:SqlDataSource>

                        </td>
                        <td class="style18">
                                                        <asp:Label ID="lGuiaRemision" runat="server" Text="DOCUMENTO A MODIFICAR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbNumDocModificado" runat="server" Height="19px" Width="136px" MaxLength="17" 
                                                            OnTextChanged="tbNumDocModificado_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" 
                                                            runat="server" ControlToValidate="tbNumDocModificado" CssClass="style25" 
                                                            ErrorMessage="*" style="color: #FF0000" 
                                                            ValidationExpression="^([0-9]{3,3}-[0-9]{3,3}-[0-9]{9,9})?$"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" 
                                                            ControlToValidate="tbNumDocModificado" CssClass="style25" ErrorMessage="*" 
                                                            style="color: #FF0000"></asp:RequiredFieldValidator>
                                                            <br />
                                                        <asp:Label ID="lbl_termino" runat="server" Text="TÉRMINO:" Visible="False"></asp:Label>
                                                        <asp:TextBox ID="txt_termino" runat="server" Height="19px" MaxLength="15" 
                                                            Width="70px" Visible="False"></asp:TextBox>
                                                        
                        </td>
                        <td style="vertical-align:top" class="style26">
                            <asp:Label ID="lFechaEmision" runat="server" Text="FECHA DE EMISIÓN:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbFechaEmision" runat="server" Height="19px" Width="185px" 
                                ReadOnly="True"></asp:TextBox>
                                
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator12" 
                                runat="server" ControlToValidate="tbFechaEmision" CssClass="style25" 
                                ErrorMessage="*" style="color: #FF0000" 
                                ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"></asp:RegularExpressionValidator>--%>

                            <br />
                            FECHA DE DOCUMENTO A SUSTENTAR:<br />
                            <asp:TextBox ID="tbFechaEmisionDocSustento" runat="server" Height="19px" 
                                Width="185px"></asp:TextBox>
                            <asp:CalendarExtender ID="tbFechaEmisionDocSustento_CalendarExtender" 
                                runat="server" Enabled="True" Format="dd/MM/yyyy" 
                                TargetControlID="tbFechaEmisionDocSustento">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" 
                                ControlToValidate="tbFechaEmisionDocSustento" CssClass="style25" 
                                ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>

                                 <br />
                                <br />
                            
                            &nbsp;&nbsp;
                                                       
                               

                           
                        </td>
                        <td style="vertical-align: top">
                            <asp:Label ID="lMoneda" runat="server" Text="MONEDA:" Visible="false"></asp:Label>
                             <asp:TextBox ID="tbMoneda" runat="server" Height="19px" MaxLength="15" 
                                Width="70px" Visible="false">DOLAR</asp:TextBox>
                           
                            <br />
                             <asp:Label ID="lbl_proforma" runat="server" Text="PROFORMA:" Visible="False"></asp:Label>
                             <br />
                            
                            <asp:TextBox ID="txt_proforma" runat="server" Height="19px" MaxLength="15" 
                                Width="70px" Visible="False"></asp:TextBox>

                                
                                 
                            <br />
                                 <asp:Label ID="lbl_pedido" runat="server" Text="PEDIDO:" Visible="False"></asp:Label> 
                            <br />
                            <asp:TextBox ID="txt_pedido" runat="server" Height="19px" MaxLength="15" 
                                Width="70px" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            RISE:<br />
                            <asp:TextBox ID="tbRise" runat="server" Height="19px" Width="393px"></asp:TextBox>
                            <br />
                            <asp:Label ID="ldirEstablecimiento" runat="server" Text="DIRECCION SUCURSAL EMISOR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirEstablecimiento" runat="server" Height="47px" 
                                Width="393px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            MOTIVO DE LA NOTA:<br />
                            <asp:TextBox ID="tbMotivo" runat="server" Height="67px" MaxLength="300" 
                                TextMode="MultiLine" Width="334px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" 
                                ControlToValidate="tbMotivo" ErrorMessage="*" style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            <asp:CheckBox ID="cbContribuyenteEspecial" runat="server" 
                                Text="CONTRIBUYENTE ESPECIAL" />
                            &nbsp;<asp:TextBox ID="tbContribuyenteEspecial" runat="server" MaxLength="4" 
                                Width="51px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbContribuyenteEspecial_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                TargetControlID="tbContribuyenteEspecial">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <asp:CheckBox ID="cbObligado" runat="server" Text="OBLIGADO A CONTABILIDAD" 
                                Checked="True" />
                        </td>
                        <td rowspan="2" class="style23" colspan="2">
                            <asp:Label ID="lSubtotal12" runat="server" Text="SUBTOTAL 12%:"></asp:Label>
                            <asp:TextBox ID="tbSubtotal12" runat="server" Height="19px" 
                                Width="150px" ReadOnly="True" style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lSubtotal0" runat="server" Text="SUBTOTAL 0%:"></asp:Label>
                            <asp:TextBox ID="tbSubtotal0" runat="server" Height="19px" 
                                ReadOnly="True" Width="150px" style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lSubtotalNoSujeto" runat="server" Text="SUBTOTAL No sujeto de IVA:"></asp:Label>
                            <asp:TextBox ID="tbSubtotalNoSujeto" runat="server" Height="19px" 
                                ReadOnly="True" Width="150px" style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lTotalSinImpuestos" runat="server" 
                                Text="SUBTOTAL SIN IMPUESTOS:"></asp:Label>
                            <asp:TextBox ID="tbTotalSinImpuestos" runat="server" Height="19px" 
                                ReadOnly="True" Width="150px" style="text-align: right">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lTotalDescuento" runat="server" Text="DESCUENTO:"></asp:Label>
                            <asp:TextBox ID="tbTotalDescuento" runat="server" Height="19px" Width="150px" 
                                ReadOnly="True" style="text-align: right">0</asp:TextBox>
                            
                            <br />
                            <asp:Label ID="lIVA12" runat="server" Text="IVA 12%:"></asp:Label>
                            <asp:TextBox ID="tbIVA12" runat="server" Height="19px" ReadOnly="True" 
                                style="text-align: right" Width="150px">0</asp:TextBox>
                            <br />
                            <asp:Label ID="lImporteTotal" runat="server" Text="IMPORTE TOTAL:"></asp:Label>
                            <asp:TextBox ID="tbImporteTotal" runat="server" Height="19px" Width="150px" 
                                ReadOnly="True" style="text-align: right">0</asp:TextBox>
                                                        <br />
                            <asp:Label ID="lImporteaPagar" runat="server" Text="IMPORTE A PAGAR:"></asp:Label>
                            <asp:TextBox ID="tbImporteaPagar" runat="server" Height="19px" ReadOnly="True" 
                                Width="150px" style="text-align: right">0</asp:TextBox>
                            <br style="text-align: right" />
                            <asp:Label ID="lPropinas" runat="server" Text="PROPINAS:" Visible="false"></asp:Label>
                            <asp:TextBox ID="tbPropinas" runat="server" Height="19px" Width="150px" 
                                style="text-align: right" AutoPostBack="True" OnTextChanged="tbPropinas_TextChanged" Visible="false" 
                                >0</asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator15" 
                                runat="server" ControlToValidate="tbPropinas" 
                                ErrorMessage="*" ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$" 
                                ValidationGroup="Receptor" style="color: #FF0000"></asp:RegularExpressionValidator>
                            <asp:Label ID="lICE" runat="server" Text="ICE:" Visible="false"></asp:Label>
                            <asp:TextBox ID="tbICE" runat="server" Height="19px" ReadOnly="True" 
                                style="text-align: right" Width="150px" Visible="false">0</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
                            
                            <asp:Label ID="lidentificacionComprador" runat="server" 
                                Text="IDENTIFICACIÓN DEL COMPRADOR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbIdentificacionComprador" runat="server" Height="19px" 
                                MaxLength="13" Width="183px" AutoPostBack="True" OnTextChanged="tbIdentificacionComprador_TextChanged"
                                ></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="tbIdentificacionComprador_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                TargetControlID="tbIdentificacionComprador">
                            </asp:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" 
                                ControlToValidate="tbIdentificacionComprador" CssClass="style29" 
                                ErrorMessage="Ingresa el RUC"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" 
                                runat="server" ControlToValidate="tbIdentificacionComprador" CssClass="style29" 
                                ErrorMessage="RUC Invalido" ValidationExpression="^[0-9a-zA-Z]{0,13}?$"></asp:RegularExpressionValidator>
                            
                                <Services>
                                    <asp:ServiceReference Path="../autoRuc.asmx" />
                                </Services>
                            
                            <asp:AutoCompleteExtender ID="tbIC_AutoCompleteExtender" runat="server" 
                                CompletionInterval="10" CompletionListCssClass="CompletionListCssClass" 
                                CompletionSetCount="12" DelimiterCharacters="" Enabled="True" 
                                ServiceMethod="getRuc" ServicePath="../autoRuc.asmx" 
                                ShowOnlyCurrentWordInCompletionListItem="True" TargetControlID="tbIdentificacionComprador" 
                                UseContextKey="True">
                                </asp:AutoCompleteExtender>
                            <br />
                            <br />
                             <asp:Label ID="lTipoIdentificacionComprador" runat="server" 
                                Text="IDENTIFICACION"></asp:Label>
                            :<br />&nbsp;&nbsp;<asp:DropDownList ID="ddlTipoIdentificacion" runat="server" 
                                DataSourceID="SqlDataTipoIdentificacion" DataTextField="descripcion" 
                                DataValueField="codigo" AppendDataBoundItems="True">
                                <asp:ListItem>Selecciona el Tipo</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label ID="lRazonSocialComprador" runat="server" 
                                Text="RAZÓN SOCIAL DEL COMPRADOR:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocialComprador" runat="server" Height="46px" 
                                Width="393px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                                ControlToValidate="tbRazonSocialComprador" ErrorMessage="*" 
                                style="color: #FF0000"></asp:RequiredFieldValidator>

                                 <br />
                                <br />
                                <asp:Label ID="lbl_dir_cli" runat="server" Text="DOMICILIO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txt_dir_cli" runat="server" Height="46px" Width="393px"
                                TextMode="MultiLine"></asp:TextBox>
                                 <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="4">
                            Email:
                            <asp:TextBox ID="tbEmail" runat="server" Height="19px" Width="413px"></asp:TextBox>
                            
                            <asp:Label ID="lMsjDocumento" runat="server" style="color: #FF0000"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="lbl_fono" runat="server" Text="TELÉFONO:"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txt_fono" runat="server" Height="19px" MaxLength="15" 
                                Width="100px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txt_fono_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                TargetControlID="txt_fono" ValidChars=",-">
                            </asp:FilteredTextBoxExtender>
                            <br />
                            <span class="style10">Formato:
                            <a href="mailto:email1@email.com,email2@email.com">
                            email1@email.com,email2@email.com</a> </span>
                            <br />
                            <span class="style10">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator16" 
                                runat="server" ControlToValidate="tbEmail" 
                                ErrorMessage="El formato de Email no es válido" ForeColor="Red" 
                                ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3}))([,][_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+((\.[a-z0-9-]+)|(\.[a-z0-9-]+)(\.[a-z]{2,3})))*$" 
                                ValidationGroup="email"></asp:RegularExpressionValidator>
                            </span>
                            <br />
                            <br />
                        </td>
                    </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataTipoIdentificacion" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Identificacion')">
            </asp:SqlDataSource>
            
        </asp:WizardStep>
       <%-- <asp:WizardStep runat="server" Title="Informacion Adicional" ID="infoAdicional">
            <table style="width:100%;">
                <tr>
                    <td class="style6">
                        <strong>Agregar Campo Adicional.</strong></td>
                </tr>
                <tr>
                    <td class="style28">
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