using System.Text.Json.Serialization;

namespace swConsultaDoc.Domain
{
    public class Response<T>
    {
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "Error";
        [JsonPropertyName("datos")]
        public T? Data { get; set; }
        [JsonPropertyName("mensajes")]
        public string Mensaje { get; set; }

    }
}
