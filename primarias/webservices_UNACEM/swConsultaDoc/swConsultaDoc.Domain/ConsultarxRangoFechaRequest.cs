namespace swConsultaDoc.Domain
{
    public class ConsultarxRangoFechaRequest
    {
        public string RucEmisor { get; set; } = string.Empty;
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string TipoDocumento { get; set; }
        public string Estado { get; set; }

    }
}
