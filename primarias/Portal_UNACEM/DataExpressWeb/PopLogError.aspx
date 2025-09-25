<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopLogError.aspx.cs" Inherits="DataExpressWeb.PopLogError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script type = "text/javascript">
        function OnClose() {
            if (window.opener != null && !window.opener.closed) {
                window.opener.HideModalDiv();
            }
        }
        window.onunload = OnClose;
    </script>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <asp:GridView runat="server" ID="ugLogError" AutoGenerateColumns="false"   CellPadding="3" GridLines="Horizontal" BackColor="White" BorderColor="#336666"  BorderWidth="3px" BorderStyle="Double" AllowPaging="true" AllowSorting="True"
                         DataSourceID="dataSourceLogError">
            <Columns>
                  <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" />
                 <asp:BoundField DataField="detalle" HeaderText="Detalle Tecnico del SRI" SortExpression="detalle"/>
                 <asp:BoundField DataField="fechaAutorizacion" HeaderText="Fecha Autorización" SortExpression="fechaAutorizacion"/>
                 <asp:BoundField DataField="numeroAutorizacion" HeaderText="Nº Autorización" SortExpression="numeroAutorizacion"/>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <HeaderStyle BackColor="#008834" Font-Bold="True" ForeColor="White" 
                Font-Size="Small" />
            <PagerStyle BackColor="#008834" ForeColor="White" 
                HorizontalAlign="Center" />
            <RowStyle BackColor="White" ForeColor="#333333" />
             <FooterStyle BackColor="#CCCC99" />
            <HeaderStyle BackColor="#5F6062" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#5F6062" ForeColor="Black" HorizontalAlign= "Center" />
            <RowStyle BackColor="#DEE2EB" />
            <SelectedRowStyle BackColor="#CE5D5A" ForeColor="White" Font-Bold="True" />
            <SortedAscendingCellStyle BackColor="#FBFBF2" />
            <SortedAscendingHeaderStyle BackColor="#848384" />
            <SortedDescendingCellStyle BackColor="#EAEAD3" />
            <SortedDescendingHeaderStyle BackColor="#575357" />
        </asp:GridView>  
    </div>
    <asp:SqlDataSource ID="dataSourceLogError" runat="server" ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>"
        SelectCommand="select A.detalle, A.fecha, numeroAutorizacion, fechaAutorizacion from	LogErrorFacturas	A inner join	GENERAL  B on A.numeroDocumento = B.codigoControl  where idComprobante =@idComprobante" >
        <SelectParameters>
            <asp:Parameter Name="idComprobante"  DefaultValue="0"/>
        </SelectParameters>
      </asp:SqlDataSource>
    </form>
</body>
</html>
