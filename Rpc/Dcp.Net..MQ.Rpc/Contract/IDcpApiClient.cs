using Dcp.Net.MQ.Rpc.Aop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Contract
{
    public interface IDcpApiClient:IDcpApi
    {
        /// <summary>
        /// 获取拦截器
        /// </summary>
        IApiInterceptor ApiInterceptor { get; }
    }
}
