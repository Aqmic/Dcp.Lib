﻿using Dcp.Net.MQ.Rpc.Contract;
using Dcp.Net.MQ.Rpc.Default;
using Dcp.Net.MQ.Rpc.Exceptions;
using Dcp.Net.MQ.Rpc.Handler;
using Dcp.Net.MQ.Rpc.Models;
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
        public string Exchange { get;protected set; }
        public string MqAddress { get;protected set; }
        public string ApplicationId { get;protected set; }
        public RpcServer CurrentRpcServer { get;private set; }
        /// <summary>
        /// Rpc入口管理
        /// </summary>
        /// <param name="mqAddress">mq连接地址</param>
        /// <param name="applicationId">当前应用id</param>
        /// <param name="exchange">交换机可以不填，默认RPC_EXCHANGE</param>
        public RpcManager(string mqAddress,string applicationId,string exchange=null)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentNullException("applicationId【业务应用id不能为空】应用id不能为空！");
            }
            if (string.IsNullOrEmpty(mqAddress))
            {
                throw new ArgumentNullException("mqAddress【MQ访问连接】应用id不能为空！");
            }
            this.MqAddress = mqAddress;
            if (string.IsNullOrEmpty(exchange))
            {
                this.Exchange = "RPC_EXCHANGE";
            }
        }
        public  void CreateClient()
        {
            DcpApiClientProxy.Init(new DcpApiConfig()
            {
                MqAddress = this.MqAddress
            });
        }
        public void RegisterAssembly(Assembly dcpContractAssembly)
        {
            var defaultRegisterService=IocUnity.Get<DefaultRegisterService>();
            if (defaultRegisterService == null)
            {
                defaultRegisterService = new DefaultRegisterService();
                IocUnity.AddSingleton<DefaultRegisterService>(defaultRegisterService);
            }
            defaultRegisterService.RegisterAssembly(dcpContractAssembly);
        }
        public  string GetCurrentServerRpcName()
        {
            return $"Rpc_Service_Queque_{ApplicationId}";
        }
        public  RpcServer StartServer()
        {
            DistributedMQConfig distributedMQConfig = new DistributedMQConfig
            {
                ServerAddress = this.MqAddress,
                Exchange = this.Exchange,
                ProducerID=GetCurrentServerRpcName(),
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
        private  void RpcServer_ReciveMsgedEvent(MQMessage mQMessage)
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
                RpcRemotingException rpcRemotingException = new RpcRemotingException();
                dcpResponseMessage.RemotingException = rpcRemotingException;
                dcpResponseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            mQMessage.Body = Dynamic.Core.Runtime.SerializationUtility.ToBytes(dcpResponseMessage);
            CurrentRpcServer.Send(mQMessage);
        }
    }
}