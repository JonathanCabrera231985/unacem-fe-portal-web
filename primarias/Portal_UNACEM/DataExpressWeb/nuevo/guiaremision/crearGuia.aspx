<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="crearGuia.aspx.cs" Inherits="DataExpressWeb.crearGuia" %>

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
            width: 93%;
            height: 37px;
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
            width: 146px;
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
        .style24
        {
        }
        .style25
        {
        }
        .style29
        {
            text-align: center;
            width: 107px;
        }
        .style30
        {
            text-align: center;
            width: 164px;
        }
        .style10
        {
            font-size: small;
        }
        
        .style31
        {
            text-align: center;
            font-weight: bold;
            font-size: xx-small;
            height: 16px;
        }
        .style33
        {
            width: 392px;
        }
        .style34
        {
            width: 349px;
            font-weight: bold;
        }
        .style35
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" BackColor="#F7F6F3" BorderColor="#CCCCCC"
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
            BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" ForeColor="#173E57" />
        <NavigationStyle HorizontalAlign="Center" />
        <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
        <SideBarStyle BackColor="#5F6062" BorderWidth="0px" Font-Size="Small" VerticalAlign="Top"
            Width="20%" BorderColor="#173E57" />
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
                    SelectCommand="SELECT noEmpleado, idEmpleado FROM Empleados WITH (NOLOCK) WHERE (idEmpleado = @idEmpleado)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idEmpleado" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSucursal" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT clave, sucursal FROM Sucursales WITH (NOLOCK) WHERE (idSucursal = @idSucursal)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idSucursal" SessionField="sucursalUser" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataDocumento" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Comprobante') AND (codigo = '06')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataEmision" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion, codigo, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Emision')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataAmbiente" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK)  WHERE (tipo = 'Ambiente')">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Destinatario" ID="Destinatario">
                <span class="style11"><strong>DESTINATARIO<br />
                </strong></span>
                <br />
                <table class="style13">
                    <tr valign="top">
                        <td class="style25">
                            <strong>IDENTIFICACIÓN:<br />
                                <asp:TextBox ID="tbIdentificacion" runat="server" Height="20px" ValidationGroup="Destinatario"
                                    Width="118px" CssClass="txt_gr2"></asp:TextBox>
                            </strong>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="*"
                                Style="color: #FF0000" ControlToValidate="tbIdentificacion" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <strong>RAZÓN SOCIAL:</strong><br />
                            <asp:TextBox ID="tbRazonDestinatario" runat="server" Height="32px" TextMode="MultiLine"
                                ValidationGroup="Destinatario" Width="249px" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="*"
                                Style="color: #FF0000" ControlToValidate="tbRazonDestinatario" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3">
                            <strong>DIRECCIÓN:</strong><br />
                            <asp:TextBox ID="tbDireccionDestinatario" runat="server" Height="32px" TextMode="MultiLine"
                                ValidationGroup="Destinatario" Width="239px" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="*"
                                Style="color: #FF0000" ControlToValidate="tbDireccionDestinatario" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style25" colspan="3">
                            <strong>MOTIVO DE TRASLADO:</strong><br />
                            <asp:TextBox ID="tbMotivoTraslado" runat="server" Height="32px" TextMode="MultiLine"
                                ValidationGroup="Destinatario" Width="373px" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="*"
                                Style="color: #FF0000" ControlToValidate="tbMotivoTraslado" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="3" style="font-weight: 700">
                            <strong>DOC. ADUANERO UNICO:<br />
                                <asp:TextBox ID="tbDocAduaneroUnico" runat="server" Height="21px" MaxLength="20"
                                    ValidationGroup="Deta" Width="149px" CssClass="txt_gr2"></asp:TextBox>
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="style25" colspan="6">
                            <strong>RUTA:<br />
                                <asp:TextBox ID="tbRutaTraslado" runat="server" Height="20px" ValidationGroup="Deta"
                                    Width="40%" CssClass="txt_gr2"></asp:TextBox>
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="style29">
                            <strong>COD. ESTABL. DESTINO:<br />
                            </strong>
                        </td>
                        <td class="style29">
                            <strong>
                                <br />
                                DOCUMENTO:<br />
                            </strong>
                        </td>
                        <td class="style30">
                            <strong>NO. DE DOC:</strong><br />
                        </td>
                        <td class="style21">
                            <strong>NO. DE AUTORIZACIÓN DEL DOCUMENTO</strong><br />
                            &nbsp;
                        </td>
                        <td class="style21">
                            &nbsp;
                        </td>
                        <td>
                            <strong>FECHA DE EMISION:<br />
                            </strong>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="style29">
                            <strong>
                                <asp:TextBox ID="tbCodigoEstabl" runat="server" Height="19px" Style="text-align: right"
                                    ValidationGroup="Deta" Width="50px" MaxLength="3" CssClass="txt_gr2">001</asp:TextBox>
                            </strong>
                        </td>
                        <td class="style29">
                            <strong>
                                <asp:DropDownList ID="ddlcodDocSustento" runat="server" AppendDataBoundItems="True"
                                    DataSourceID="SqlDataComprobante" DataTextField="descripcion" DataValueField="codigo"
                                    ValidationGroup="Destinatario">
                                    <asp:ListItem Value="0">Elegir Documento</asp:ListItem>
                                </asp:DropDownList>
                            </strong>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="ddlcodDocSustento"
                                ErrorMessage="*" InitialValue="0" Style="color: #FF0000" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                        <td class="style30">
                            <asp:TextBox ID="tbNumDocSustento" runat="server" Height="19px" MaxLength="17" ValidationGroup="Destinatario"
                                Width="130px" CssClass="txt_gr2"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                ControlToValidate="tbNumDocSustento" CssClass="style25" ErrorMessage="*" Style="color: #FF0000"
                                ValidationExpression="^([0-9]{3,3}-[0-9]{3,3}-[0-9]{9,9})?$" ValidationGroup="Destinatario"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="tbNumDocSustento"
                                CssClass="style25" ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                        <td class="style21">
                            <asp:TextBox ID="tbNumAutoDocSustento" runat="server" Height="21px" MaxLength="37"
                                ValidationGroup="Deta" Width="171px" CssClass="txt_gr2"></asp:TextBox>
                        </td>
                        <td class="style21">
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="tbfechaEmisionDocSustento" runat="server" Height="21px" ValidationGroup="Destinatario"
                                Width="104px" CssClass="txt_gr2"></asp:TextBox>
                            <asp:CalendarExtender ID="tbfechaEmisionDocSustento_CalendarExtender" runat="server"
                                Enabled="True" TargetControlID="tbfechaEmisionDocSustento" Format="dd/MM/yyyy" >
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="tbfechaEmisionDocSustento"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Destinatario"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lMsjDetinatario" runat="server" Style="color: #FF0000"></asp:Label>
                <strong>
                    <br />
                    <asp:SqlDataSource ID="SqlDataComprobante" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WITH (NOLOCK) WHERE (tipo = 'Comprobante')">
                    </asp:SqlDataSource>
                </strong>
                <br />
                <asp:Button ID="bDestinatarios" runat="server" OnClick="Button1_Click" Text="Agregar Destinatario"
                    Width="129px" ValidationGroup="Destinatario" />
                <br />
                <br />
                <asp:GridView ID="gvDestinatario" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="SqlDataDestinatarios" GridLines="None" DataKeyNames="idDestinatarioTemp"
                    Width="95%" ForeColor="#333333">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="Destinatario" HeaderText="DESTINATARIO" ReadOnly="True"
                            SortExpression="Destinatario" />
                        <asp:BoundField DataField="dirDestinatario" HeaderText="DIRECCIÓN" SortExpression="dirDestinatario" />
                        <asp:BoundField DataField="motivoTraslado" HeaderText="MOTIVO" SortExpression="motivoTraslado" />
                        <asp:BoundField DataField="ruta" HeaderText="RUTA" SortExpression="ruta" />
                        <asp:BoundField DataField="Documento" HeaderText="DOCUMENTO" ReadOnly="True" SortExpression="Documento" />
                        <asp:BoundField DataField="numDocSustento" HeaderText="NO. DOC." SortExpression="numDocSustento" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#DEE2EB" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataDestinatarios" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="DELETE FROM DestinatariosTemp WHERE (idDestinatarioTemp = @idDestinatarioTemp)"
                    SelectCommand="SELECT identificacionDestinatario +' '+ razonSocialDestinatario AS Destinatario, dirDestinatario, motivoTraslado, docAduaneroUnico, codEstabDestino, ruta, (SELECT descripcion FROM Catalogo1_C AS c WITH (NOLOCK)  WHERE (codigo = DestinatariosTemp.codDocSustento) AND (tipo = 'Comprobante')) AS Documento, numDocSustento, id_Empleado,idDestinatarioTemp FROM DestinatariosTemp WITH (NOLOCK)  WHERE (id_Empleado = @idUser)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDestinatarioTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Detalles" ID="infoDetalles">
                <br class="style12" />
                <strong><span class="style11">
                    <asp:DropDownList ID="ddlDestinatario" runat="server" AppendDataBoundItems="True"
                        DataSourceID="SqlDataLstDestinatarios" DataTextField="Destinatario" DataValueField="idDestinatarioTemp"
                         AutoPostBack="True">
                        <asp:ListItem Selected="True" Value="0">Selecciona el Destinatario:</asp:ListItem>
                    </asp:DropDownList>

                    <%--OnSelectedIndexChanged="ddlDestinatario_SelectedIndexChanged" AutoPostBack="True">--%>
                </span>
                    <table class="style9">
                        <tr class="style10">
                            <td class="style31">
                                &nbsp;
                            </td>
                            <strong>
                                <td class="style31">
                                    CÓDIGO INTERNO
                                </td>
                            </strong>
                            <td class="style31">
                            </td>
                            <td class="style31">
                                CÓDIGO ADICIONAL
                            </td>
                            <td class="style31">
                                CANTIDAD
                            </td>
                            <td class="style31">
                                DESCRIPCIÓN
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <strong>
                                <td>
                                    <asp:TextBox ID="tbCodigoInterno" runat="server" Height="19px" MaxLength="25" ValidationGroup="DetallesDestinatario"
                                        Width="107px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="tbCodigoInterno"
                                        CssClass="style20" ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                                </td>
                            </strong>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="tbCodigoAdicional" runat="server" Height="19px" MaxLength="25" ValidationGroup="DetallesDestinatario"
                                    Width="110px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="tbCantidad" runat="server" Height="19px" Style="text-align: right"
                                    ValidationGroup="DetallesDestinatario" Width="50px">1</asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                    ControlToValidate="tbCantidad" CssClass="style20" ErrorMessage="*" Style="color: #FF0000"
                                    ValidationExpression="^[0-9]+(([\.]|[\,])[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="tbDescripcion" runat="server" Height="56px" MaxLength="300" TextMode="MultiLine"
                                    ValidationGroup="DetallesDestinatario" Width="322px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbDescripcion"
                                    CssClass="style20" ErrorMessage="*" Style="color: #FF0000"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="bAgregarDetalleDest" runat="server" Height="25px" Text="Agregar Detalle"
                        ValidationGroup="DetallesDestinatario" Width="119px" OnClick="bAgregarDetalleDest_Click" />
                    <br />
                </strong>
                <asp:Label ID="lMsjDetallesDest" runat="server" Style="color: #FF0000"></asp:Label>
                <br />
                <asp:GridView ID="gvDetallesDestinatario" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" DataSourceID="SqlDataDetallesDest" ForeColor="#333333" DataKeyNames="idDetallesTemp"
                    GridLines="None" Height="18px" Width="90%">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="codigoAuxiliar" HeaderText="COD. AUXILIAR" SortExpression="codigoAuxiliar" />
                        <asp:BoundField DataField="codigoPrincipal" HeaderText="COD. PRINCIPAL" SortExpression="codigoPrincipal" />
                        <asp:BoundField DataField="cantidad" HeaderText="CANT." SortExpression="cantidad" />
                        <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" SortExpression="descripcion" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#DEE2EB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#6D95E1"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#E9EBEF"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#4870BE"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataDetallesDest" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="DELETE FROM DetallesTemp WHERE (idDetallesTemp = @idDetallesTemp)"
                    SelectCommand="SELECT DetallesTemp.codigoAuxiliar, DetallesTemp.codigoPrincipal, DetallesTemp.cantidad, DetallesTemp.descripcion,DetallesTemp.idDetallesTemp FROM DestinatariosTemp WITH (NOLOCK)  INNER JOIN DetallesTemp WITH (NOLOCK) ON DestinatariosTemp.idDestinatarioTemp = DetallesTemp.id_Destinatario WHERE (DestinatariosTemp.idDestinatarioTemp = @idDestinatario) AND (DetallesTemp.id_Empleado = @idUser)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDetallesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlDestinatario" Name="idDestinatario" PropertyName="SelectedValue" />
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <%-- <asp:DropDownList ID="ddlDetallesDestinatarios" runat="server" DataSourceID="SqlDataLstDetallesDest"
                    DataTextField="Descripcion" DataValueField="idDetallesTemp" AppendDataBoundItems="True"
                    AutoPostBack="True" Height="32px" OnSelectedIndexChanged="ddlDetallesDestinatarios_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="0">Selecciona un Detalle</asp:ListItem>
                </asp:DropDownList>--%>
                <%--<asp:SqlDataSource ID="SqlDataLstDetallesDest" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT descripcion + ': ' + codigoPrincipal AS Descripcion, id_Destinatario, id_Empleado, idDetallesTemp FROM DetallesTemp WITH (NOLOCK)  WHERE (id_Empleado = @idUser) AND (id_Destinatario = @idDestinatario)">
                    <SelectParameters>
                        <asp:SessionParameter Name="idUser" SessionField="idUser" />
                        <asp:ControlParameter ControlID="ddlDestinatario" Name="idDestinatario" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
                <%-- <strong><span class="style11">
                    <br />
                    DETALLE ADICIONAL</span></strong><br class="style12" />
                <table class="style9">
                    <tr>
                        <td>
                            <b>NOMBRE</b>
                        </td>
                        <td>
                            <b>VALOR</b>
                        </td>
                        <td rowspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbDENombre" runat="server" Height="34px" Width="254px" MaxLength="300"
                                TextMode="MultiLine" ValidationGroup="detallesAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDENombre"
                                CssClass="style20" ErrorMessage="*" ValidationGroup="detallesAdic" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="tbDEValor" runat="server" Height="34px" Width="254px" MaxLength="300"
                                TextMode="MultiLine" ValidationGroup="detallesAdic"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbDEValor"
                                CssClass="style20" ErrorMessage="*" ValidationGroup="detallesAdic" Style="color: #FF0000"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="bAgregarDetalle" runat="server" Height="25px" OnClick="bAgregarDetalleAdic_Click"
                    Text="Agregar Det. Adic." ValidationGroup="detallesAdic" Width="119px" />
                <asp:GridView ID="gvDetAdic" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="SqlDataGridDatosAdicionales" ForeColor="#333333" DataKeyNames="idDetallesAdicionalesTemp"
                    GridLines="None" Width="69%">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="nombre" HeaderText="NOMBRE" SortExpression="nombre">
                            <HeaderStyle Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="valor" HeaderText="VALOR" SortExpression="valor" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('¿Desea eliminar el registro?');"
                                    CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#4580A8" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#DEE2EB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#6D95E1"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#E9EBEF"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#4870BE"></SortedDescendingHeaderStyle>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataGridDatosAdicionales" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    DeleteCommand="DELETE FROM DetallesAdicionalesTemp WHERE (idDetallesAdicionalesTemp = @idDetallesAdicionalesTemp)"
                    SelectCommand="SELECT nombre, valor, id_DetallesTemp, id_Empleado,idDetallesAdicionalesTemp FROM DetallesAdicionalesTemp WITH (NOLOCK) WHERE (id_DetallesTemp = @id_DetallesTemp)">
                    <DeleteParameters>
                        <asp:Parameter Name="idDetallesAdicionalesTemp" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlDetallesDestinatarios" Name="id_DetallesTemp"
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Label ID="lMsjDetallesAdic" runat="server" Style="color: #FF0000"></asp:Label>--%>
                <br />
                <br />
                <strong><span class="style11">
                    <asp:SqlDataSource ID="SqlDataLstDestinatarios" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        SelectCommand="SELECT idDestinatarioTemp, identificacionDestinatario + ', ' + razonSocialDestinatario + ', ' + dirDestinatario AS Destinatario, id_Empleado FROM DestinatariosTemp WITH (NOLOCK) WHERE (id_Empleado = @idUser)">
                        <SelectParameters>
                            <asp:SessionParameter Name="idUser" SessionField="idUser" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </span></strong>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Documento" ID="infoDocumentos">
                <div class="style5">
                    <span class="style4">Información de la Guía<br />
                    </span>
                </div>
                <table style="vertical-align: text-top">
                    <tr>
                        <td class="style33">
                            <asp:Label ID="lFechaIniTrasnporte" runat="server" Text="FECHA INICIAL DEL TRASNPORTE:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbFechaIniTransporte" runat="server" Height="19px" Width="185px"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:CalendarExtender ID="tbFechaIniTransporte_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="tbFechaIniTransporte" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="tbFechaIniTransporte"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                ControlToValidate="tbFechaIniTransporte" CssClass="style25" ErrorMessage="*"
                                Style="color: #FF0000" ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"
                                ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lFechaFinTrasnporte" runat="server" Text="FECHA  FINAL DEL TRASNPORTE"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbFechaFinTransporte" runat="server" Height="19px" Width="185px"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:CalendarExtender ID="tbFechaFinTransporte_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="tbFechaFinTransporte" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="tbFechaFinTransporte"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                ControlToValidate="tbFechaFinTransporte" CssClass="style25" ErrorMessage="*"
                                Style="color: #FF0000" ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"
                                ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style33">
                            <asp:Label ID="ldirEstablecimiento" runat="server" Text="ESTABLECIMIENTO:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirEstablecimiento" runat="server" Height="47px" Width="293px"
                                TextMode="MultiLine" ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="tbDirEstablecimiento"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lGuiaRemision0" runat="server" Text="DIRECCIÓN  DE PARTIDA:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbDirPartida" runat="server" Height="45px" TextMode="MultiLine"
                                Width="268px" ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="tbDirPartida"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="style33">
                            <asp:CheckBox ID="cbContribuyenteEspecial" runat="server" Text="CONTRIBUYENTE ESPECIAL" />
                            &nbsp;<asp:TextBox ID="tbContribuyenteEspecial" runat="server" MaxLength="4" Width="51px"></asp:TextBox>
                            <br />
                            <asp:CheckBox ID="cbObligado" runat="server" Text="OBLIGADO A CONTABILIDAD" Checked="True" />
                        </td>
                        <td rowspan="2">
                            RISE:<br />
                            <asp:TextBox ID="tbRise" runat="server" Height="19px" Width="185px"></asp:TextBox>
                            <br />
                            <br />
                            PLACA:<br />
                            <asp:TextBox ID="tbPlaca" runat="server" Height="19px" Width="86px" MaxLength="20"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="tbFechaIniTransporte"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="tbFechaEmision" runat="server" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style33">
                            <asp:Label ID="lidentificacionComprador" runat="server" Text="IDENTIFICACIÓN DEL TRANPORTISTA:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbIdentificacionComprador" runat="server" Height="19px" MaxLength="13"
                                Width="183px" AutoPostBack="True" OnTextChanged="tbIdentificacionComprador_TextChanged"
                                ValidationGroup="Receptor"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="tbIC_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                CompletionListCssClass="CompletionListCssClass" CompletionSetCount="12" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getRuc" ServicePath="../autoRuc.asmx" ShowOnlyCurrentWordInCompletionListItem="True"
                                TargetControlID="tbIdentificacionComprador" UseContextKey="True">
                            </asp:AutoCompleteExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="tbIdentificacionComprador"
                                ErrorMessage="Ingresa el RUC" CssClass="style35" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                                ControlToValidate="tbIdentificacionComprador" CssClass="style35" ErrorMessage="RUC Invalido"
                                ValidationExpression="^[0-9a-zA-Z]{0,13}?$" ValidationGroup="Receptor"></asp:RegularExpressionValidator>
                            
                                <Services>
                                    <asp:ServiceReference Path="../autoRuc.asmx" />
                                </Services>
                            
                            <br />
                            <br />
                            <asp:Label ID="lTipoIdentificacionComprador" runat="server" Text="TIPO DE INDENTIFICACIÓN:"></asp:Label>
                            :<br />&nbsp;&nbsp;<asp:DropDownList ID="ddlTipoIdentificacion" runat="server" DataSourceID="SqlDataTipoIdentificacion"
                                DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True">
                                <asp:ListItem>Selecciona el Tipo</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Label ID="lRazonSocialComprador" runat="server" Text="RAZÓN SOCIAL DEL TRANSPORTISTA:"></asp:Label>
                            <br />
                            <asp:TextBox ID="tbRazonSocialComprador" runat="server" Height="46px" Width="347px"
                                TextMode="MultiLine" ValidationGroup="Receptor"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="tbRazonSocialComprador"
                                ErrorMessage="*" Style="color: #FF0000" ValidationGroup="Receptor"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style24" colspan="2">
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
            </asp:WizardStep>
            <%--<asp:WizardStep runat="server" Title="Informacion Adicional" ID="infoAdicional">
            <table style="width:100%;">
                <tr>
                    <td class="style6" colspan="2">
                        <strong>Agregar Campo Adicional</strong></td>
                </tr>
                <tr>
                    <td class="style34">
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
