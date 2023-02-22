using System.ServiceModel;
using ImagenSoft.ModuloWeb.Entidades;

namespace ImagenSoft.ModuloWeb.Interfaces.Publicador
{
    [ServiceContract(Namespace = "http://moduloweb.adicional.com")]
    public interface IModuloWebPerform
    {
        [OperationContract(Name = "PingLite")]
        bool Ping();

        [OperationContract(Name = "Ping")]
        bool Ping(SesionModuloWeb sesion);

        [OperationContract(Name = "GetConfig")]
        byte[] GetConfig(byte[] request);
    }
}
