using System.Reflection;

namespace Dcp.Net.MQ.Rpc.Register
{
    internal interface IRegisterService
    {
        void RegisterAssembly(Assembly assembly);
    }
}

