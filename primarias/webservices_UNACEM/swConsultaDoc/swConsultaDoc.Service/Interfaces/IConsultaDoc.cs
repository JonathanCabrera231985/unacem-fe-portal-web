using swConsultaDoc.Domain;

namespace swConsultaDoc.Data.Interfaces
{
    public interface IConsultaDoc
    {

        List<T> GetConsulta<T>(string RucEmisor,  string xmlContent, ref string mensajeerror);
        UsuarioModel? ValidaUsuario(UsuarioRequestModel Datos, ref string mensajeerror);
    }
}
