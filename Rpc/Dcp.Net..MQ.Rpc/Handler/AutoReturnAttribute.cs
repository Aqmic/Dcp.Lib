﻿using Dcp.Net.MQ.Rpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.Handler
{
    /// <summary>
    /// 自动适应返回类型的处理
    /// 支持返回TaskOf(HttpResponseMessage)或TaskOf(byte[])或TaskOf(string)或TaskOf(Stream)
    /// 支持返回xml或json转换对应类型
    /// 没有任何IApiReturnAttribute特性修饰的接口方法，将默认为AutoReturn修饰
    /// </summary> 
    public class AutoReturnAttribute : ApiReturnAttribute
    {
       

        /// <summary>
        /// 获取异步结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        protected override async Task<object> GetTaskResult(ApiActionContext context)
        {
            return context.ResponseMessage.Result;
            //var response = context.ResponseMessage;
            //var dataType = context.ApiActionDescriptor.Return.DataType;

            //if (dataType == typeof(HttpResponseMessage))
            //{
            //    return response;
            //}

            //if (dataType == typeof(string))
            //{
            //    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //}

            //if (dataType == typeof(byte[]))
            //{
            //    return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            //}

            //if (dataType == typeof(Stream))
            //{
            //    return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            //}

            //var contentType = new ContentType(response.Content.Headers.ContentType);
            //if (contentType.IsApplicationJson() == true)
            //{
            //    return await jsonReturn.GetTaskResult(context).ConfigureAwait(false);
            //}
            //else if (contentType.IsApplicationXml() == true)
            //{
            //    return await xmlReturn.GetTaskResult(context).ConfigureAwait(false);
            //}

            //throw new ApiReturnNotSupportedExteption(response, dataType);
        }
    }
}
