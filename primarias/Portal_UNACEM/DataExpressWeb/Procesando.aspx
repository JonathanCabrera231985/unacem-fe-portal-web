<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="Procesando.aspx.cs" Inherits="DataExpressWeb.Procesando" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
 .modalPopup
 {
     background-color: #445512;
     border-width: 3px;
     border-style: solid;
     border-color: Gray;
     padding: 3px;
 }
 .modalBackground
 {
     position:absolute;
     width: 100%;
     height:100%;
     text-align:center;
     background-color: #DEE2EB;
     filter: alpha(opacity=70);
     opacity: 0.70;
     top: 0px;
     left: 0px;
}
   
 </style>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:Timer ID="Timer1" runat="server" OnTick="timer1_tick" Interval="2000"></asp:Timer>
 <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
     <ContentTemplate >
        <div class="modalBackground">
            <br /><br /> <br /><br /><br /><br /><br /><br /> <br /><br /><br /><br /><br /><br /><br />
             Procesando.... 
            <br />
            <asp:Image ID="Image2" runat="server" ImageUrl="~/imagenes/loader.gif" />
            <br />
            <br />
            <br />
            <asp:HiddenField ID="hdCount" runat="server" />
        </div>
     </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
