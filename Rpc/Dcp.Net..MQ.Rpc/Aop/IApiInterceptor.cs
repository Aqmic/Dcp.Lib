﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Aop
{
    /// <summary>
    /// 定义Rpc接口拦截器的行为
    /// </summary>
    public interface IApiInterceptor : IDisposable
    {
        /// <summary>
        /// 拦截方法的调用
        /// </summary>
        /// <param name="target">接口的实例</param>
        /// <param name="method">接口的方法</param>
        /// <param name="parameters">接口的参数集合</param>
        /// <returns></returns>
        object Intercept(object target, MethodInfo method, object[] parameters);
    }
}
