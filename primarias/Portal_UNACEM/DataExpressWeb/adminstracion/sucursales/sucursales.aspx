<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="sucursales.aspx.cs" Inherits="Administracion.sucursales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


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
    <script  type="text/javascript" language="javascript">
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

        return true;
    }
	  </script>
    <center>
        <strong><span class="style2">SUCURSALES</span></strong></center>
    <table class="style7">
        <tr>
            <td class="style9">
                Clave:<br />
                <asp:TextBox ID="tbClave" runat="server" Width="163px" CssClass="txt_gr2"></asp:TextBox>
            </td>
            <td>
                Sucursal:<br />
                <asp:TextBox ID="tbSucursal" runat="server" Width="269px" CssClass="txt_gr2"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style8" colspan="2">
                Domicilio:<br />
                <asp:TextBox ID="tbDomicilio" runat="server" Width="611px" CssClass="txt_gr2"></asp:TextBox>
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
    <asp:Label ID="lMensaje" runat="server" Style="color: #FF0000"></asp:Label>
    <br />
    <br />
    <asp:GridView ID="gvSucursales" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataKeyNames="idSucursal" DataSourceID="SqlDataSource1" GridLines="Horizontal"
        Width="70%" BackColor="White" BorderColor="#336666" BorderWidth="3px" BorderStyle="Double"
        AllowPaging="True" OnSelectedIndexChanged="grid_sucursales_SelectedIndexChanged"
        OnPageIndexChanged="grid_sucursales_PageIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/imagenes/icono_adelante.gif">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle CssClass="p_gridview_h" />
            </asp:CommandField>
            <asp:BoundField DataField="clave" HeaderText="CLAVE">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="sucursal" HeaderText="SUCURSAL" SortExpression="sucursal" />
            <asp:BoundField DataField="domicilio" HeaderText="DOMICILIO" SortExpression="domicilio" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <a href='modificarSucursal.aspx?id=<%# Eval("idSucursal") %>'>Editar</a>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('Si eliminas la sucursal se borraran todos los datos que contengan la sucursal. ¿Desea eliminar el registro?');"
                        CausesValidation="False" CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle Width="100px" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" Font-Size="Small" />
        <PagerStyle BackColor="#5F6062" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="White" ForeColor="#333333" />
        <SelectedRowStyle BackColor="Silver" ForeColor="White" Font-Bold="True" />
        <SortedAscendingCellStyle BackColor="#F7F7F7" />
        <SortedAscendingHeaderStyle BackColor="#487575" />
        <SortedDescendingCellStyle BackColor="#E5E5E5" />
        <SortedDescendingHeaderStyle BackColor="#275353" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="PA_Busqueda_Sucursales" DeleteCommand="UPDATE Sucursales SET eliminado='True' WHERE (idSucursal = @idSucursal) UPDATE Empleados SET eliminado='true' WHERE (id_Sucursal=@idSucursal)"
        SelectCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="idSucursal" />
        </DeleteParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="-" Name="QUERY" Type="String" />
            <asp:SessionParameter DefaultValue="" SessionField="rucEmpresa" Name="idEmpresa" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
    <br />
      <asp:Panel ID="Panel_cajaSucursal" Style="padding-top: 10px" runat="server" Width="100%">
        <asp:Label ID="Label1" runat="server" Text="Administrador de Cajas por Sucursal"
            ForeColor="#3983B7" CssClass="titulo_pagina"></asp:Label>
        
        <asp:GridView ID="gv_cajaSucursal" runat="server" AutoGenerateColumns="False" DataKeyNames="idCaja"
            DataSourceID="ds_CajaSucursal" OnDataBinding="grid_cajaSucursal_DataBinding"
            BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px"
            CellPadding="4" GridLines="Horizontal" Width="70%" ShowFooter="True" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:TemplateField HeaderText="Caja Local" SortExpression="NumeroSisposnet">
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_numsispo" runat="server" ValidationGroup="gvinsert" onkeypress="return isNumberKey(event)"
                            MaxLength="2" Visible="False" CssClass="cajaEditGrid" CausesValidation="True" Width="30px"></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("NumeroSisposnet") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtnumsis" runat="server" Text='<%# Bind("NumeroSisposnet") %>' ValidationGroup="gvupdate" onkeypress="return isNumberKey(event)" 
                        MaxLength="2" CssClass="cajaEditGrid" CausesValidation="True" Width="30px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo de Documento" SortExpression="NumeroRentas">
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_nrentas" CssClass="cajaEditGrid" runat="server" SelectedValue='<%# Bind("NumeroRentas") %>'
                            Visible="False" DataSourceID="DataSourceTipoDocument" DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("NumeroRentas") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                       <asp:DropDownList ID="ddlNumeroRentas" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("NumeroRentas") %>'
                        DataSourceID="DataSourceTipoDocument" DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Establecimiento" SortExpression="estab">
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_estab" runat="server" MaxLength="3" ValidationGroup="gvinsert" onkeypress="return isNumberKey(event)"
                           Visible="False" CssClass="cajaEditGrid" CausesValidation="True" Width="35px"></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("estab") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtestab" runat="server" Text='<%# Bind("estab") %>' CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)"
                            ValidationGroup="gvupdate" MaxLength="3" CausesValidation="true" Width="35px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Punto Emisión" SortExpression="ptoEmi">
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_ptoemi" runat="server" MaxLength="3" onkeypress="return isNumberKey(event)"
                            Visible="False" CssClass="cajaEditGrid" CausesValidation="true" Width="35px"></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ptoEmi") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtptoEmi" runat="server" Text='<%# Bind("ptoEmi") %>' CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)"
                            CausesValidation="True" ValidationGroup="gvupdate" MaxLength="3" Width="35px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Secuencial" SortExpression="secuencial">
                    <FooterTemplate>
                        <asp:TextBox ID="txt_i_secuencial" runat="server" MaxLength="9" onkeypress="return isNumberKey(event)"
                            Visible="False" CssClass="cajaEditGrid" CausesValidation="True" Width="70px"></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("secuencial") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtsecuencial" runat="server" Text='<%# Bind("secuencial") %>' CssClass="cajaEditGrid" onkeypress="return isNumberKey(event)"
                             AutoPostBack="True" ValidationGroup="gvupdate" MaxLength="9" Width="70px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado" SortExpression="estado">
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_estado" runat="server" Visible="False" CssClass="cajaEditGrid"
                             DataSourceID="DataSourceEstado"  DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("estado") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlestado" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("estado") %>'
                         DataSourceID="DataSourceEstado"  DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Factura Electrónica" SortExpression="estadoFE">
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_estadofe" runat="server" ValidationGroup="insert" Visible="False" CssClass="cajaEditGrid"
                        DataSourceID="DataSourceEstado"  DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("estadoFE") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlestadofe" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("estadoFE") %>'
                            DataSourceID="DataSourceEstado"  DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Emisión" SortExpression="estadoPro">
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_i_estadopro" runat="server" ValidationGroup="gvinsert"
                            Visible="False" CssClass="cajaEditGrid" DataSourceID="DataSourceTipoEmision"  DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("estadoPro") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlestadopro" runat="server" CssClass="cajaEditGrid" SelectedValue='<%# Bind("estadoPro") %>'
                            DataSourceID="DataSourceTipoEmision"  DataTextField="descripcion" DataValueField="codigo" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <FooterTemplate>
                        <asp:LinkButton ID="lbnuevo" runat="server" CausesValidation="False" OnClick="lbnuevo_Click">Nuevo</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lbcancel" runat="server" CausesValidation="False" OnClick="lbcancel_Click"
                            Visible="False">Cancelar</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lbinsert" runat="server" OnClick="lbinsert_Click" ValidationGroup="gvinsert"
                            Visible="False">Insertar</asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="Editar"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbactualizar" runat="server" CausesValidation="True" CommandName="Update" ValidationGroup="gvupdate"
                            Text="Actualizar"></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancelar"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle CssClass="cajaEditGrid" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                &nbsp;<asp:Button ID="btnCajas" runat="server" Text="Agregar Cajas" OnClick="btnCajas_Click" />
            </EmptyDataTemplate>
            <FooterStyle CssClass="cajaEditGrid" HorizontalAlign="Center" />
            <HeaderStyle CssClass="p_gridview_h" />
            <RowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="DataSourceTipoDocument" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
            SelectCommand="select  SUBSTRING(UPPER (descripcion), 1, 1) + SUbSTRING (LOWER (descripcion), 2,LEN(descripcion))  descripcion, codigo from Catalogo1_C where tipo = 'Comprobante'">
        </asp:SqlDataSource>
         <asp:SqlDataSource runat="server" ID="DataSourceTipoEmision" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
            SelectCommand="select SUBSTRING(UPPER (descripcion), 1, 1) + SUbSTRING (LOWER (descripcion), 2,LEN(descripcion)) descripcion, codigo from Catalogo1_C where tipo = 'Ambiente'">
        </asp:SqlDataSource>
        <asp:SqlDataSource runat="server" ID="DataSourceEstado" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
            SelectCommand="select 'Activo' descripcion, 'A' codigo union select 'Inactivo' descripcion, 'I' codigo ">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="ds_CajaSucursal" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
            DeleteCommand="UPDATE CajaSucursal SET estado = 'I', estadoFE = 'I', estadoPro = '1' WHERE (idCaja = @idCaja)"
            InsertCommand="INSERT INTO CajaSucursal(idSucursal, NumeroSisposnet, NumeroRentas, estab, ptoEmi, secuencial, estado, estadoFE, estadoPro) VALUES (@idSucursal, @NumeroSisposnet, @NumeroRentas, @estab, @ptoEmi, @secuencial, @estado, @estadoFE, @estadoPro)"
            SelectCommand="SELECT idCaja, idSucursal, NumeroSisposnet, NumeroRentas, estab, ptoEmi, secuencial, estado, estadoFE, estadoPro FROM CajaSucursal WHERE (idSucursal = @idSucursal)"
            UpdateCommand="UPDATE CajaSucursal SET NumeroSisposnet = @NumeroSisposnet, NumeroRentas = @NumeroRentas, estab = @estab, ptoEmi = @ptoEmi, secuencial = @secuencial, estado = @estado, estadoFE = @estadoFE, estadoPro = @estadoPro WHERE (idCaja = @idCaja)">
            <DeleteParameters>
                <asp:Parameter Name="idCaja" />
            </DeleteParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="gvSucursales" Name="idSucursal" PropertyName="SelectedValue" />
            </InsertParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="gvSucursales" Name="idSucursal" PropertyName="SelectedValue" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="NumeroSisposnet" />
                <asp:Parameter Name="NumeroRentas" />
                <asp:Parameter Name="estab" />
                <asp:Parameter Name="ptoEmi" />
                <asp:Parameter Name="secuencial" />
                <asp:Parameter Name="estado" />
                <asp:Parameter Name="estadoFE" />
                <asp:Parameter Name="estadoPro" />
                <asp:Parameter Name="idCaja" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </asp:Panel>
</asp:Content>
