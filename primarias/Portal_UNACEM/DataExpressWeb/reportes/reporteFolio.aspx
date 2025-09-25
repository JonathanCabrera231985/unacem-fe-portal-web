<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reporteFolio.aspx.cs" Inherits="DataExpressWeb.reporteFolio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .style2
    {
        font-size: large;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset><legend class="style2">REPORTE DE FOLIOS</legend>
    <table align="center">
        <tr>
            <td>
                <asp:Calendar ID="Calendar1" runat="server" BackColor="White"  
                    BorderColor="White" Font-Names="Verdana" Font-Size="9pt" 
                    ForeColor="Black" Height="190px" Width="266px" 
                    onselectionchanged="Calendar1_SelectionChanged" BorderWidth="1px" 
                    NextPrevFormat="FullMonth">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle 
                        VerticalAlign="Bottom" Font-Bold="True" Font-Size="8pt" 
                        ForeColor="#333333" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="White" 
                        Font-Bold="True" BorderWidth="4px" Font-Size="12pt" ForeColor="#7AC043" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bGenerar" runat="server" Text="Generar" 
                    onclick="bGenerar_Click" />
                <br />
                <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                <asp:Label ID="Label3" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </fieldset>
</asp:Content>
