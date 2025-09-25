namespace swConsultaDoc.Domain
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public bool Estado { get; set; }

    }
}
