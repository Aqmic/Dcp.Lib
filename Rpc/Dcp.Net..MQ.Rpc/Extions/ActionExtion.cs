using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Geek.Net.MQ;
using Dynamic.Core.Runtime;
using Dynamic.Core.Comm;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Dynamic.Core.Service;
using Dynamic.Core.Reflection;

namespace Dcp.Net.MQ.Rpc.Extions
{
    public static class ActionExtion
    {
        public static string GetRouteAddress(this ActionSerDes actionSerDes)
        {
            string routeAddress = null;
            if (actionSerDes != null)
            {
                int paramCount = 0;
                if (actionSerDes.ParamterInfoArray == null)
                {
                    actionSerDes.ParamterInfoArray = new ParamterInfoDes[0];
                }
                paramCount = actionSerDes.ParamterInfoArray.Length;
                routeAddress = $"{actionSerDes.TargetTypeFullName}/{actionSerDes.MethodName}/";
                StringBuilder paramIdEntityBuilder = new StringBuilder();
                if (actionSerDes.ParamterInfoArray != null)
                {
                    for (int i = 0; i < actionSerDes.ParamterInfoArray.Length; i++)
                    {
                        var item = actionSerDes.ParamterInfoArray[i];
                        if (i == 0)
                        {
                            paramIdEntityBuilder.Append("(");
                        }
                        if (i == actionSerDes.ParamterInfoArray.Length - 1)
                        {
                            paramIdEntityBuilder.Append($"{item.TypeFullName}");
                            paramIdEntityBuilder.Append(")");
                        }
                        else
                        {
                            paramIdEntityBuilder.Append($"{item.TypeFullName},");
                        }
                    }
                }
            }
            return routeAddress;
        }
        public static object[] GetParamters(this ActionSerDes actionSerDes, MethodInfo methodInfo = null)
        {
            IList<object> paramterList = new List<object>();
            if (actionSerDes != null && actionSerDes.ParamterInfoArray != null)
            {
                foreach (var item in actionSerDes.ParamterInfoArray)
                {
                    Type itemType = item.GetRType(methodInfo);
                    object itemValue = item.Value;
                    if (itemType.IsValueType)
                    {
                        //值类型
                        itemValue = Convert.ChangeType(item.Value, itemType);
                    }
                    else if (itemValue != null)
                    {
                        JContainer jResult = itemValue as JContainer;
                        if (jResult != null)
                        {
                            MethodInfo _JsonMethod = typeof(SerializationUtility).GetMethods().FirstOrDefault(f => f.Name == "JsonToObject" && f.IsGenericMethodDefinition);
                            _JsonMethod = _JsonMethod.MakeGenericMethod(itemType);
                            //itemValue= _JsonMethod.Invoke(null,new object[] { jResult.ToString() });
                            var dynamicDelegate = DynamicMethodTool.GetMethodInvoker(_JsonMethod);
                            itemValue = dynamicDelegate(null, new object[] { jResult.ToString() });
                        }
                    }
                    paramterList.Add(itemValue);
                }
            }
            return paramterList.ToArray();
        }
        public static Type[] GetParamterTypes(this ActionSerDes actionSerDes, MethodInfo methodInfo = null)
        {
            IList<Type> paramterList = new List<Type>();
            if (actionSerDes != null && actionSerDes.ParamterInfoArray != null)
            {
                foreach (var item in actionSerDes.ParamterInfoArray)
                {
                    paramterList.Add(item.GetRType(methodInfo));
                }
            }
            return paramterList.ToArray();
        }


        public static Type GetRType(this ParamterInfoDes paramterInfoDes, MethodInfo methodInfo = null)
        {
            Type itemType = null;

            if (methodInfo != null)
            {
                var parameterInfo = methodInfo.GetParameters().FirstOrDefault(f => f.ParameterType.FullName == paramterInfoDes.TypeFullName);
                if (parameterInfo == null)
                {
                    throw new NullReferenceException($"{methodInfo.Name},无法被路由！");
                }
                itemType = parameterInfo.ParameterType;
            }
            else
            {
                if (paramterInfoDes.Value != null)
                {
                    itemType = paramterInfoDes.Value.GetType();
                }
                else
                {
                    itemType = Type.GetType(paramterInfoDes.TypeFullName);
                }
                if (itemType.IsValueType)
                {
                    itemType = Type.GetType(paramterInfoDes.TypeFullName);
                }
            }
            return itemType;
        }


    }
}
