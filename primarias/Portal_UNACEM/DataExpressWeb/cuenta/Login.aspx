<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DataExpressWeb.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #Text1 {
            margin-right: 1px;
        }

        .style1 {
            width: 291px;
            text-align: center;
        }

        .style3 {
            color: #000000;
        }

        .style2 {
            color: #000000;
        }

        fieldset {
            border: 2px solid #ccc;
            padding: 2rem;
            border-radius: 8px;
            margin-bottom: 1.5rem;
        }
    </style>

    <script type="text/javascript">



        function mostrarModalPrivacidad() {
            var modal = document.getElementById('<%= modalPrivacidad.ClientID %>');
            var overlay = document.getElementById('<%= modalOverlay.ClientID %>');

            if (modal && overlay) {
                modal.style.display = 'block';
                overlay.style.display = 'block';
            }
        }

        function ocultarModalPrivacidad() {
            var check = document.getElementById('<%= checkPolitica.ClientID %>');
            var modal = document.getElementById('<%= modalPrivacidad.ClientID %>');
            var overlay = document.getElementById('<%= modalOverlay.ClientID %>');

            if (check && check.checked) {
                modal.style.display = 'none';
                overlay.style.display = 'none';
            } else {
                alert("Debe aceptar la política de privacidad para continuar.");
            }
        }

        //aqui inicia cookies

        function mostrarModalCookies() {
            document.getElementById('<%= modalCookies.ClientID %>').style.display = 'block';
        }
        function cerrarModalCookies() {
            // Crear una cookie con duración de 30 días
            var d = new Date();
            d.setTime(d.getTime() + (30 * 24 * 60 * 60 * 1000)); // 30 días en milisegundos
            var expires = "expires=" + d.toUTCString();
            document.cookie = "cookiesAceptadas=true;" + expires + ";path=/";

            // Ocultar el modal
            document.getElementById('<%= modalCookies.ClientID %>').style.display = "none";
        }
        function rechazarCookies() {
            var d = new Date();
            d.setTime(d.getTime() + (365 * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = "cookiesAceptadas=false;" + expires + ";path=/";

            document.getElementById('<%= modalCookies.ClientID %>').style.display = "none";

             // Aquí podrías deshabilitar scripts de seguimiento si los cargas condicionalmente
         }

        function obtenerUbicacion() {

            if (location.protocol == 'https:' || location.hostname == 'localhost')
            {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        document.getElementById('<%= hfLatitud.ClientID %>').value = position.coords.latitude;
                    document.getElementById('<%= hfLongitud.ClientID %>').value = position.coords.longitude;


                        document.getElementById('<%= bSesion.ClientID %>').click();
                    }, function (error) {
                        alert("No se pudo obtener la ubicación: " + error.message);
                                 <%--__doPostBack('<%= bSesion.UniqueID %>', '');--%>
                    });
                     } else {
                         alert("Tu navegador no soporta geolocalización.");
                          <%--  __doPostBack('<%= bSesion.UniqueID %>', '');--%>
                }
            }

            return true; // Importante: para evitar que el botón envíe antes de capturar ubicación
        }






    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Campos ocultos para ubicación -->
    <asp:HiddenField ID="hfLatitud" runat="server" />
    <asp:HiddenField ID="hfLongitud" runat="server" />

    <table style="width: 63%;">
        <tr>
            <td class="style1">
                <asp:UpdatePanel ID="upLogin" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- tus controles -->
                        <fieldset style="width: 250px">
                            <legend class="style3">Datos de Ingreso</legend>
                            <span class="style3">Username:
                                <br />
                            </span>
                            <asp:TextBox ID="tbRfcuser" runat="server" CssClass="txt_gr2"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="tbRfcuser" ErrorMessage="Requiere Username"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                            <br class="style3" />
                            <span class="style3">Password:<br />
                            </span>
                            <asp:TextBox ID="tbPass" runat="server" TextMode="password" autocomplete="off"
                                CssClass="txt_gr2" MaxLength="13"></asp:TextBox>
                            <span class="style3">
                                <br />
                                <span class="style2">
                                    <asp:Label ID="lblPass" runat="server" Visible="false" Text="Requiere Password" ForeColor="Red"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="tbPass" ErrorMessage="Requiere Password" ForeColor="Red"></asp:RequiredFieldValidator>
                                </span>
                                <br />


                                <asp:Button ID="bSesion" runat="server" Text="Ingresar"
                                    OnClick="bSesion_Click" 
                                    OnClientClick="return obtenerUbicacion()"
                                    CssClass="botones" Style="margin-bottom: 50xp" />
                                <br />
                                <asp:Label ID="lMensaje" runat="server" Style="color: #CC0000"></asp:Label>
                                <br />
                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/cuenta/Olvido.aspx">¿Ha olvidado la contraseña?</asp:HyperLink>
                            </span>


                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>



            </td>

            <td style="text-align: center"></td>
        </tr>
    </table>




    <div id="modalCookies" runat="server" style="display: none; position: fixed; top: 35%; left: 50%; transform: translate(-50%, -50%); background-color: #f0f0f0; color: #333; padding: 15px; text-align: center; border: 1px solid #ccc; box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); z-index: 1001;">
        <div style="max-width: 400px; margin: 0 auto;">
            <div style="display: flex; flex-direction: column; align-items: center;">
                <span style="font-size: 1.2em; font-weight: bold; margin-bottom: 10px;">
                    <img src="../imagenes/cookies-icon.png" width="24" height="24" alt="cookie icon">
                    Utilizamos cookies
                </span>
                <p style="font-size: 0.9em; margin-bottom: 15px;">
                    Usamos cookies para mejorar su experiencia de navegación en nuestra web, para mostrarle
                contenidos personalizados y para analizar el tráfico en nuestra web. 
                <a target="_blank" href="PoliticaCookies.aspx" style="color: #007bff; text-decoration: underline;">Ver política de cookies</a>
                </p>
                <div>
                    <button
                        type="button"
                        id="AceptarCookie"
                        onclick="cerrarModalCookies()"
                        style="background-color: #28a745; color: white; border: none; padding: 10px 20px; margin-left: 5px; margin-right: 5px; cursor: pointer; border-radius: 5px;">
                        Aceptar
                    </button>

                    <button
                        type="button"
                        id="RechazarCookie"
                        onclick="rechazarCookies()"
                        style="background-color: white; color: #333; border: 1px solid #ccc; padding: 10px 20px; margin-left: 5px; margin-right: 5px; cursor: pointer; border-radius: 5px;">
                        Rechazar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Fondo oscuro para bloquear navegación -->
    <div id="modalOverlay" runat="server" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5); z-index: 1000;">
    </div>

    <!-- Modal de política de privacidad -->
    <div id="modalPrivacidad" runat="server" style="display: none; position: fixed; top: 35%; left: 50%; transform: translate(-50%, -50%); background-color: #f0f0f0; color: #333; padding: 15px; text-align: center; border: 1px solid #ccc; box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); z-index: 1001;">
        <div style="max-width: 400px; margin: 0 auto;">
            <!-- Botón de cierre -->
            <button type="button" onclick="ocultarModalPrivacidad()"
                style="position: absolute; top: 5px; right: 5px; background-color: transparent; border: none; font-size: 18px; cursor: pointer;">
                &times;
            </button>

            <div style="display: flex; flex-direction: column; align-items: center;">
                <span style="font-size: 1.2em; font-weight: bold; margin-bottom: 10px;">Política de Privacidad
                </span>

                <div style="display: flex; align-items: center; gap: 8px;">
                    <asp:CheckBox ID="checkPolitica" runat="server" />
                    <span>Acepto la política de privacidad</span>
                </div>

                <p style="font-size: 0.9em; margin-bottom: 15px;">
                    Nos comprometemos a proteger su información personal. Le invitamos a conocer más sobre cómo recopilamos, usamos y protegemos sus datos.
                <a target="_blank" href="PoliticaPrivacidad.aspx" style="color: #007bff; text-decoration: underline;">Ver política de privacidad</a>
                </p>
            </div>
        </div>
    </div>








    <br />

    <br />

    <div style="display: flex; gap: 10px; margin-top: 25px; margin-left: 150px;">
        <a href="PoliticaCookies.aspx" target="_blank" style="color: #007bff; font-size: 1.5em; text-decoration: underline;">Ver política de cookies</a>
        <a href="PoliticaPrivacidad.aspx" target="_blank" style="color: #007bff; font-size: 1.5em; margin-left: 150px; text-decoration: underline;">Ver política de Privacidad</a>
        <a href="AvisoLegal.aspx" target="_blank" style="color: #007bff; font-size: 1.5em; margin-left: 150px; text-decoration: underline;">Aviso Legal</a>
        <a href="https://unacem.ec/sistema-de-gestion" target="_blank" style="color: #007bff; font-size: 1.5em; margin-left: 150px; text-decoration: underline;">Política SGSI</a>
    </div>

</asp:Content>
