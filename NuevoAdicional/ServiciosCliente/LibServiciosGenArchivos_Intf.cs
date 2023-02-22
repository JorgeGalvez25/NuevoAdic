using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiciosCliente
{
    using System;
    using RemObjects.SDK;
    using RemObjects.SDK.Types;
    using System.Collections.Generic;

    public interface IServiciosGenArchivos : RemObjects.SDK.IROService
    {
        int Sum(int A, int B);
        System.DateTime GetServerTime();
        bool SetRegenerarArchivosVolumetricos(System.DateTime AFecha, int ACorte, out string AMensajeError);
    }
    public partial class ServiciosGenArchivos_Proxy : RemObjects.SDK.Proxy, IServiciosGenArchivos
    {
        public ServiciosGenArchivos_Proxy(RemObjects.SDK.IMessage aMessage, RemObjects.SDK.IClientChannel aClientChannel) :
            base(aMessage, aClientChannel)
        {
        }
        public ServiciosGenArchivos_Proxy(RemObjects.SDK.IMessage aMessage, RemObjects.SDK.IClientChannel aClientChannel, string aOverrideInterfaceName) :
            base(aMessage, aClientChannel, aOverrideInterfaceName)
        {
        }
        public ServiciosGenArchivos_Proxy(RemObjects.SDK.IRemoteService aRemoteService) :
            base(aRemoteService)
        {
        }
        public ServiciosGenArchivos_Proxy(RemObjects.SDK.IRemoteService aRemoteService, string aOverrideInterfaceName) :
            base(aRemoteService, aOverrideInterfaceName)
        {
        }
        protected override string @__GetInterfaceName()
        {
            return "ServiciosGenArchivos";
        }
        public virtual int Sum(int A, int B)
        {
            @__Message.InitializeRequestMessage(@__ClientChannel, "LibServiciosGenArchivos", @__GetActiveInterfaceName(), "Sum");
            @__Message.WriteInt32("A", A);
            @__Message.WriteInt32("B", B);
            @__Message.FinalizeMessage();
            @__ClientChannel.Dispatch(@__Message);
            int Result = @__Message.ReadInt32("Result");
            @__Message.Clear();
            return Result;
        }
        public virtual System.DateTime GetServerTime()
        {
            @__Message.InitializeRequestMessage(@__ClientChannel, "LibServiciosGenArchivos", @__GetActiveInterfaceName(), "GetServerTime");
            @__Message.FinalizeMessage();
            @__ClientChannel.Dispatch(@__Message);
            System.DateTime Result = @__Message.ReadDateTime("Result");
            @__Message.Clear();
            return Result;
        }
        public virtual bool SetRegenerarArchivosVolumetricos(System.DateTime AFecha, int ACorte, out string AMensajeError)
        {
            @__Message.InitializeRequestMessage(@__ClientChannel, "LibServiciosGenArchivos", @__GetActiveInterfaceName(), "SetRegenerarArchivosVolumetricos");
            @__Message.WriteDateTime("AFecha", AFecha);
            @__Message.WriteInt32("ACorte", ACorte);
            @__Message.FinalizeMessage();
            @__ClientChannel.Dispatch(@__Message);
            bool Result = @__Message.ReadBoolean("Result");
            AMensajeError = @__Message.ReadAnsiString("AMensajeError");
            @__Message.Clear();
            return Result;
        }
    }
    public class CoServiciosGenArchivos
    {
        public static IServiciosGenArchivos Create(RemObjects.SDK.IMessage aMessage, RemObjects.SDK.IClientChannel aClientChannel)
        {
            return new ServiciosGenArchivos_Proxy(aMessage, aClientChannel);
        }
        public static IServiciosGenArchivos Create(RemObjects.SDK.IRemoteService aRemoteService)
        {
            return new ServiciosGenArchivos_Proxy(aRemoteService);
        }
    }
}