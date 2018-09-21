using Dcp.Net.MQ.Rpc.Register;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Dcp.Net.MQ.Rpc.Contract;
using Dynamic.Core.Service;
using Dcp.Net.MQ.Rpc.Core;

namespace Dcp.Net.MQ.Rpc.Default
{
    public class DefaultRegisterService : IRegisterService
    {        /// <summary>
             /// 接口对应的代理类型的构造器缓存
             /// </summary>
        private static readonly ConcurrentCache<Type, ConstructorInfo> DcpApiTypeCtorCache = new ConcurrentCache<Type, ConstructorInfo>();
        public void RegisterAssembly(Assembly assembly)
        {
           var assemblyTypes=assembly.GetTypes();
           var rpcAssemblyTypes=assemblyTypes.Where(f => !f.IsAbstract && !f.IsInterface && typeof(IDcpApi).IsAssignableFrom(f));
            foreach (var item in rpcAssemblyTypes)
            {
                var iocType = typeof(IocUnity);
                var methodInfo = iocType.GetMethod("AddTransient");
                methodInfo = methodInfo.MakeGenericMethod(item);
                methodInfo.Invoke(null, null);
            }
        }
        
    }
}
