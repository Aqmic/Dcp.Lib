namespace Dcp.Net.MQ.Rpc.TestIn
{
    using Dcp.Net.MQ.Rpc;
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
                RpcManager rpcManager = new RpcManager(_mqAddress, "demo测试131243");
                rpcManager.RegisterAssembly(typeof(Program).Assembly);
                rpcManager.StartServer();
                rpcManager.CreateClient();
                var rpcTestApi = DcpApiClientProxy.Create<IRpcTestApi>();

                while (Console.ReadLine()!="exit")
                {
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

