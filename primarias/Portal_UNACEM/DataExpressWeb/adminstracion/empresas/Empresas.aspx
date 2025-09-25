<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Empresas.aspx.cs" Inherits="DataExpressWeb.adminstracion.empresas.Empresas" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2
        {
            font-size: large;
        }
        .style7
        {
            width: 68%;
        }
        .style8
        {
        }
        .style9
        {
            width: 192px;
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
    "width=1000," +
    "height=500," +
    "left = 140," +
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

    </script>
    <div id="divBackground" style="position: absolute; top: 0px; left: 0px; background-color: black;
        z-index: 100; opacity: 0.8; filter: alpha(opacity=60); -moz-opacity: 0.8; overflow: hidden;
        display: none">
    </div>

   <center>
        <strong><span class="style2">EMPRESAS</span></strong>
   </center>
   <table class="style7">
        <tr>
            <td class="style9">
                RUC:<br />
                <asp:TextBox ID="tbRuc" runat="server" Width="163px"></asp:TextBox>
            </td>
                        <td>
                Razón Social:<br />
                <asp:TextBox ID="tbrazonSocial" runat="server" Width="590px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style8" colspan="2">
                Matríz:<br />
                <asp:TextBox ID="tbMatriz" runat="server" Width="783px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style9">
                <br />
                <asp:Button ID="bBuscarReg" runat="server" OnClick="bBuscarReg_Click" Text="Buscar" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="bActualizar" runat="server" OnClick="bActualizar_Click" Text="Actualizar" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
   </table>
    <br />
    <br />
    <asp:GridView runat="server" ID="gvDetalleEmpresa" DataSourceID="SqlDataSource1" AutoGenerateColumns="false" CellPadding="4" 
                            GridLines="Horizontal" Width="70%" BackColor="White" BorderColor="#336666" DataKeyNames="IDEEMI">
        <Columns>
             <asp:BoundField DataField="RFCEMI" HeaderText="RUC" SortExpression="RFCEMI" />
             <asp:BoundField DataField="NOMEMI" HeaderText="RAZÓN SOCIAL" SortExpression="NOMEMI" />
             <asp:BoundField DataField="nombreComercial" HeaderText="NOMBRE COMERCIAL" SortExpression="nombreComercial" />
             <asp:BoundField DataField="dirMatriz" HeaderText="MATRÍZ" SortExpression="dirMatriz" />
              <asp:TemplateField HeaderText="Configuracion" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                             <a href="javascript://" onclick="showModalPopUp('../empresas/PopConfigEmpresa.aspx?IdEmpresa=<%# Eval("IDEEMI") %>')">
                                <img src="../imagenes/web_search.png" alt="Consulta" border="0" align="middle" height="22"  width="22">
                             </a>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateField>
             <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <a href='../empresas/agregarEmpresa.aspx?IdEmpresa=<%# Eval("IDEEMI") %>'>Editar</a>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Esta seguro que desea eliminar la empresa');"
                        CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle Width="100px" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <HeaderStyle BackColor="#E1DDD9" Font-Bold="True" ForeColor="#002f5a" Font-Size="Small" />
        <PagerStyle BackColor="#010E9D" ForeColor="#0066FF" HorizontalAlign="Center" />
        <RowStyle BackColor="White" ForeColor="#333333" />
        <SelectedRowStyle BackColor="Silver" ForeColor="#6699FF" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#F7F7F7" />
        <SortedAscendingHeaderStyle BackColor="#487575" />
        <SortedDescendingCellStyle BackColor="#E5E5E5" ForeColor="#0066FF" />
        <SortedDescendingHeaderStyle BackColor="#275353" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="PA_Empresa_AMC" DeleteCommand="update EMISOR set Estado = 1 where IDEEMI = @IDEEMI"
        SelectCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="IDEEMI" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="<INSTRUCCION><Filtro><Opcion>3</Opcion><IDEEMI></IDEEMI><RFCEMI></RFCEMI><NOMEMI></NOMEMI><dirMatriz></dirMatriz></Filtro></INSTRUCCION>" Name="documentoXML" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
 </asp:Content>