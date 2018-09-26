using Dcp.Net.MQ.Rpc.Aop;
using Dcp.Net.MQ.Rpc.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.Core
{
    /// <summary>
    /// 提供ApiTask的创建
    /// </summary>
    abstract class ApiTask
    {

        /// <summary>
        /// 完成的任务
        /// </summary>
        /// <returns></returns>
        public static readonly Task CompletedTask = Task.CompletedTask;

        /// <summary>
        /// 获取ITaskOf(dataType)的构造器
        /// </summary>
        /// <param name="dataType">泛型参数类型</param>
        /// <returns></returns>
        public static ConstructorInfo GetITaskConstructor(Type dataType)
        {
            return typeof(ApiTaskOf<>)
                .MakeGenericType(dataType)
                .GetConstructor(new[] { typeof(DcpApiConfig), typeof(ApiActionDescriptor) });
        }

        /// <summary>
        /// 创建ApiTaskOf(T)的实例
        /// </summary>
        /// <param name="httpApiConfig">http接口配置</param>
        /// <param name="apiActionDescriptor">api描述</param>
        /// <returns></returns>
        public static ApiTask CreateInstance(DcpApiConfig httpApiConfig, ApiActionDescriptor apiActionDescriptor)
        {
            // var instance = new ApiTask<TResult>(httpApiConfig, apiActionDescriptor);
            var ctor = apiActionDescriptor.Return.ITaskCtor;
            return ctor.Invoke(new object[] { httpApiConfig, apiActionDescriptor }) as ApiTask;
        }

        /// <summary>
        /// 创建请求任务
        /// 返回请求结果
        /// </summary>
        /// <returns></returns>
        public abstract Task InvokeAsync();


        /// <summary>
        /// 表示Api请求的异步任务
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        private class ApiTaskOf<TResult> : ApiTask, ITask<TResult>
        {
            /// <summary>
            /// http接口配置
            /// </summary>
            private readonly DcpApiConfig ApiConfig;

            /// <summary>
            /// api描述
            /// </summary>
            private readonly ApiActionDescriptor apiActionDescriptor;

            /// <summary>
            /// Api请求的异步任务
            /// </summary>
            /// <param name="httpApiConfig">http接口配置</param>
            /// <param name="apiActionDescriptor">api描述</param>
            public ApiTaskOf(DcpApiConfig httpApiConfig, ApiActionDescriptor apiActionDescriptor)
            {
                this.ApiConfig = httpApiConfig;
                this.apiActionDescriptor = apiActionDescriptor;
            }

            /// <summary>
            /// 执行InvokeAsync
            /// 并返回其TaskAwaiter对象
            /// </summary>
            /// <returns></returns>
            public TaskAwaiter<TResult> GetAwaiter()
            {
                return this.RequestAsync().GetAwaiter();
            }

            /// <summary>
            /// 配置用于等待的等待者
            /// </summary>
            /// <param name="continueOnCapturedContext">试图继续回夺取的原始上下文，则为 true；否则为 false</param>
            /// <returns></returns>
            public ConfiguredTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
            {
                return this.RequestAsync().ConfigureAwait(continueOnCapturedContext);
            }
            /// <summary>
            /// 创建请求任务
            /// </summary>
            /// <returns></returns>
            public override Task InvokeAsync()
            {
                return this.RequestAsync();
            }

            /// <summary>
            /// 创建请求任务
            /// </summary>
            /// <returns></returns>
            Task<TResult> ITask<TResult>.InvokeAsync()
            {
                return this.RequestAsync();
            }

            /// <summary>
            /// 执行一次请求,远程提交
            /// </summary>
            /// <returns></returns>
            private async Task<TResult> RequestAsync()
            {
                //
                var context = new ApiActionContext
                {
                    ApiActionDescriptor = this.apiActionDescriptor,
                    ApiConfig = this.ApiConfig,
                    RequestMessage = new Handler.DcpRequestMessage { RequestUri = "this.ApiConfig.HttpHost" },
                    ResponseMessage = null,
                    Exception = null,
                    Result = null
                };
                await context.PrepareRequestAsync().ConfigureAwait(false);
                await context.ExecFiltersAsync(filter => filter.OnBeginRequestAsync).ConfigureAwait(false);
                var state = await context.ExecRequestAsync().ConfigureAwait(false);
                await context.ExecFiltersAsync(filter => filter.OnEndRequestAsync).ConfigureAwait(false);

                return state ? (TResult)context.Result : throw context.Exception;
            }
        }
    }
}
