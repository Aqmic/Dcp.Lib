namespace Dcp.Net.MQ.Rpc.TestIn
{
    using Acb.Contracts.IntegralShop;
    using Acb.Plugin.PrivilegeManage.Constract;
    using Dcp.Net.MQ.Rpc;
    using Dcp.Net.MQ.Rpc.Config;
    using Dcp.Net.MQ.Rpc.Contract;
    using Dcp.Net.MQ.Rpc.Default;
    using Dcp.Net.MQ.Rpc.Exceptions;
    using Dcp.Net.MQ.Rpc.Handler;
    using Dcp.Net.MQ.Rpc.TestIn.Constract;
    using Dcp.Net.MQ.Rpc.TestIn.RpcTest;
    using Dynamic.Core.Comm;
    using Dynamic.Core.Log;
    using Dynamic.Core.Models;
    using Dynamic.Core.Service;
    using Geek.Net.MQ;
    using Geek.Net.MQ.Config;
    using GlobPermissionVerificationPlugin.Constract;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Text;

    internal class Program
    {
        private static RpcClient _rpcClient;
        private static RpcServer _rpcServer;

        private static string _mqAddress = File.ReadAllText(@"d:\mqaddress.txt");
   
        private  static void Main(string[] args)
        {
            //日志初始化，必须要加这句（rpc内部依赖该日志组件）
            LoggerManager.InitLogger(new LogConfig() { });
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
                var rpcTestApi = DcpApiClientProxy.Create<IRpcTestApi>(true);

                var nativeTestInterface = rpcTestApi.Test(new UserInfo()
                {

                    Name = "asdfsadfsdf",
                    Des = "234234234"

                });
                var jj = nativeTestInterface.Result;



                Console.WriteLine(Dynamic.Core.Runtime.SerializationUtility.ObjectToJson(jj));


                while (Console.ReadLine() != "exit")
                {

                    try
                    {
                        var ewrer = DcpApiClientProxy.Create<IPrivilegeManageConstract>(true);
                        var wer333 = ewrer.GetUsersOfInfo(new Acb.Plugin.PrivilegeManage.Constract.Models.Dtos.User.UserQueryPageDto()
                        {
                            Page = 1,
                            Size = 1000,
                            userQuery = new Acb.Plugin.PrivilegeManage.Constract.Models.Dtos.User.UserQueryDto()
                            {
                                OpenId = "o9MuI0-fi_eL8aKSlyTc1Bgjpprs",
                                SystemId = "161722f8658fc420c26408d635cd7f66",
                                State = 1
                            }

                        });
                        var jjj = wer333.Result;
                        var JJJsTR = Dynamic.Core.Runtime.SerializationUtility.ObjectToJson(jjj);
                        Console.WriteLine(JJJsTR);
                    }
                    catch (Exception ex)
                    {
                        var ere = ex.ToString();
                        Console.WriteLine(ere);
                    }


                    var abc = DcpApiClientProxy.Create<IPrivilegeManageConstract>(true);
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

