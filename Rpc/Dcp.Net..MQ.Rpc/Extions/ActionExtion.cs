using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Geek.Net.MQ;
using Dynamic.Core.Runtime;
using Dynamic.Core.Comm;

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
        public static object[] GetParamters(this ActionSerDes actionSerDes)
        {
            IList<object> paramterList = new List<object>();
            if (actionSerDes != null && actionSerDes.ParamterInfoArray != null)
            {
                foreach (var item in actionSerDes.ParamterInfoArray)
                {
                    paramterList.Add(item.Value);
                }
            }
            return paramterList.ToArray();
        }

       
    }
}
