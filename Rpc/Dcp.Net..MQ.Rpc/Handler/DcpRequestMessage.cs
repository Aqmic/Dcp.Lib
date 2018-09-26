using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Handler
{
    public class DcpRequestMessage
    {
        public string Content { get; set; }
        /// <summary>
        /// 通过url方式调用，这种方法预留，后面跨平台时候试用
        /// </summary>
        public string RequestUri { get; set; }

        /// <summary>
        /// 二进制方式调用，.NET平台内部调用，性能更高
        /// </summary>
        public ActionSerDes ActionInfo { get; set; }

    }
    public class ParamterInfoDes {
        public ParamterInfoDes()
        {
            this.Endoding = "utf-8";
        }
        public string ParamterName { get; set; }
        public string TypeFullName { get; set; }
        public object Value { get; set; }
        public string Endoding { get; set; }
    }
    public class RtnInfoDes {
        public RtnInfoDes()
        {
            this.Endoding = "utf-8";
        }
        public string TypeFullName { get; set; }
        public string Endoding { get; set; }
        public string DataTypeFullName { get; set; }
        public object Value { get; set; }
    }
    public class ActionSerDes
    {
        /// <summary>
        /// 远程执行类
        /// </summary>
        public string TargetTypeFullName { get; set; }
        /// <summary>
        /// 远程执行方法
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public RtnInfoDes RtnInfo{get;set;}
        /// <summary>
        /// 调用远程方法的参数
        /// </summary>
        public ParamterInfoDes[] ParamterInfoArray { get; set; }
    }
}
