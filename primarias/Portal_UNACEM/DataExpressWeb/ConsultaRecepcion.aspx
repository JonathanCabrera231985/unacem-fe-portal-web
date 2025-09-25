<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"  ValidateRequest="false" AutoEventWireup="true" CodeBehind="ConsultaRecepcion.aspx.cs" Inherits="DataExpressWeb.ConsultaRecepcion" %>

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
        function UploadFile() {
            var files = $("#<%=fuRetenciones.ClientID%>").get(0).files;
            counter = 0;

            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                var formdata = new FormData();
                formdata.append("fuRetenciones", file);
                var ajax = new XMLHttpRequest();
                ajax.upload.addEventListener("progress", progressHandler, false);
                ajax.addEventListener("load", completeHandler, false);
                ajax.addEventListener("error", errorHandler, false);
                ajax.addEventListener("abort", abortHandler, false);
                ajax.open("POST", "Fileuploadhandler .ashx");
                ajax.send(formdata);
            }
        }
        function progressHandler(event) {
            $("#loaded_n_total").html("Cargado " + event.loaded + " bytes de " + event.total);
            var percent = (event.loaded / event.total) * 100;
            $("#progressBar").val(Math.round(percent));
            $("#status").html(Math.round(percent) + "% cargando... por favor espere");
        }

        function completeHandler(event) {
            counter++
            $("#status").html(counter + " " + event.target.responseText);
        }

        function errorHandler(event) {
            $("#status").html("Carga Fallida");
        }

        function abortHandler(event) {
            $("#status").html("Carga Abortada");
        }
    </script>
    <div id="divBackground" style="position: absolute; top: 0px; left: 0px; background-color: black;
        z-index: 100; opacity: 0.8; filter: alpha(opacity=60); -moz-opacity: 0.8; overflow: hidden;
        display: none">
    </div>
    <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
        vertical-align: top;">
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: medium; text-align: center"><span class="style16">Buscador</span> </td>
        </tr>
        <tr>
            <td class="style1" colspan="2">&nbsp;</td>
            <td class="style1" colspan="3">
                <br class="style9" /></td>
        </tr>
        <tr valign="top">
            <td class="style7" rowspan="2"><strong>No.Documento(s):</strong><br />
                <asp:TextBox ID="tbFolioAnterior" runat="server" Width="151px" 
                    CssClass="txt_gr2"></asp:TextBox>
                &nbsp;<span class="style13">Formato(1,2,3,4-8)</span><br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato Incorrecto"
                    ForeColor="Red" ControlToValidate="tbFolioAnterior" ValidationExpression="((\d+|([,]\d+))+([-]\d)*)*"></asp:RegularExpressionValidator>
            </td>
            <td rowspan="2" colspan="2"><strong>Tipo:</strong><br />
                <asp:DropDownList ID="ddlTipoDocumento" runat="server" DataSourceID="SqlDataSourceTipoDoc"
                    DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                    AutoPostBack="True" OnSelectedIndexChanged="CambiarTipo">
                    <asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                <br />
                <br />
                <br /></td>
            <td><strong>Fecha Inicial:</strong><br />
                <asp:TextBox ID="tbFechaInicial" runat="server" CssClass="txt_gr2"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaInicial_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaInicial" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
            <td class="style17"><strong>Fecha Final:<br /></strong>
                <asp:TextBox ID="tbFechaFinal" runat="server" CssClass="txt_gr2"></asp:TextBox>
                <asp:CalendarExtender ID="tbFechaFinal_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="tbFechaFinal" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="1">
                <%--<asp:Panel ID="PanelRetenciones" runat="server" Visible="false">
                    <asp:FileUpload ID="fuRetenciones" runat="server" AllowMultiple="true" accept=".xml"/>
                    <asp:Button ID="BtnRete2"  OnClientClick="UploadFile()" runat="server" Text="Cargar Rt2.0" Width="87px" Style="height: 26px" CssClass="botones" />
                    <progress id="progressBar" value="0" max="100" style="width: 300px;"></progress>
                    <h3 id="status"></h3>
                    <p id="loaded_n_total"></p>
                </asp:Panel>--%>
            </td>
            <td class="style12" colspan="2" align="center">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;           
                 </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Buscar" Width="87px"
                    Style="height: 26px" CssClass="botones" />
                &nbsp;&nbsp;
            </td>
            <td class="style7" colspan="2">
                <asp:Panel ID="PanelRetenciones" runat="server" Visible="false">
                    <asp:FileUpload ID="fuRetenciones" runat="server" AllowMultiple="true" accept=".xml"/>
                    <asp:Button ID="BtnRete2"  OnClientClick="UploadFile()" runat="server" Text="Cargar Rt2.0" Width="87px" Style="height: 26px" CssClass="botones" />
                    <progress id="progressBar" value="0" max="100" style="width: 300px;"></progress>
                    <h3 id="status"></h3>
                    <p id="loaded_n_total">
                    </p>
                </asp:Panel>
            </td>
            <td class="style2" colspan="4">
                <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                <br /></td>
        </tr>
       
        <tr>
            <td colspan="5" bgcolor="#5F6062" style="font-weight: 700; font-size: xx-small; text-align: center">&nbsp;
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:DataFilter1 ID="DataFilter11" runat="server" />
            <asp:GridView  ID="gvFacturas" DataSourceID="SqlDataSource1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                GridLines="Horizontal" Width="80%" BackColor="White"
                BorderColor="#DEDFDE" BorderWidth="3px" BorderStyle="Double" Style="margin-top: 0px"                
                AllowPaging="True" AllowSorting="True" OnPageIndexChanged="gvFacturas_PageIndexChanged"
                OnSelectedIndexChanged="gvFacturas_SelectedIndexChanged"
                OnSorting="gvFacturas_SelectedIndexChanged" PageSize="15" >
                <Columns>
                    <asp:TemplateField HeaderText="DOCUMENTO" SortExpression="descripcion">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("descripcion") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="numero" HeaderText="NÚMERO" InsertVisible="False" ReadOnly="True"
                        SortExpression="numero" HeaderStyle-Width="10%">
                    <HeaderStyle Width="10%" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="FECHA RECEPCION"  SortExpression="FECHA" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" text='<%# Bind("FECHA") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" text='<%# Bind("FECHA") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ESTADO" SortExpression="ESTADO">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ESTADO") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ESTADO") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="XML" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href='download.aspx?file=<%# Encrypt(Eval("idlog").ToString()) %>'>
                            <img src="imagenes/xml32x32.png" alt="xml" border="0" align="middle" height="22"
                                    width="22"></a>
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
                <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign="Center" Font-Underline="True" BorderColor="#003300" />
                <RowStyle BackColor="#DEE2EB" />
                <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                <SortedAscendingHeaderStyle BackColor="#848384" />
                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                <SortedDescendingHeaderStyle BackColor="#575357" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="LecturaWebServices" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter DefaultValue="null" Name="consulta" Type="String" />
                    <asp:Parameter DefaultValue="null" Name="fechaconsultaInicio" Type="String" />
                    <asp:Parameter DefaultValue="null" Name="fechaconsultaFinal" Type="String" />
                    <asp:Parameter DefaultValue="null" Name="tipodocumento" Type="String" />
                    <asp:Parameter DefaultValue="-" Name="MYWHERE" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceTipoDoc" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WHERE (tipo = 'Comprobante') and codigo IN ('01','03','04','05','06','07', '072')"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
