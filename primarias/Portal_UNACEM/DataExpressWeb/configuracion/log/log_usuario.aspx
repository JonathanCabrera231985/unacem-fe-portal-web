<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log_usuario.aspx.cs" Inherits="DataExpressWeb.configuracion.log.log_usuario" MasterPageFile="~/Site.Master"  %>


   <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
        vertical-align: top;" align="center">
    <tr>
    <td style="font-weight: 700; font-size: medium; text-align: center">
                Log de Usuario
            </td>
    </tr>
    <tr align="center">
    
    <td align="center">
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" BackColor="White" 
            BorderColor="#CCCCCC" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" DataKeyNames="idErrorFactura" DataSourceID="SqlDataSource1"
                    Height="16px" Width="900px" style="text-align: left">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" InsertVisible="False" SortExpression="idErrorFactura">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("idErrorFactura") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("idErrorFactura") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" />
                        <asp:BoundField DataField="descripcion" HeaderText="Comprobante" SortExpression="descripcion" />
                        <asp:BoundField DataField="Documento" HeaderText="N° Documento" SortExpression="Documento" />
                        <asp:BoundField DataField="numeroDocumento" HeaderText="N° Control" SortExpression="numeroDocumento" />
                        <asp:BoundField DataField="detalle" HeaderText="Detalle" SortExpression="detalle" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
                    SelectCommand="SELECT top 10  A.idErrorFactura, A.detalle, A.fecha, B.descripcion, Substring(SUBSTRING(A.numeroDocumento,(select case when len(A.numeroDocumento) = 42 then 16 else 3 end ),15),1,3) + '-'
							+ Substring(SUBSTRING(A.numeroDocumento,(select case when len(A.numeroDocumento) = 42 then 16 else 3 end ),15),4,3)  + '-'
							+ Substring(SUBSTRING(A.numeroDocumento,(select case when len(A.numeroDocumento) = 42 then 16 else 3 end ),15),7,15) Documento
							 , A.numeroDocumento FROM [LogErrorFacturas] A
							left join Catalogo1_C B on B.codigo = Substring(SUBSTRING(A.numeroDocumento,(select case when len(A.numeroDocumento) = 42 then 14 else 3 end ),15),1,2)   and B.tipo = @tipoComprobante
							 WHERE (A.tipo = @tipo)  order by fecha desc">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Usuario" Name="tipo" Type="String" />
                        <asp:Parameter DefaultValue="Comprobante" Name="tipoComprobante" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
    </td>
    </tr>
    </table>
    
    </asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    </asp:Content>


