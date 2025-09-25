using Microsoft.AspNetCore.Mvc;
using swConsultaDoc.Api.Services.Contracts;
using swConsultaDoc.Domain;
using System.Text.Json;

namespace swConsultaDoc.Api.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UsuariosController : Controller
    {
        private readonly IAuthService authService;
        public UsuariosController(IAuthService authService)
        {
            this.authService = authService;
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Token(UsuarioRequestModel credenciales)
        {
            Object? valor = authService.Valida(credenciales);

            return Json(data: valor);
        }

    }
}