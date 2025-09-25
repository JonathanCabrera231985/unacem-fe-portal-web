namespace swConsultaDoc.Domain
{
    public class ConsultarxNumDocRequest
    {
        public string RucEmisor { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumDocumento { get; set; } = string.Empty;
    }
}
