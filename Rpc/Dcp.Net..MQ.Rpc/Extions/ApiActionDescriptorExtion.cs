using Dcp.Net.MQ.Rpc.Aop;
using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Extions
{
    public static class ApiActionDescriptorExtion
    {
        public static ActionSerDes ToActionSerDes(this ApiActionDescriptor apiActionDescriptor)
        {
            ActionSerDes actionSerDes = null;
            if (apiActionDescriptor != null)
            {
                actionSerDes = new ActionSerDes();
                actionSerDes.TargetTypeFullName = apiActionDescriptor.TargetTypeFullName;
                actionSerDes.MethodName = apiActionDescriptor.Member.Name;
                if (apiActionDescriptor.Parameters != null)
                {
                    IList<ParamterInfoDes> paramterInfoDesList = new List<ParamterInfoDes>();
                    foreach (var item in apiActionDescriptor.Parameters)
                    {
                        ParamterInfoDes paramterInfoDes = item.ToParamInfo();
                        paramterInfoDesList.Add(paramterInfoDes);
                    }
                    actionSerDes.ParamterInfoArray = paramterInfoDesList.ToArray();
                    actionSerDes.RtnInfo = apiActionDescriptor.Return.ToRtnInfo();
                }
            }
            return actionSerDes;
        }
        public static RtnInfoDes ToRtnInfo(this ApiReturnDescriptor apiReturnDescriptor)
        {
            RtnInfoDes rtnInfoDes = null;
            if (apiReturnDescriptor != null)
            {
                rtnInfoDes = new RtnInfoDes();
                rtnInfoDes.TypeFullName = apiReturnDescriptor.ReturnType.FullName;
                rtnInfoDes.DataTypeFullName = apiReturnDescriptor.DataType.FullName;
            }
            return rtnInfoDes;
        }
        public static ParamterInfoDes ToParamInfo(this ApiParameterDescriptor apiParameterDescriptor)
        {
            ParamterInfoDes paramterInfoDes = null;
            if (apiParameterDescriptor != null)
            {
                paramterInfoDes = new ParamterInfoDes();
                paramterInfoDes.ParamterName = apiParameterDescriptor.Name;
                paramterInfoDes.TypeFullName = apiParameterDescriptor.ParameterType.FullName;
                paramterInfoDes.Value = apiParameterDescriptor.Value;
            }
            return paramterInfoDes;
        }
    }
}
