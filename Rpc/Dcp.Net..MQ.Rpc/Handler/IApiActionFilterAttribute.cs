using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Handler
{
    /// <summary>
    /// 定义ApiAction过滤器修饰特性的的行为
    /// </summary>
    public interface IApiActionFilterAttribute : IApiActionFilter, IAttributeMultiplable
    {
    }
}
