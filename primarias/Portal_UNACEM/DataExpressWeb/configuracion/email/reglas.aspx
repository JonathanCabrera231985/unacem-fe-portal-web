<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reglas.aspx.cs" Inherits="DataExpressWeb.distribucion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <strong><span class="style2">Reglas envio de correos</span></strong></center>
            <br />
            <table style="border-style: none; border-color: inherit; border-width: 1px; width: 691px;
                vertical-align: top;">
                <tr>
                    <td class="style3">
                        CLIENTE:
                        <br />
                        <asp:TextBox ID="txt_identificacion" runat="server" CssClass="txt_gr1" Width="250px"></asp:TextBox>
                    </td>
                    <td class="style3">
                        Nombre Regla:
                        <br />
                        <asp:TextBox ID="txt_nombre" runat="server" CssClass="txt_gr1" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3" colspan="2">
                        <asp:Button runat="server" ID="btoBuscar" Text="Buscar" 
                            OnClick="btoBuscar_Click" CssClass="botones" />&nbsp;
                        <asp:Button runat="server" ID="btoActualizar" Text="Actualizar" 
                            OnClick="btoActualizar_Click" CssClass="botones" />
                    </td>
                </tr>
            </table>
            <br />    
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="idEmailRegla" DataSourceID="reglasDataSource" 
        CellPadding="4" ForeColor="#333333" GridLines="None" Width="682px" AllowPaging="True" PageSize="15">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="nombreRegla" HeaderText="NOMBRE" 
                SortExpression="nombreRegla" />
            <asp:BoundField DataField="NOMREC" HeaderText="CLIENTE" 
                SortExpression="NOMREC" />
            <asp:CheckBoxField DataField="estadoRegla" HeaderText="ACTIVO" 
                SortExpression="estadoRegla" >
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:TemplateField HeaderText="ACCIÓN" ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Actualizar"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Cancel" Text="Cancelar"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                                <a href='modReglas.aspx?regladi=<%# Eval("idEmailRegla") %>'" >Editar</a>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                        CommandName="Delete" Text="Eliminar"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle Width="100px" />
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign="Center" />        
        <RowStyle BackColor="#DEE2EB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <asp:SqlDataSource ID="reglasDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
        SelectCommand="SELECT e.idEmailRegla,e.nombreRegla,isnull(r.NOMREC,'NO ES CLIENTE') as NOMREC,e.estadoRegla FROM [EmailsReglas] e left join Receptor r on e.Receptor=r.RFCREC WHERE e.eliminado='False'
         and (e.nombreRegla like '%'+@nombre+'%' or @nombre = '-') and (r.NOMREC like '%'+@ruc+'%' or @ruc = '-')" 
        DeleteCommand="UPDATE EmailsReglas  SET eliminado='True' WHERE idEmailRegla= @idEmailRegla">
        <DeleteParameters>
            <asp:Parameter Name="idEmailRegla" />
        </DeleteParameters>
         <SelectParameters>
                    <asp:Parameter DefaultValue="-" Name="nombre" Type="String" />
                    <asp:Parameter DefaultValue="-" Name="ruc" Type="String" />
                </SelectParameters>
    </asp:SqlDataSource>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
