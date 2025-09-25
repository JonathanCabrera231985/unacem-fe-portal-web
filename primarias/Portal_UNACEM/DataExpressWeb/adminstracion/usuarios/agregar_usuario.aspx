<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="agregar_usuario.aspx.cs" Inherits="Administracion.agregar_usuario
" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            try {
                $('#<%=gvSucursales.ClientID %> img').click(function () {
                    try {
                        var img = $(this)
                        var idsucursal = $(this).attr('idsucursal');

                        var tr = $('#<%=gvSucursales.ClientID %> tr[idsucursal =' + idsucursal + ']')
                        tr.toggle();

                        if (tr.is(':visible'))
                            img.attr('src', 'minus.gif');
                        else
                            img.attr('src', 'plus.gif');
                    } catch (err) {
                        alert("erro segunda funcion:" + err.Message);
                    }
                });
            } catch (err) {
                alert("erro:" + err.Message);
            }
        });

        function checkDocumento(control, grid) {
            try {
                var checked = control.checked;
                alert(checked); //do your stuff here
                var GView = $('#<%=gvSucursales.ClientID %>');
                alert("2"); //do your stuff here
                if (GView != null) {
                    var totalLineas = GView.rows.length - 1
                    alert("4"); //do your stuff here
                    for (var r = 1; r <= totalLineas; r++) {
                        alert(GView.rows[r].cells[0].innerHTML);
                    }
                }
                alert("3"); //do your stuff here
            } catch (err) {
                alert("erro:" + err.Message);
            }
        }

    </script>
    <style type="text/css">
        .style1
        {
            width: 54%;
        }
        .style3
        {
            width: 124px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: x-small;
            font-weight: bold;
        }
        .style4
        {
            width: 124px;
            font-size: x-small;
            font-family: Arial, Helvetica, sans-serif;
        }
        .style5
    {
        width: 124px;
        font-size: x-small;
        font-family: Arial, Helvetica, sans-serif;
        height: 21px;
    }
    .style6
    {
        height: 21px;
        width: 518px;
    }
        .style7
        {
            width: 124px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: x-small;
            font-weight: bold;
            height: 29px;
            color: #000000;
        }
        .style8
        {
        height: 29px;
        width: 518px;
    }
        .style9
        {
            width: 266px;
            height: 52px;
            color: #000000;
        }
        .style10
        {
            color: #000000;
        }
        .style11
        {
            width: 124px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: x-small;
            font-weight: bold;
            color: #000000;
        }
        .style12
    {
        width: 518px;
    }
        .style13
        {
            font-size: large;
        }
        .style14
        {
            color: #000000;
            font-weight: bold;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center><strong><span class="style13">AGREGAR USUARIO</span></strong></center><br />
    <table class="style1">
    <tr runat="server" id="ColumnSucursal" visible="true">
        <td class="style7">
            Tipo de Usuario: </td>
        <td class="style8">
            <asp:DropDownList ID="ddlTipoUsuario" runat="server" AutoPostBack="True" 
                onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem Value="0">-- Seleccionar Usuario --</asp:ListItem>
                <asp:ListItem Value="1">Empleado</asp:ListItem>
                <asp:ListItem Value="2">Cliente</asp:ListItem>
            </asp:DropDownList>
            <br />
        </td>
    </tr>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <tr id="columnEmpresa" runat="server" visible="false">
        <td class="style7">
            Empresa: </td>
        <td class="style8">
            <asp:DropDownList ID="cboEmpresa" runat="server" DataSourceID="SqlDataSourceEmpresa"
                        DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="True"
                        AutoPostBack="True" onselectedindexchanged="cboEmpresaList1_SelectedIndexChanged">
                <asp:ListItem Value="0">-- Seleccionar Empresa --</asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceEmpresa" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                        SelectCommand="SELECT IDEEMI codigo, NOMEMI descripcion  FROM EMISOR WHERE (Estado = 0)">
            </asp:SqlDataSource>
            <br />
        </td>
    </tr>
    <tr runat="server" id="Tr1" visible="false" >
        <td class="style4">
            <asp:Label ID="Label5" runat="server" Text="Sucursal:" CssClass="style14"></asp:Label>
        </td>
        <td class="style12">
            <asp:DropDownList ID="DropDownList1" runat="server" >
                <asp:ListItem Value="0">-- Seleccionar Empresa --</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
        </ContentTemplate>
    </asp:UpdatePanel> 
        <tr>
        <td class="style11">
            <asp:Label ID="lRfc" runat="server" Text="RUC:"></asp:Label>
            </td>
        <td class="style12">
            <asp:TextBox ID="tbRFC" runat="server" ontextchanged="tbRFC_TextChanged" AutoPostBack="true"></asp:TextBox>
            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                ErrorMessage="*" ControlToValidate="tbRFC" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
        </tr>
        <tr>
        <td class="style11">
            Nombre o Razón Social:</td>
        <td class="style12">
            <asp:TextBox ID="tbNombre" runat="server" Width="352px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ControlToValidate="tbNombre" ErrorMessage="Requiere nombre" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
        </tr>
    <tr>
        <td class="style7">
            Nombre usuario:</td>
        <td class="style8">
            <asp:TextBox ID="tbUsername" runat="server" MaxLength="15" Width="170px" 
                ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="tbUsername" ErrorMessage="Requiere username" ForeColor="Red">*</asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lNombre" runat="server" ForeColor="Black" 
                style="font-size: x-small; font-style: italic; color: #808080;"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style11">
            Contraseña:</td>
        <td class="style12">
            <asp:TextBox ID="tbContrasena" runat="server" TextMode="Password" Width="168px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="tbContrasena" ErrorMessage="Requiere contraseña" 
                ForeColor="Red">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="style11">
            Repetir contraseña:</td>
        <td class="style12">
            <asp:TextBox ID="tbRepetir" runat="server" TextMode="Password" Width="168px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ControlToValidate="tbRepetir" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                ControlToCompare="tbContrasena" ControlToValidate="tbRepetir" 
                ErrorMessage="Contraseña no es igual" ForeColor="Red">*</asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <td class="style11">
            Status:</td>
        <td class="style12">
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Value="1">---Activo---</asp:ListItem>
                <asp:ListItem Value="0">---Inactivo---</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="style3">
            <asp:Label ID="lRol" runat="server" Text="Rol:" CssClass="style10"></asp:Label>
        </td>
        <td class="style12">
            <asp:DropDownList ID="ddlRol" runat="server" DataSourceID="SqlDataSourceRol" 
                DataTextField="descripcion" DataValueField="idRol">
                <asp:ListItem Value="0">Selecciona el Rol</asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceRol" runat="server" 
                ConnectionString="<%$ ConnectionStrings:upsdataConnectionString %>" 
                SelectCommand="SELECT [idRol], [descripcion] FROM [Roles] WHERE eliminado ='False'">
            </asp:SqlDataSource>
        </td>
    </tr>
    <tr>
        <td class="style5">
            <asp:Label ID="lSesion" runat="server" Text="Sesion" CssClass="style14" 
                Visible="False"></asp:Label>
        </td>
        <td class="style6">
            <asp:DropDownList ID="ddlSesion" runat="server" 
                DataSourceID="SqlDataSourceSesion" DataTextField="descripcion" 
                DataValueField="idSesion" Visible="False" Height="16px" Width="87px">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceSesion" runat="server" 
                ConnectionString="<%$ ConnectionStrings:upsdataConnectionString %>" 
                SelectCommand="SELECT [descripcion], [idSesion] FROM [Sesiones] WHERE eliminado='False'">
            </asp:SqlDataSource>
        </td>
    </tr>
    <tr>
        <td class="style4">
            <asp:Label ID="lSucursal" runat="server" Text="Sucursal:" CssClass="style14" Visible="false"></asp:Label>
        </td>
        <td class="style12">
            <asp:DropDownList ID="ddlSucursal" runat="server" Visible="false"
                DataSourceID="SqlDataSourceSucursal" DataTextField="Sucursal" 
                DataValueField="idSucursal">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSourceSucursal" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT idSucursal, sucursal + ':' + clave AS Sucursal FROM Sucursales WHERE eliminado='False' and IDEEMI = @IDEEMI ">
                <SelectParameters>
                    <asp:SessionParameter Name="IDEEMI" SessionField="rucEmpresa" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
    </tr>
    <tr>
        <td class="style4">
            <asp:Label ID="lEmail" runat="server" Text="Email:" CssClass="style14"></asp:Label>
        </td>
        <td class="style12">
            <asp:TextBox ID="tbEmail" runat="server" Height="59px" Width="372px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style9">
            &nbsp;</td>
        <td class="style12">
            <asp:Button ID="bGuardar" runat="server" Text="Guardar" 
                onclick="btnGuardar_Click"  />
        &nbsp;<asp:Label ID="lMsj" runat="server" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style9">
            &nbsp;</td>
        <td class="style12">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
        </td>
    </tr>
</table>
<asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" 
        CellPadding="5" Visible="false" AllowPaging="True" DataKeyNames="idSucursal"
    onrowdatabound="gvSucursales_RowDataBound" AllowSorting="True" GridLines="Horizontal"
        onpageindexchanging="gvSucursales_PageIndexChanging" >
         <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt="" src="plus.gif"  idsucursal="<%# Eval("idSucursal") %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="clave" HeaderText="Cod Estab" SortExpression="clave" />
                <asp:BoundField DataField="sucursal" HeaderText="SUCURSAL" SortExpression="sucursal" />
                <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" SortExpression="domicilio" />
                <asp:TemplateField HeaderText="Marcar">
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server"  />
                            <asp:HiddenField ID="checkLinea" runat="server" Value='<%#Eval("idSucursal")%>' />
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                        <ItemStyle HorizontalAlign="Center" />
                  </asp:TemplateField>
                    <asp:TemplateField>
                         <ItemTemplate>
                            <tr style="display:none;" idsucursal="<%# Eval("idSucursal") %>">
                            <td colspan="100%">
                                <div style="position:relative;left:25px;">
                                    <asp:GridView ID="gvPuntoEmision" runat="server" AutoGenerateColumns="False" CellPadding="4"  DataKeyNames= "idCaja">
                                        <Columns>
                                            <asp:BoundField DataField="idCaja" HeaderText="Cod Punto Emision" SortExpression="idCaja" />
                                            <asp:BoundField DataField="ptoEmi" HeaderText="Punto Emision" SortExpression="ptoEmi" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Comprobante" SortExpression="descripcion" />
                                              <asp:TemplateField HeaderText="Marcar">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="check" runat="server" />
                                                        <asp:HiddenField ID="checkCaja" runat="server" Value='<%#Eval("idCaja")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <HeaderStyle BackColor="#012D5C" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign= "Center" />
                                        <RowStyle BackColor="#DEE2EB" />
                                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
                                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                        <SortedAscendingHeaderStyle BackColor="#848384" />
                                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                        <SortedDescendingHeaderStyle BackColor="#575357" />
                                    </asp:GridView>
                                 </div>
                            </td>
                        </tr>
                         </ItemTemplate>
                    </asp:TemplateField>
         </Columns>
        <EmptyDataTemplate>
            No existen datos.
        </EmptyDataTemplate> 
        <FooterStyle BackColor="#CCCC99" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign= "Center" />
        <RowStyle BackColor="#DEE2EB" />
        <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#FBFBF2" />
        <SortedAscendingHeaderStyle BackColor="#848384" />
        <SortedDescendingCellStyle BackColor="#EAEAD3" />
        <SortedDescendingHeaderStyle BackColor="#575357" />
</asp:GridView>
 <asp:SqlDataSource ID="SqlDataSourceLineas" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="select * from Sucursales where eliminado = 0 and IDEEMI = @rucEmpresa ">
        <SelectParameters>
            
            <asp:Parameter DefaultValue="" Name="rucEmpresa" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSourcePuntoEmision" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT A.idCaja, A.idSucursal, b.descripcion, A.ptoEmi FROM dbo.CajaSucursal A INNER JOIN dbo.Catalogo1_C B ON A.NumeroRentas = B.codigo AND b.tipo = 'Comprobante' where A.idSucursal = @idSucursal ">
        <SelectParameters>
            <asp:Parameter DefaultValue="" Name="idSucursal" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>