using Dcp.Net.MQ.Rpc.Aop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Contract
{
    /// <summary>
    /// 表示DcpApi客户端
    /// 提供创建HttpApiClient实例的方法
    /// </summary>
    [DebuggerTypeProxy(typeof(DebugView))]
    public abstract partial class DcpApiClientProxy : IDcpApiClient, IDcpApi, IDisposable
    {
        /// <summary>
        /// 获取Api拦截器
        /// </summary>
        public IApiInterceptor ApiInterceptor { get; private set; }

        /// <summary>
        /// Dcp客户端的基类
        /// </summary>
        /// <param name="apiInterceptor">拦截器</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DcpApiClientProxy(IApiInterceptor apiInterceptor)
        {
            this.ApiInterceptor = apiInterceptor ?? throw new ArgumentNullException(nameof(apiInterceptor));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.ApiInterceptor.Dispose();
        }

        /// <summary>
        /// 调试视图
        /// </summary>
        private class DebugView : DcpApiClientProxy
        {
            /// <summary>
            /// 调试视图
            /// </summary>
            /// <param name="target">查看的对象</param>
            public DebugView(DcpApiClientProxy target)
                : base(target.ApiInterceptor)
            {
            }
        }
    }
}
