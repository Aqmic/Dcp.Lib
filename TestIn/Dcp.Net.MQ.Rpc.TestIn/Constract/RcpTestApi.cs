using Dynamic.Core.Auxiliary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.TestIn.Constract
{
    public class RcpTestApi : IRpcTestApi
    {
        public void ConsoleTest()
        {
            IOHelper.WriteLine($"{DateTime.Now}=>我被调用了");
        }

        public void Dispose()
        {
          //  throw new NotImplementedException();
        }
    }
}
