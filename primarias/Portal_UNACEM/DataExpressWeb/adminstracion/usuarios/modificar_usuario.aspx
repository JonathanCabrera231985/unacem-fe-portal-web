<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="modificar_usuario.aspx.cs" Inherits="Administracion.modificar_usuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
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
                            img.attr('src', '../../imagenes/minus.gif');
                        else
                            img.attr('src', '../../imagenes/plus.gif');
                    } catch (err) {
                        alert("erro segunda funcion:" + err.Message);
                    }
                });
            } catch (err) {
                alert("erro:" + err.Message);
            }
        });
        function validarContrasena() {
            var contrasena1 = document.getElementById('<%= tbContraseña.ClientID %>').value;
                    if (contrasena1.trim() === "") {
                        alert("El campo contraseña no puede estar vacío.");
                        return false; // evita el postback
                    }
                    return true; // permite el postback
                }
    </script>

    <style type="text/css">

        .style1
        {
            width: 100%;
        }
        .style4
        {
            height: 20px;
        }
        
        .style19
        {
            height: 20px;
            width: 153px;
            color: #000000;
            font-weight: bold;
            text-align: right;
        }
        .style21
        {
            color: #000000;
        }
        .style22
        {
            color: #000000;
            font-weight: bold;
            text-align: right;
            width: 153px;
        }
        .style23
        {
            text-align: right;
            width: 153px;
        }
        .style24
        {
            font-weight: bold;
            text-align: right;
            width: 153px;
        }
        .style16
        {}
        </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="style1">
    <tr class="style16">
        <td class="style18" colspan="2">
            <strong>MODIFICAR USUARIO</strong></td>
    </tr>
        <tr>
        <td class="style22">
            Nombre completo:</td>
        <td class="style13">
            <asp:TextBox ID="tbNombre" runat="server" Width="416px" CssClass="style16"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="tbNombre" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            <span class="style16">
        </td>
        </tr>
        <tr>
        <td class="style22">
            Username:</td>
        <td class="style13">
            <asp:TextBox ID="tbUsername" runat="server" MaxLength="15" Width="170px" 
                CssClass="style16" ReadOnly="True"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="tbUsername" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            <span class="style16">
        </td>
        </tr>
        <tr>
        <td class="style22">
            Contraseña:</td>
        <td class="style13">
            <asp:TextBox ID="tbContraseña" runat="server" Width="168px" CssClass="style16" TextMode="Password" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="tbContraseña" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            <span class="style16">
        </td>
        </tr>
          <tr>
        <td class="style22">
            <asp:Label ID="lEmail" runat="server" Text="Email:"></asp:Label>
        </td>
        <td class="style12">
            <asp:TextBox ID="tbEmail" runat="server" Height="59px" Width="372px"></asp:TextBox>
        </td>
    </tr>
        <tr>
        <td class="style22">
            Status:</td>
        <td class="style13">
            <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True" 
                CssClass="style16">
                <asp:ListItem Value="1">Activo</asp:ListItem>
                <asp:ListItem Value="0">Inactivo</asp:ListItem>
            </asp:DropDownList>
        </td>
        </tr>
        <tr>
        <td class="style24">
            <asp:Label ID="lRol" runat="server" Text="Rol:" Visible="False" 
                CssClass="style21"></asp:Label>
            </td>
        <td class="style13">
            <asp:DropDownList ID="ddlRol" runat="server" DataSourceID="SqlDataSource2" 
                DataTextField="descripcion" DataValueField="idRol" CssClass="style16">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT * FROM [Roles] WITH (NOLOCK)"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
        <td class="style24">
            <asp:Label ID="lSucursal" runat="server" Text="Sucursal" Visible="False" 
                CssClass="style21"></asp:Label>
            </td>
        <td class="style13">
            <asp:DropDownList ID="ddlSucursal" runat="server" DataSourceID="SqlDataSource3" Visible="false"
                DataTextField="Sucursal" DataValueField="idSucursal" CssClass="style16">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                
                SelectCommand="SELECT idSucursal, sucursal + ':' + clave AS Sucursal FROM Sucursales WITH (NOLOCK) "></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
        <td class="style22">
            &nbsp;</td>
        <td class="style13">
            <asp:DropDownList ID="ddlSesion" runat="server" DataSourceID="SqlDataSource1" 
                DataTextField="descripcion" DataValueField="idSesion" CssClass="style16" 
                Visible="False">
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
                SelectCommand="SELECT * FROM [Sesiones] WITH (NOLOCK)"></asp:SqlDataSource>
            </td>
        </tr>
        <tr class="style16">
        <td class="style19">
            &nbsp;</td>
        <td class="style13">
            &nbsp;</td>
        </tr>
        <tr>
        <td class="style23">
            </td>
        <td class="style4">
            <asp:Button ID="bModificar" runat="server" Text="Actualizar" 
                style="height: 26px" OnClientClick="return validarContrasena();" onclick="bModificar_Click1" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="bCancelar" runat="server" onclick="bCancelar_Click" 
                Text="Cancelar" />
        </td>
        </tr>
        </table>
        <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" Visible="false" AllowPaging="True" DataKeyNames="idSucursal"
    onrowdatabound="gvSucursales_RowDataBound" 
        onpageindexchanging="gvSucursales_PageIndexChanging" >
         <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <img alt="" src="../../imagenes/plus.gif"  idsucursal="<%# Eval("idSucursal") %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="clave" HeaderText="Cod Estab" SortExpression="clave" />
            <asp:BoundField DataField="sucursal" HeaderText="SUCURSAL" SortExpression="sucursal" />
            <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" SortExpression="domicilio" />
            <asp:TemplateField HeaderText="Marcar">
                <ItemTemplate>
                    <asp:CheckBox ID="check" runat="server" Checked='<%# Convert.ToBoolean(Eval("estado")) %>'/>
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
                                                    <asp:CheckBox ID="check" runat="server" Checked='<%# Convert.ToBoolean(Eval("estado")) %>' />
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
                SelectCommand="select Sucursales.idSucursal, clave,sucursal,domicilio,  case when isnull( empleado_Sucursal.idSucursal, '') = '' then 0 else 1 end estado  from Sucursales WITH (NOLOCK) 
left join empleado_Sucursal WITH (NOLOCK) on Sucursales.idSucursal = empleado_Sucursal.idSucursal and empleado_Sucursal.idEmpleado = @idUser where Sucursales.IDEEMI = @rucEmpresa and eliminado = 0">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="" SessionField="rucEmpresa" Name="rucEmpresa" Type="String" />
            <asp:SessionParameter DefaultValue=""  Name="idUser" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
