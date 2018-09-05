﻿namespace Dcp.Net.MQ.Rpc.TestIn
{
    using Dcp.Net.MQ.Rpc;
    using Dynamic.Core.Comm;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;

    internal class Program_test
    {
        private static RpcClient _rpcClient;
        private static RpcServer _rpcServer;

        private static void _rpcClient_ReciveMsgedEvent(MQMessage mQMessage)
        {
        }

        private static RpcClient GetClient() => 
            new RpcClient(new DistributedMQConfig { 
                ServerAddress = "amqp连接地址",
                Topic = "RPC_EXCHANGE",
                ProducerID = "Rpc_Response_Queue",
                ConsumerID = "Rpc_Response_RouteKey",
                MsgSendType = MessageSendType.P2P,
                IsDurable = false
            }, null);

        private static void Main2(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            HttpClient client = new HttpClient();
            client.GetStringAsync("http://www.baidu.com").Wait();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            _rpcServer = StartServer();
            _rpcClient = GetClient();
            _rpcClient.ReciveMsgedEvent -= new ReciveMQMessageHandler(Program_test._rpcClient_ReciveMsgedEvent);
            _rpcClient.ReciveMsgedEvent += new ReciveMQMessageHandler(Program_test._rpcClient_ReciveMsgedEvent);
            int num = 0;
            while (Console.ReadLine() != "0")
            {
                int num2 = 100;
                Stopwatch stopwatch2 = new Stopwatch();
                stopwatch2.Start();
                for (int i = 0; i < num2; i++)
                {
                    Stopwatch stopwatch3 = new Stopwatch();
                    stopwatch3.Start();
                    Console.WriteLine($"{(int) _rpcClient.GetReplyCount()}_{(int) num}");
                    string sendObj = "我是测试数据" + IdentityHelper.NewSequentialGuid().ToString("N");
                    string str2 = _rpcClient.Call<string>(sendObj, 0x3e8);
                    stopwatch3.Stop();
                    Console.WriteLine($"exucte time {(long) stopwatch3.ElapsedMilliseconds}");
                    Console.WriteLine($"发送数据=》【{sendObj}】" + ((int) i));
                    Console.WriteLine($"接收数据=>【{str2}】" + ((int) i));
                }
                stopwatch2.Stop();
                Console.WriteLine($"{(int) num2}执行耗时{(long) stopwatch2.ElapsedMilliseconds}");
            }
            Console.WriteLine("Hello World!");
        }

        private static void RpcServer_ReciveMsgedEvent(MQMessage mQMessage)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(mQMessage.Body) + "_我是服务端的回复哈");
            mQMessage.Body = bytes;
            _rpcServer.Send(mQMessage);
            Console.WriteLine("RpcServer收到消息！");
        }

        private static RpcServer StartServer()
        {
            DistributedMQConfig distributedMQConfig = new DistributedMQConfig {
                ServerAddress = "amqp连接地址",
                Topic = "RPC_EXCHANGE",
                ProducerID = "Rpc_Request_Queque",
                ConsumerID = "Rpc_Request_RouteKey",
                MsgSendType = MessageSendType.P2P,
                IsDurable = false
            };
            RpcServer server = new RpcServer(distributedMQConfig);
            server.ReciveMsgedEvent -= new ReciveMQMessageHandler(Program_test.RpcServer_ReciveMsgedEvent);
            server.ReciveMsgedEvent += new ReciveMQMessageHandler(Program_test.RpcServer_ReciveMsgedEvent);
            return server;
        }
    }
}
