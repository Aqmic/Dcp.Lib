using Dynamic.Core.Auxiliary;
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
    }
}
