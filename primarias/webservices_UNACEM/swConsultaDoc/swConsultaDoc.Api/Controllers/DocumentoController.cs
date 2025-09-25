using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using swConsultaDoc.Api.Services.Contracts;
using swConsultaDoc.Domain;
using System.Text.Json;

namespace swConsultaDoc.Api.Controllers
{
    [ApiController]
    [Route("/documento")]
    [Authorize]
    public class DocumentoController : Controller
    {
        #region ATRIBUTOS
        private readonly IDocumentoService documentoService;
        #endregion

        #region CONSTRUCTOR
        public DocumentoController(IDocumentoService documentoService)
        {
            this.documentoService = documentoService;
        }
        #endregion

        [Route("ObtieneDocumentoxIdSistema")]
        [HttpPost]
        public ActionResult ObtieneDocumentoxIdSistema(ConsultarxIdSistemaRequest datos)
        {
            Object? valor = documentoService.ObtieneDocumentoxIdSistema(datos);

            return Json(data: valor);
        }

        [Route("ObtieneDocumentoxNumDoc")]
        [HttpPost]
        public ActionResult ObtieneDocumentoxNumDoc(ConsultarxNumDocRequest datos)
        {
            Object? valor = documentoService.ObtieneDocumentoxNumDoc(datos);

            return Json(data: valor);
        }

        [Route("ObtieneDocumentoxRangoFecha")]
        [HttpPost]
        public ActionResult ObtieneDocumentoxRangoFecha(ConsultarxRangoFechaRequest datos)
        {
            Object? valor = documentoService.ObtieneDocumentoxRangoFecha(datos);

            return Json(data: valor);
        }
    }
}
