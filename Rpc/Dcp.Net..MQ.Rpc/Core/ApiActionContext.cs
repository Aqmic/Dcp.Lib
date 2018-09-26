using Dcp.Net.MQ.Rpc.Aop;
using Dcp.Net.MQ.Rpc.Extions;
using Dcp.Net.MQ.Rpc.Handler;
using Dcp.Net.MQ.Rpc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.Core
{
    /// <summary>
    /// 表示请求Api的上下文
    /// </summary>
    public class ApiActionContext
    {
        /// <summary>
        /// 自定义数据的存储和访问容器
        /// </summary>
        private Tags tags;
        /// <summary>
        /// 获取本次请求相关的自定义数据的存储和访问容器
        /// </summary>
        public Tags Tags
        {
            get
            {
                if (this.tags == null)
                {
                    this.tags = new Tags();
                }
                return this.tags;
            }
        }

        /// <summary>
        /// 获取关联的HttpApiConfig
        /// </summary>
        public DcpApiConfig ApiConfig { get; internal set; }
        /// <summary>
        /// 获取关联的ApiActionDescriptor
        /// </summary>
        public ApiActionDescriptor ApiActionDescriptor { get; internal set; }
        /// <summary>
        /// 获取关联的HttpRequestMessage
        /// </summary>
        public DcpRequestMessage RequestMessage { get; internal set; }

        /// <summary>
        /// 获取关联的HttpResponseMessage
        /// </summary>
       public DcpResponseMessage ResponseMessage { get; internal set; }

        /// <summary>
        /// 获取调用Api得到的结果
        /// </summary>
        public object Result { get; internal set; }

        /// <summary>
        /// 获取调用Api产生的异常
        /// </summary>
        public Exception Exception { get; internal set; }
        
        /// <summary>
        /// 准备请求数据
        /// </summary>
        /// <returns></returns>
        internal async Task PrepareRequestAsync()
        {
            var apiAction = this.ApiActionDescriptor;
            var validateProperty = this.ApiConfig.UseParameterPropertyValidate;

            foreach (var parameter in apiAction.Parameters)
            {
                 ParameterValidator.Validate(parameter, validateProperty);
            }
            foreach (var actionAttribute in apiAction.Attributes)
            {
                await actionAttribute.BeforeRequestAsync(this).ConfigureAwait(false);
            }
            foreach (var parameter in apiAction.Parameters)
            {
                foreach (var parameterAttribute in parameter.Attributes)
                {
                    await parameterAttribute.BeforeRequestAsync(this, parameter).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// 执行请求
        /// 返回是否执行成功
        /// </summary>
        /// <returns></returns>
        internal async Task<bool> ExecRequestAsync()
        {
            try
            {
                var apiAction = this.ApiActionDescriptor;
                var client = this.ApiConfig.ApiClient;
                this.RequestMessage.ActionInfo=apiAction.ToActionSerDes();
                this.ResponseMessage = client.Call<DcpResponseMessage>(this.RequestMessage,this.ApiConfig.TimeOut); // await ApiClient.SendAsync(this.RequestMessage).ConfigureAwait(false);
                this.Result = await apiAction.Return.Attribute.GetTaskResult(this).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// 执行所有过滤器
        /// </summary>
        /// <param name="funcSelector">方法选择</param>
        /// <returns></returns>
        internal async Task ExecFiltersAsync(Func<IApiActionFilter, Func<ApiActionContext, Task>> funcSelector)
        {
            foreach (var filter in this.ApiConfig.GlobalFilters)
            {
                await funcSelector(filter)(this).ConfigureAwait(false);
            }
            foreach (var filter in this.ApiActionDescriptor.Filters)
            {
                await funcSelector(filter)(this).ConfigureAwait(false);
            }
        }
    }
}
