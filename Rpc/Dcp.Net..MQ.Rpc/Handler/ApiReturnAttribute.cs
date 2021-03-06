﻿using Dcp.Net.MQ.Rpc.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.Handler
{
    /// <summary>
    /// 回复内容处理特性抽象
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiReturnAttribute : Attribute, IApiReturnAttribute
    {
        /// <summary>
        /// 获取或设置是否确保响应的http状态码通过IsSuccessStatusCode验证
        /// 当值为true时，请求可能会引发HttpStatusFailureException
        /// 默认为true
        /// </summary>
        public bool EnsureSuccessStatusCode { get; set; } = true;

        /// <summary>
        /// 获取异步结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <exception cref="HttpStatusFailureException"></exception>
        /// <returns></returns>
        Task<object> IApiReturnAttribute.GetTaskResult(ApiActionContext context)
        {
            if (this.EnsureSuccessStatusCode)
            {
                var statusCode = context.ResponseMessage.StatusCode;

                if (context.ResponseMessage.RemotingException != null)
                {
                    throw context.ResponseMessage.RemotingException;
                }   
            }
            return this.GetTaskResult(context);
        }


        /// <summary>
        /// 指示状态码是否为成功的状态码
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <returns></returns>
        protected virtual bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            var status = (int)statusCode;
            return status >= 200 && status <= 299;
        }

        /// <summary>
        /// 获取异步结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        protected abstract Task<object> GetTaskResult(ApiActionContext context);
    }
}
