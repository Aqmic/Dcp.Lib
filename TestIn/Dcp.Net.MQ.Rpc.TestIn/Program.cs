namespace Dcp.Net.MQ.Rpc.TestIn
{
    using Dcp.Net.MQ.Rpc;
    using Dcp.Net.MQ.Rpc.Default;
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
                // Console.ReadLine();
                Console.WriteLine("=======================22222");
                RunIUserApi();
                
                //return;

                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();
                //HttpClient client = new HttpClient();
                //client.GetStringAsync("http://www.baidu.com").Wait();
                //stopwatch.Stop();
                //Console.WriteLine(stopwatch.ElapsedMilliseconds);
                //_rpcServer = StartServer();
                //_rpcClient = GetClient();
                //_rpcClient.ReciveMsgedEvent -= new ReciveMQMessageHandler(Program._rpcClient_ReciveMsgedEvent);
                //_rpcClient.ReciveMsgedEvent += new ReciveMQMessageHandler(Program._rpcClient_ReciveMsgedEvent);
                //int num = 0;
                //while (Console.ReadLine() != "0")
                //{
                //    int num2 = 100;
                //    Stopwatch stopwatch2 = new Stopwatch();
                //    stopwatch2.Start();
                //    for (int i = 0; i < num2; i++)
                //    {
                //        Stopwatch stopwatch3 = new Stopwatch();
                //        stopwatch3.Start();
                //        Console.WriteLine($"{(int)_rpcClient.GetReplyCount()}_{(int)num}");
                //        string sendObj = "我是测试数据" + IdentityHelper.NewSequentialGuid().ToString("N");
                //        string str2 = _rpcClient.Call<string>(sendObj, 0x3e8);
                //        stopwatch3.Stop();
                //        Console.WriteLine($"exucte time {(long)stopwatch3.ElapsedMilliseconds}");
                //        Console.WriteLine($"发送数据=》【{sendObj}】" + ((int)i));
                //        Console.WriteLine($"接收数据=>【{str2}】" + ((int)i));
                //    }
                //    stopwatch2.Stop();
                //    Console.WriteLine($"{(int)num2}执行耗时{(long)stopwatch2.ElapsedMilliseconds}");
                //}
                //Console.WriteLine("Hello World!");

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
            try
            {
                defaultRegisterService.CallAction(msgRequest.ActionInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            DcpResponseMessage dcpResponseMessage = new DcpResponseMessage() {
                RemotingException = new Exceptions.RpcRemotingException() {
                    Source = "测试异常",
                },
                Result = new ResultModel() {
                    data = "测试",
                    state = true
                },
            };
            mQMessage.Body = Dynamic.Core.Runtime.SerializationUtility.ToBytes(dcpResponseMessage);
            _rpcServer.Send(mQMessage);
            Console.WriteLine(msgRequest.ActionInfo.MethodName);
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

