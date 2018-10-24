using Dcp.Net.MQ.Rpc.Config;
using Dcp.Net.MQ.Rpc.Contract;
using Dcp.Net.MQ.Rpc.Default;
using Dcp.Net.MQ.Rpc.Exceptions;
using Dcp.Net.MQ.Rpc.Handler;
using Dcp.Net.MQ.Rpc.Models;
using Dynamic.Core.Log;
using Dynamic.Core.Models;
using Dynamic.Core.Service;
using Geek.Net.MQ;
using Geek.Net.MQ.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc
{
    public class RpcManager
    {
        ILogger _logger = LoggerManager.GetLogger("RpcManager");
        public MqRpcConfig MqRpcConfig { get; set; }
        public string ApplicationId { get; protected set; }
        public RpcServer CurrentRpcServer { get; private set; }
        /// <summary>
        /// Rpc入口管理
        /// </summary>
        /// <param name="mqAddress">mq连接地址</param>
        /// <param name="applicationId">当前应用id</param>
        /// <param name="exchange">交换机可以不填，默认RPC_EXCHANGE</param>
        public RpcManager(MqRpcConfig mqRpcConfig)
        {
            if (mqRpcConfig == null)
            {
                throw new ArgumentNullException("MqRpcConfig不能为空！");
            }
            this.MqRpcConfig = mqRpcConfig;
            if (string.IsNullOrEmpty(mqRpcConfig.ApplicationId))
            {
                throw new ArgumentNullException("ApplicationId【业务应用id不能为空】应用id不能为空！");
            }
            if (string.IsNullOrEmpty(mqRpcConfig.MqAddress))
            {
                throw new ArgumentNullException("MqAddress【MQ访问连接】应用id不能为空！");
            }

            if (string.IsNullOrEmpty(this.MqRpcConfig.Exchange))
            {
                this.MqRpcConfig.Exchange = "RPC_EXCHANGE";
            }
            this.ApplicationId = this.MqRpcConfig.ApplicationId;
        }
        /// <summary>
        /// 创建代理客户端，可以多次会自动判断，是否已经初始化过
        /// </summary>
        public void CreateClient()
        {

            DcpApiClientProxy.Init(new DcpApiConfig()
            {
                MqAddress = this.MqRpcConfig.MqAddress,
                TimeOut = this.MqRpcConfig.RequestTimeOut
            });

        }
        public void RegisterType(Type interfaceConstract, Type constractService)
        {
            var defaultRegisterService = IocUnity.Get<DefaultRegisterService>();
            if (defaultRegisterService == null)
            {
                defaultRegisterService = new DefaultRegisterService();
                IocUnity.AddSingleton<DefaultRegisterService>(defaultRegisterService);
            }
            defaultRegisterService.RegisterType(interfaceConstract, constractService);
        }
        public void RegisterAssembly(Assembly dcpContractAssembly)
        {
            var defaultRegisterService = IocUnity.Get<DefaultRegisterService>();
            if (defaultRegisterService == null)
            {
                defaultRegisterService = new DefaultRegisterService();
                IocUnity.AddSingleton<DefaultRegisterService>(defaultRegisterService);
            }
            defaultRegisterService.RegisterAssembly(dcpContractAssembly);
        }
        public string GetCurrentServerRpcName()
        {
            return $"Rpc_Service_Queque-{ApplicationId}";
        }
        public RpcServer StartServer()
        {
            DistributedMQConfig distributedMQConfig = new DistributedMQConfig
            {
                ServerAddress = this.MqRpcConfig.MqAddress,
                Exchange = this.MqRpcConfig.Exchange,
                ProducerID = GetCurrentServerRpcName(),
                MsgSendType = MessageSendType.Router,
                IsDurable = false
            };
            var routeKeyList = IocUnity.Get<DefaultRegisterService>().GetRouteKeyList();
            RpcServer server = new RpcServer(distributedMQConfig, routeKeyList, null);
            server.ReciveMsgedEvent -= new ReciveMQMessageHandler(RpcServer_ReciveMsgedEvent);
            server.ReciveMsgedEvent += new ReciveMQMessageHandler(RpcServer_ReciveMsgedEvent);
            CurrentRpcServer = server;
            return server;
        }
        private void RpcServer_ReciveMsgedEvent(MQMessage mQMessage)
        {
            var msgRequest = Dynamic.Core.Runtime.SerializationUtility.BytesToObject<DcpRequestMessage>(mQMessage.Body);
            DefaultRegisterService defaultRegisterService = IocUnity.Get<DefaultRegisterService>();
            object resultObj = null;
            DcpResponseMessage dcpResponseMessage = new DcpResponseMessage();
            try
            {
                resultObj = defaultRegisterService.CallAction(msgRequest.ActionInfo);
                //var genericType = resultObj.GetType();
                //var isTaskType = genericType == typeof(Task<>) || genericType == typeof(ITask<>);
                //if (isTaskType)
                //{
                //    var taskResult = resultObj as Task<object>;
                //    dcpResponseMessage.Result = taskResult.Result;
                //}
                //var resultType=Type.GetType(msgRequest.ActionInfo.RtnInfo.DataTypeFullName);
                var taskResult = resultObj as Task;
                if (taskResult != null && msgRequest.ActionInfo.RtnInfo != null)
                {
                    //异步任务又返回值操作情况
                    dynamic dynamicResult = resultObj;
                    dcpResponseMessage.Result = dynamicResult.Result;
                }
                else
                {
                    dcpResponseMessage.Result = resultObj;
                }
                dcpResponseMessage.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                RpcRemotingException rpcRemotingException = new RpcRemotingException() {
                    CallInfo= msgRequest.ActionInfo,                    
                    Source = ex.ToString(),
                };
                dcpResponseMessage.RemotingException = rpcRemotingException;
                dcpResponseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            mQMessage.Body = Dynamic.Core.Runtime.SerializationUtility.ToBytes(dcpResponseMessage);
            CurrentRpcServer.Send(mQMessage);
        }
    }
}
