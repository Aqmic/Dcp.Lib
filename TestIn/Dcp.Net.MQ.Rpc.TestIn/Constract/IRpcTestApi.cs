using Dcp.Net.MQ.Rpc.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.TestIn.Constract
{
    public interface IRpcTestApi: IDcpApi
    {
        void ConsoleTest();
    }
}
