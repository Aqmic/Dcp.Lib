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
using Dcp.Net.MQ.Rpc.Extions;

namespace Dcp.Net.MQ.Rpc.Default
{
    public class DefaultRegisterService : IRegisterService
    {
        private static readonly object _lockRouteObj = new object();
        private static readonly IList<string> ActionRouteKeyList = new List<string>();
        private static readonly ConcurrentCache<string, Type> ConstractInterfaceCache = new ConcurrentCache<string, Type>();
        private static readonly ConcurrentCache<string, MethodInfo> ActionMethodInfoCache = new ConcurrentCache<string, MethodInfo>();

        public IList<string> GetRouteKeyList()
        {
            return ActionRouteKeyList;
        }
        public void RegisterAssembly(Assembly assembly)
        {
            var assemblyTypes = assembly.GetTypes();
            var rpcAssemblyTypes = assemblyTypes.Where(f => typeof(IDcpApi).IsAssignableFrom(f));
            var interfaceConstractTypeList = rpcAssemblyTypes.Where(f => f.IsInterface);
            var contractServertypeList = rpcAssemblyTypes.Where(f => !f.IsAbstract && !f.IsInterface);
            foreach (var item in interfaceConstractTypeList)
            {
                var constractService = contractServertypeList.FirstOrDefault(f => item.IsAssignableFrom(f));
                if (constractService != null)
                {
                    IocUnity.AddTransient(item, constractService);
                    ConstractInterfaceCache.GetOrAdd(item.FullName, key =>
                    {
                        return item;
                    });
                }
                AddRouteKey(item.FullName);
            }
        }
        private void AddRouteKey(string routeKey)
        {
            lock (_lockRouteObj)
            {
                if (!ActionRouteKeyList.Contains(routeKey))
                {
                    ActionRouteKeyList.Add(routeKey);
                }
            }
        }
        public object CallAction(ActionSerDes actionDes)
        {
            MethodInfo callMethodInfo = null;
            Type actionType = ConstractInterfaceCache.Get(actionDes.TargetTypeFullName);
            var serviceValue = IocUnity.Get(actionType);
            string actionKey = actionDes.GetRouteAddress();
            callMethodInfo = ActionMethodInfoCache.GetOrAdd(actionKey, key =>
             {
                 var sameNameMethodList = actionType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(f => f.Name == actionDes.MethodName).ToList();
                 MethodInfo methodInfo = null;
                 //只区分参数个数不区分类型
                 if (sameNameMethodList.Count == 1)
                 {
                     methodInfo = sameNameMethodList.FirstOrDefault();
                 }
                 else
                 {
                     methodInfo = sameNameMethodList.FirstOrDefault(f => f.GetParameters().Length == actionDes.ParamterInfoArray.Length);
                 }
                 return methodInfo;
             });
            if (callMethodInfo == null)
            {
                throw new KeyNotFoundException($"路由地址没有找到【{actionKey}】！");
            }
            var rtnObj = callMethodInfo.Invoke(serviceValue, actionDes.GetParamters());
            return rtnObj;
        }
    }
}
