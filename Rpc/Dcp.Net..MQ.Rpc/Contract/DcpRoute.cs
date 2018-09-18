using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Contract
{
    /// <summary>
    /// Ppc的路由规则
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =true)]
    public class DcpRouteAttribute: Attribute  
    {
    }
}
