using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;

namespace swConsultaDoc.Api.Util
{
    public class ServiBase
    {
        public JsonElement MapeoDynamico(Dictionary<string, string> p_campos, List<dynamic> p_Lista)
        {
            List<Dictionary<string, object>> p_resouesta = new();
            JsonElement respuesta;
            try
            {
                foreach (var item in p_Lista)
                {
                    IDictionary<string, object> p_Original = (IDictionary<string, object>)item;
                    IDictionary<string, object> origen = p_Original.ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
                    Dictionary<string, object> result = new(StringComparer.OrdinalIgnoreCase);

                    foreach (KeyValuePair<string, string> propiedad in p_campos)
                    {
                        string propiedadCampo = string.IsNullOrEmpty(propiedad.Value) ? propiedad.Key : propiedad.Value;
                        if (origen.ContainsKey(propiedad.Key))
                        {
                            result.Add(propiedadCampo, origen[propiedad.Key]);
                        }
                    }
                    p_resouesta.Add(result);
                }
            }
            catch (Exception)
            {

            }

            //string jresultado = JsonConvert.SerializeObject(p_resouesta, Formatting.Indented);
            string jresultado = JsonConvert.SerializeObject(p_resouesta);
            using (var doc = JsonDocument.Parse(jresultado))
            {
                respuesta = doc.RootElement.Clone();
            }
            return respuesta;
        }

        public static bool ValidaFormatFecha(string fecha)
        {
            if (fecha.Length != 8)
            {
                return false;
            }

            DateTime fechaConvertida;
            string formato = "ddMMyyyy";
            CultureInfo cultura = CultureInfo.InvariantCulture;

            bool esValido = DateTime.TryParseExact(fecha, formato, cultura, DateTimeStyles.None, out fechaConvertida);

            return esValido;
        }

        public static string ConvertirFechaASql(string fechaEnFormatoDDMMYYYY)
        {
            DateTime fecha = DateTime.ParseExact(fechaEnFormatoDDMMYYYY, "ddMMyyyy", CultureInfo.InvariantCulture);
            return fecha.ToString("yyyy-MM-dd");
        }

        public string ValidarFechas(string fechaInicioStr, string fechaFinStr, int rangoDias)
        {
            if (rangoDias > 100)
            {
                rangoDias = 100;
            }
            // Convertir las cadenas de texto a DateTime
            if (!DateTime.TryParseExact(fechaInicioStr, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaInicio) ||
                !DateTime.TryParseExact(fechaFinStr, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaFin))
            {
                // Si alguna de las conversiones falla, devolver un mensaje de error
                return "El formato de las fechas no es válido. Utilice el formato ddMMyyyy.";
            }

            // Verificar que la fecha de inicio no sea mayor que la fecha de fin
            if (fechaInicio > fechaFin)
            {
                return "El campo fechaInicio no puede ser mayor al campo fechaFin.";
            }

            // Verificar que el rango de días entre las fechas no supere el rango permitido
            TimeSpan diferencia = fechaFin - fechaInicio;
            if (diferencia.TotalDays > rangoDias)
            {
                return "Solo se puede consultar en rangos de " + rangoDias + " días.";
            }

            return "";
        }


    }
}
