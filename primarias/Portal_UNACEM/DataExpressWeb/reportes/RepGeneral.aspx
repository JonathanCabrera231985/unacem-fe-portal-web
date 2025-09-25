<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepGeneral.aspx.cs" Inherits="DataExpressWeb.RepGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style4
        {
            font-size: large;
        }
        .style5
        {
            width: 274px;
        }
        .style6
        {
            font-weight: bold;
            text-align: center;
        }
        .style7
        {
            text-align: center;
        }
    	.style8
					{
						height: 81px;
					}
    </style>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
	function a() {
		try {
	     		document.getElementById('<%= Label2.ClientID %>').textContent = "";
	     		document.getElementById('<%= bGenerar.ClientID %>').disabled = true;
	     		$("[id*=Button1]").click();
	   	}
	   	catch (err)
     { 
       alert(err); }
    	}
	function b(valor) {
		try {
			document.getElementById('<%= bGenerar.ClientID %>').disabled = valor;
		}
		catch (err2)
{ alert(err2); }
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="border: thin inset #C0C0C0; position: static; z-index: inherit; width: inherit; height: inherit"><legend class="style4">REPORTE GENERAL</legend>
    <table align="center">
        <tr>
            <td class="style17">
                <span style="text-align: left">DOCUMENTO:</span><strong 
																	style="text-align: left"><br />
																</strong>
																<asp:DropDownList ID="ddlDocumento" runat="server" AppendDataBoundItems="True" 
																	AutoPostBack="True" DataSourceID="SqlDataSourceCodDoc" 
																	DataTextField="descripcion" DataValueField="codigo" Height="19px" 
																	onselectedindexchanged="ddlDocumento_SelectedIndexChanged" Width="200px">
																	<asp:ListItem Value="0">Selecciona el Tipo</asp:ListItem>
																</asp:DropDownList>
												</td>
            <td class="style17">
                <span style="text-align: left">RUC:<strong style="text-align: left"><br />
																<asp:TextBox ID="tb_ruc" runat="server" Height="17px" Width="198px"></asp:TextBox>
																</strong></span>
												</td>
        </tr>
        <tr>
            <td class="style16">
                <span style="text-align: left">SUCURSAL:<br __designer:mapid="23" />
																<asp:DropDownList ID="ddlSucursalR" runat="server" AutoPostBack="True" 
																	DataSourceID="SqlDataSucursalR" DataTextField="sucursal" 
																	DataValueField="idSucursal" Height="19px" 
																	onselectedindexchanged="ddlSucursalR_SelectedIndexChanged" Width="200px">
																	<asp:ListItem Value="-1">Selecciona...</asp:ListItem>
																</asp:DropDownList>
																</span></td>
            <td class="style16">
                <span style="text-align: left">PUNTO DE EMISIÓN:<br __designer:mapid="27" />
																<asp:DropDownList ID="ddlPtoEmiR" runat="server" AutoPostBack="True" 
																	DataSourceID="SqlDataPtoEmisionR" DataTextField="ptoEmi" 
																	DataValueField="ptoEmi" Height="19px" 
																	OnSelectedIndexChanged="ddlPtoEmiR_SelectedIndexChanged" Width="200px">
																	<asp:ListItem Value="-1">Selecciona...</asp:ListItem>
																</asp:DropDownList>
																</span></td>
        </tr>
        <tr>
            <td class="style6">
                &nbsp;</td>
            <td class="style7">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style6">
                Fecha Inicial:</td>
            <td class="style7">
                <b style="text-align: center">Fecha Final:</b></td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Calendar ID="calentario" runat="server" BackColor="White" 
                    BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" 
                    ForeColor="Black" Height="180px" Width="200px" 
                    style="text-align: left" CellPadding="4" DayNameFormat="Shortest">
                    <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
                    <NextPrevStyle 
                        VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#5F6062" BorderColor="Black" 
                        Font-Bold="True" />
                    <TodayDayStyle BackColor="#DEE2EB" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#DEE2EB" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Calendar ID="calendario2" runat="server" BackColor="White" 
                    BorderColor="#999999" Font-Names="Verdana" Font-Size="8pt" 
                    ForeColor="Black" Height="180px" Width="200px" CellPadding="4" 
                    DayNameFormat="Shortest">
                    <DayHeaderStyle Font-Bold="True" Font-Size="7pt" BackColor="#CCCCCC" />
                    <NextPrevStyle 
                        VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" ForeColor="White" Font-Bold="True" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#5F6062" BorderColor="Black" 
                        Font-Bold="True" />
                    <TodayDayStyle BackColor="#DEE2EB" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#DEE2EB" />
                </asp:Calendar>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="style8">
                <center><asp:Button ID="bGenerar" runat="server"  OnClientClick="a();"
                    Text="Generar" style="text-align: center" />
                <br />
                <asp:Label ID="Label2" runat="server" ForeColor="Red" 
                    style="text-align: center"></asp:Label></center>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span style="text-align: left">
																<asp:SqlDataSource ID="SqlDataSourceCodDoc" runat="server" 
																	ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
																	SelectCommand="SELECT codigo, descripcion, tipo FROM Catalogo1_C WHERE (tipo = 'Comprobante') and codigo in ('01','04','05','06','07')">
																</asp:SqlDataSource>
																<asp:SqlDataSource ID="SqlDataSucursalR" runat="server" 
																	ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
																	SelectCommand="SELECT '-1' idSucursal, 'Seleccione la Sucursal' Sucursal
UNION  SELECT Sucursales.clave, Sucursales.sucursal + ':' + Sucursales.clave AS Sucursal FROM Sucursales  where eliminado=0 ORDER BY 1 DESC">
																</asp:SqlDataSource>
																<asp:SqlDataSource ID="SqlDataPtoEmisionR" runat="server" 
																	ConnectionString="<%$ ConnectionStrings:dataexpressConnectionString %>" 
																	SelectCommand="select 'Selecciona...' ptoEmi  union select  distinct ptoEmi from GENERAL G  where G.codDoc in(@docf,@docnc,@docnd,@docg,@docr) and G.estab = @idSucursal  ORDER BY 1 DESC">
																	<SelectParameters>
																		<asp:Parameter Name="docf" />
																		<asp:Parameter Name="docnc" />
																		<asp:Parameter Name="docnd" />
																		<asp:Parameter Name="docg" />
																		<asp:Parameter Name="docr" />
																		<asp:Parameter Name="idSucursal" />
																	</SelectParameters>
																</asp:SqlDataSource>
																</span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span style="text-align: left">
																<asp:Button ID="Button1" runat="server" BackColor="White" BorderStyle="None" 
																	Enabled="true" OnClick="bGenerar_Click" Text="." />
																</span></td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
    </table>
    </fieldset>
</asp:Content>
