using swConsultaDoc.Domain;
using System.Text.Json;

namespace swConsultaDoc.Api.Services.Contracts
{
    public interface IDocumentoService
    {
        public Response<JsonElement?> ObtieneDocumentoxIdSistema(ConsultarxIdSistemaRequest datos);
        public Response<JsonElement?> ObtieneDocumentoxNumDoc(ConsultarxNumDocRequest datos);
        public Response<JsonElement?> ObtieneDocumentoxRangoFecha(ConsultarxRangoFechaRequest datos);

    }
}
