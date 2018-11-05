using Dcp.Net.MQ.Rpc.Aop;
using Dcp.Net.MQ.Rpc.Core;
using Dcp.Net.MQ.Rpc.Extions;
using Dcp.Net.MQ.Rpc.Handler.Internal;
using Dynamic.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Contract
{
    /// <summary>
    /// 表示HttpApi客户端
    /// 提供创建HttpApiClient实例的方法
    /// </summary>
    public partial class DcpApiClientProxy
    {
        private static readonly object _lockObj = new object();
        private static DcpApiConfig _DefaultConfig;

        static volatile  bool IsInitSuccess;

        /// <summary>
        /// 判断代理客户段是否进行了初始化操作
        /// </summary>
        /// <returns></returns>
        public static bool GetInitStatus()
        {
            return IsInitSuccess;
        }
        public static void Reset()
        {
            lock (_lockObj)
            {
                IsInitSuccess = false;
            }
        }
        public static void Init(DcpApiConfig dcpApiConfig)
        {
            lock (_lockObj)
            {
                if (IsInitSuccess)
                {
                    return;
                }
                _DefaultConfig = dcpApiConfig;
                if (_DefaultConfig == null)
                {
                    _DefaultConfig = new DcpApiConfig();
                }
                if (string.IsNullOrEmpty(_DefaultConfig.Exchange))
                {
                    throw new ArgumentNullException("Exhange交换机值不能为空！");
                }
                if (string.IsNullOrEmpty(_DefaultConfig.MqAddress))
                {
                    throw new ArgumentNullException("MqAddress不能为空！");
                }
                IsInitSuccess = true;
            }
        }
        /// <summary>
        /// 一个站点内的默认连接数限制
        /// </summary>
        private static int connectionLimit = 128;

#if NETCOREAPP2_1
        /// <summary>
        /// 使用SocketsHttpHandler开关项的名称
        /// </summary>
        private const string useSocketsHttpHandlerSwitch = "System.Net.Http.UseSocketsHttpHandler";

        /// <summary>
        /// 获取或设置HttpClientHandler是否包装和使用SocketsHttpHandler
        /// </summary>
        public static bool UseSocketsHttpHandler
        {
            get
            {
                return AppContext.TryGetSwitch(useSocketsHttpHandlerSwitch, out bool isEnabled) && isEnabled;
            }
            set
            {
                AppContext.SetSwitch(useSocketsHttpHandlerSwitch, value);
            }
        }
#endif

        /// <summary>
        /// 获取或设置一个站点内的默认连接数限制
        /// 这个值在初始化HttpClientHandler时使用
        /// 默认值为128
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int ConnectionLimit
        {
            get
            {
                return connectionLimit;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                connectionLimit = value;
            }
        }

        /// <summary>
        /// 创建实现了指定接口的HttpApiClient实例
        /// </summary>
        /// <typeparam name="TInterface">请求接口类型</typeparam>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <returns></returns>
        public static TInterface Create<TInterface>(bool isForceProxy=false) where TInterface : class, IDcpApi
        {
            var config = new DcpApiConfig();
            config.BatInitProperty(_DefaultConfig);

            if (!isForceProxy)
            {
                var dcpService = IocUnity.Get<TInterface>();
                if (dcpService != null)
                {
                    return dcpService;
                }
            }
            return Create<TInterface>(config);
        }

        /// <summary>
        /// 创建实现了指定接口的HttpApiClient实例
        /// </summary>
        /// <typeparam name="TInterface">请求接口类型</typeparam>
        /// <param name="httpHost">Http服务完整主机域名，如http://www.webapiclient.com</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <returns></returns>
        public static TInterface Create<TInterface>(string httpHost) where TInterface : class, IDcpApi
        {
            var config = new DcpApiConfig();
            config.BatInitProperty(_DefaultConfig);
            if (string.IsNullOrEmpty(httpHost) == false)
            {
                config.HttpHost = new Uri(httpHost, UriKind.Absolute);
            }
            return Create<TInterface>(config);
        }

        /// <summary>
        /// 创建实现了指定接口的HttpApiClient实例
        /// </summary>
        /// <typeparam name="TInterface">请求接口类型</typeparam>
        /// <param name="httpApiConfig">接口配置</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <returns></returns>
        public static TInterface Create<TInterface>(DcpApiConfig dcpApiConfig) where TInterface : class, IDcpApi
        {
            return Create(typeof(TInterface), dcpApiConfig) as TInterface;
        }

        /// <summary>
        /// 创建实现了指定接口的HttpApiClient实例
        /// </summary>
        /// <param name="interfaceType">请求接口类型</param>
        /// <param name="httpApiConfig">接口配置</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <returns></returns>
        public static object Create(Type interfaceType, DcpApiConfig httpApiConfig)
        {
            if (httpApiConfig == null)
            {
                throw new ArgumentNullException(nameof(httpApiConfig));
            }
            var interceptor = new ApiInterceptor(httpApiConfig);
            return Create(interfaceType, interceptor);
        }

        /// <summary>
        /// 创建实现了指定接口的HttpApiClient实例
        /// </summary>
        /// <param name="interfaceType">请求接口类型</param>
        /// <param name="apiInterceptor">http接口调用拦截器</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <returns></returns>
        public static object Create(Type interfaceType, IApiInterceptor apiInterceptor)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            if (apiInterceptor == null)
            {
                throw new ArgumentNullException(nameof(apiInterceptor));
            }

            return DcpApiClient.CreateInstance(interfaceType, apiInterceptor);
        }
    }
}
