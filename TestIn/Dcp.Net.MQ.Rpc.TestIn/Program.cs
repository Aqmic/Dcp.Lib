namespace Dcp.Net.MQ.Rpc.TestIn
{
    using Dcp.Net.MQ.Rpc;
    using Dcp.Net.MQ.Rpc.Config;
    using Dcp.Net.MQ.Rpc.Contract;
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
    using PolicyCalculator.Plugin.Contract;
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
   
        private  static void Main(string[] args)
        {
            try
            {
                MqRpcConfig mqRpcConfig = new MqRpcConfig() {
                    MqAddress=_mqAddress,
                    ApplicationId= "demo测试131243",
                    RequestTimeOut=30*1000
                };
                RpcManager rpcManager = new RpcManager(mqRpcConfig);
                rpcManager.RegisterAssembly(typeof(Program).Assembly);
                rpcManager.RegisterAssembly(typeof(Program).Assembly);
                rpcManager.StartServer();
                rpcManager.CreateClient();
                var rpcTestApi = DcpApiClientProxy.Create<IRpcTestApi>();



                var jsqApi = DcpApiClientProxy.Create<IPolicyCalculatorConstract>();
             


                while (Console.ReadLine()!="exit")
                {
                    var abc = jsqApi.GetAllBusinessPackage().Result;

                    foreach (var item in abc.Data)
                    {
                        Console.WriteLine(item.NiceName + $"【{item.Id}】");
                    }
                    Console.WriteLine();
                    var result=rpcTestApi.WriteLine("测试WriteLine方法=》" + DateTime.Now).Result;
                  Console.WriteLine("client"+result.data);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
          
        }

     
    }
}

