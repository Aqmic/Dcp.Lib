using Dcp.Net.MQ.Rpc.Register;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Dcp.Net.MQ.Rpc.Contract;
using Dynamic.Core.Service;
using Dcp.Net.MQ.Rpc.Core;
using Dcp.Net.MQ.Rpc.Handler;

namespace Dcp.Net.MQ.Rpc.Default
{
    public class DefaultRegisterService : IRegisterService
    {        /// <summary>
             /// 接口对应的代理类型的构造器缓存
             /// </summary>
        private static readonly ConcurrentCache<Type, ConstructorInfo> DcpApiTypeCtorCache = new ConcurrentCache<Type, ConstructorInfo>();
        private static readonly ConcurrentCache<string, Type> ConstractInterfaceCache = new ConcurrentCache<string, Type>();
        public void RegisterAssembly(Assembly assembly)
        {
           var assemblyTypes=assembly.GetTypes();
           var rpcAssemblyTypes=assemblyTypes.Where(f =>typeof(IDcpApi).IsAssignableFrom(f));
            var arrary = rpcAssemblyTypes.ToArray();
            foreach (var item in rpcAssemblyTypes)
            {
                if (!item.IsAbstract&&!item.IsInterface)
                {
                    var iocType = typeof(IocUnity);
                    var methodInfo = iocType.GetMethod("AddTransient");
                    methodInfo = methodInfo.MakeGenericMethod(item);
                    methodInfo.Invoke(null, null);
                }
                else
                {
                    ConstractInterfaceCache.GetOrAdd(item.FullName, (key) => {
                        return item;
                    });
                }
                
            }
        }
        public void Call(ActionSerDes actionDes)
        {
            Type actionType = ConstractInterfaceCache.Get(actionDes.TypeFullName);
            var iocType = typeof(IocUnity);
            var methodInfo = iocType.GetMethod("Get");
            methodInfo = methodInfo.MakeGenericMethod(actionType);
            methodInfo.Invoke(null, null);
        }
        
    }
}
