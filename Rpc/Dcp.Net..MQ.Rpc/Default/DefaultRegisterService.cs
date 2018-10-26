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
using Dynamic.Core.Log;
using Dynamic.Core.Reflection;

namespace Dcp.Net.MQ.Rpc.Default
{
    public class DefaultRegisterService : IRegisterService
    {
        private static readonly ILogger _logger=LoggerManager.GetLogger("DefaultRegisterService");
        private static readonly object _lockRouteObj = new object();
        private static readonly object _lockRegisterObj = new object();
        private static readonly IList<string> ActionRouteKeyList = new List<string>();
        private static readonly ConcurrentCache<string, Type> ConstractInterfaceCache = new ConcurrentCache<string, Type>();
        private static readonly ConcurrentCache<string, MethodInfo> ActionMethodInfoCache = new ConcurrentCache<string, MethodInfo>();
        private static readonly IList<string> _ConstractAssemblyeCache = new List<string>();
        public IList<string> GetRouteKeyList()
        {
            return ActionRouteKeyList;
        }
        public bool IsRegistered(Assembly assembly)
        {
           return IsRegistered(assembly.FullName);
        }
        public bool IsRegistered(string assemblyFullName)
        {
            if (string.IsNullOrEmpty(assemblyFullName))
            {
                return false;
            }
            lock (_lockRegisterObj)
            {
               var regAssemblyName=_ConstractAssemblyeCache.FirstOrDefault(f => f.Equals(assemblyFullName, StringComparison.OrdinalIgnoreCase));
                if (string.IsNullOrEmpty(regAssemblyName))
                {
                    _ConstractAssemblyeCache.Add(assemblyFullName);
                    return false;
                }
                return true;
            }
        }
        public void RegisterAssembly(Assembly assembly)
        {
            if (assembly.FullName == this.GetType().Assembly.FullName)
            { 
                //不能自己注册自己
                return;
            }
            if (IsRegistered(assembly))
            {
                //已经注册的就不注册了
                return;
            }
            var assemblyTypes = assembly.GetTypes();
            var rpcAssemblyTypes = assemblyTypes.Where(f => typeof(IDcpApi).IsAssignableFrom(f));
            var interfaceConstractTypeList = rpcAssemblyTypes.Where(f => f.IsInterface);
            var contractServertypeList = rpcAssemblyTypes.Where(f => !f.IsAbstract && !f.IsInterface);
            foreach (var item in interfaceConstractTypeList)
            {
                var constractService = contractServertypeList.FirstOrDefault(f => item.IsAssignableFrom(f));
                RegisterType(item, constractService);
            }
        }
        public void RegisterType(Type interfaceConstract, Type constractService) {
           
            if (constractService != null&&interfaceConstract!=null)
            {
                if (!constractService.IsAbstract && !constractService.IsInterface&&interfaceConstract.IsAssignableFrom(constractService))
                {
                    IocUnity.AddTransient(interfaceConstract, constractService);
                    ConstractInterfaceCache.GetOrAdd(interfaceConstract.FullName, key =>
                    {
                        return interfaceConstract;
                    });
                    AddRouteKey(interfaceConstract.FullName);
                }
               
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
            dynamic serviceValue = IocUnity.Get(actionType);
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

            var actionParamters = actionDes.GetParamters(callMethodInfo);
            object rtnObj = null;
            try
            {
                var dynamicDelegate = DynamicMethodTool.GetMethodInvoker(callMethodInfo);
                rtnObj = dynamicDelegate(serviceValue,actionParamters);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                throw;
            }
            
            return rtnObj;
        }
    }
}
