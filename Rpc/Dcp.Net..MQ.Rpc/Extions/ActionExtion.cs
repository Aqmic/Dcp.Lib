using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

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
                routeAddress = $"{actionSerDes.TargetTypeFullName}/{actionSerDes.MethodName}/{paramCount}";
            }
            return routeAddress;
        }
        //public static object ToParamters(this ParamterInfoDes paramterInfoDes)
        //{
        //    paramterInfoDes.
        //}
    }
}
