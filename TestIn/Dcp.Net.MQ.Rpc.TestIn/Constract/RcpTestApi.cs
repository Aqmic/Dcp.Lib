using Dynamic.Core.Auxiliary;
using Dynamic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.TestIn.Constract
{
    public class RcpTestApi : IRpcTestApi
    {
        public Task<string> ConsoleTest()
        {
            IOHelper.WriteLine($"{DateTime.Now}=>我被调用了");
            return null;
        }

        public void Dispose()
        {
            //  throw new NotImplementedException();
        }

        public Task<ResultModel> WriteLine(string contentStr)
        {
            return (new TaskFactory()).StartNew<ResultModel>(() =>
            {
                ResultModel resultModel = new ResultModel();
                Console.WriteLine(contentStr);
                resultModel.data = "来看看测试结果";
                resultModel.state = true;
                resultModel.msg = "ok";
                return resultModel;
            });

        }
    }
}
