﻿namespace Dcp.Net.MQ.Rpc.TestIn
{
    using Dcp.Net.MQ.Rpc;
    using Dcp.Net.MQ.Rpc.Default;
    using Dcp.Net.MQ.Rpc.Exceptions;
    using Dcp.Net.MQ.Rpc.Handler;
    using Dcp.Net.MQ.Rpc.TestIn.Constract;
    using Dcp.Net.MQ.Rpc.TestIn.RpcTest;
    using Dynamic.Core.Comm;
    using Dynamic.Core.Models;
    using Dynamic.Core.Service;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Text;

    internal class Program
    {
        private static RpcClient _rpcClient;
        private static RpcServer _rpcServer;

        //private static string _mqAddress = "amqp://icb:icb158@10.10.10.2:13043/";// "amqp://icb:icb158@220.167.101.49:13043/";// File.ReadAllText(@"d:\mqaddress.txt");
        private static string _mqAddress = "amqp://icb:icb158@220.167.101.49:13043/";// File.ReadAllText(@"d:\mqaddress.txt");


        private static void _rpcClient_ReciveMsgedEvent(MQMessage mQMessage)
        {

        }

        private static RpcClient GetClient() => 
            new RpcClient(new DistributedMQConfig { 
                ServerAddress = _mqAddress,
                Exchange = "RPC_EXCHANGE",
                ProducerID = "Rpc_Response_Queue",
                ConsumerID = "Rpc_Response_RouteKey",
                MsgSendType = MessageSendType.Worker,
                IsDurable = false
            }, null);
        static async void RunIUserApi() {
            _rpcServer = StartServer();
            Console.WriteLine("start-server-ok");
            RpcDemo rpcDemo = new RpcDemo();
            var abc = await rpcDemo.TestIn("-1");
            
            while (Console.ReadLine() != "exit")
            {
                Console.Clear();
                Stopwatch stopwatch = new Stopwatch();
                for (int i = 0; i < 1000; i++)
                {
                    stopwatch.Reset();
                    stopwatch.Start();
                     rpcDemo = new RpcDemo();
                     abc = await rpcDemo.TestIn(i.ToString());
                    stopwatch.Stop();
                    Console.WriteLine(i + "SEND:"+stopwatch.ElapsedMilliseconds);
                }
                
            }
        }
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(DateTime.Now);
                Console.ReadLine();
              
                DefaultRegisterService defaultRegisterService = new DefaultRegisterService();
                defaultRegisterService.RegisterAssembly(typeof(Program).Assembly);

               

                IocUnity.AddSingleton<DefaultRegisterService>(defaultRegisterService);
              
                RunIUserApi();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
          
        }

        private static void RpcServer_ReciveMsgedEvent(MQMessage mQMessage)
        {
            var msgRequest = Dynamic.Core.Runtime.SerializationUtility.BytesToObject<DcpRequestMessage>(mQMessage.Body);
            DefaultRegisterService defaultRegisterService=IocUnity.Get<DefaultRegisterService>();
            object resultObj = null;
            DcpResponseMessage dcpResponseMessage = new DcpResponseMessage();
            try
            {
               resultObj=defaultRegisterService.CallAction(msgRequest.ActionInfo);
            }
            catch (Exception ex)
            {
                RpcRemotingException rpcRemotingException = new RpcRemotingException();
                dcpResponseMessage.RemotingException = rpcRemotingException;
                
            }
          

            mQMessage.Body = Dynamic.Core.Runtime.SerializationUtility.ToBytes(dcpResponseMessage);
            _rpcServer.Send(mQMessage);
        }

        private static RpcServer StartServer()
        {
            DistributedMQConfig distributedMQConfig = new DistributedMQConfig {
                ServerAddress = _mqAddress,
                Exchange = "RPC_EXCHANGE",
                ProducerID = "Rpc_Service_Queque" +"1233333",
                //ConsumerID = "Rpc_Request_RouteKey",
                MsgSendType = MessageSendType.Router,
                IsDurable = false
            };
            var routeKeyList= IocUnity.Get<DefaultRegisterService>().GetRouteKeyList();
            RpcServer server = new RpcServer(distributedMQConfig, routeKeyList, "testRpcServer_7AEEEE78-D076-4047-8992-229D96263CBD");
            server.ReciveMsgedEvent -= new ReciveMQMessageHandler(Program.RpcServer_ReciveMsgedEvent);
            server.ReciveMsgedEvent += new ReciveMQMessageHandler(Program.RpcServer_ReciveMsgedEvent);
            return server;
        }
    }
}

