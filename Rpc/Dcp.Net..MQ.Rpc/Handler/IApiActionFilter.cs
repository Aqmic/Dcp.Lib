using Dcp.Net.MQ.Rpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.Handler
{
    /// <summary>
    /// 定义ApiAction过滤器的行为
    /// </summary>
    public interface IApiActionFilter
    {
        /// <summary>
        /// 准备请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        Task OnBeginRequestAsync(ApiActionContext context);

        /// <summary>
        /// 请求完成之后
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        Task OnEndRequestAsync(ApiActionContext context);
    }
}
