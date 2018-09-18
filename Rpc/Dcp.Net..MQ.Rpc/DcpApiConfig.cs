using Dcp.Net.MQ.Rpc.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc
{
    public class DcpApiConfig : IDisposable
    {
        /// <summary>
        /// 获取全局过滤器集合
        /// 非线程安全类型
        /// </summary>
        public GlobalFilterCollection GlobalFilters { get; private set; } = new GlobalFilterCollection();
        public RpcClient ApiClient { get; set; }
        /// <summary>
        /// 获取或设置是否对参数的属性值进行输入有效性验证
        /// 默认为true
        /// </summary>
        public bool UseParameterPropertyValidate { get; set; } = true;

        public Uri HttpHost { get; set; }
        public void Dispose()
        {
           
        }
    }
}
