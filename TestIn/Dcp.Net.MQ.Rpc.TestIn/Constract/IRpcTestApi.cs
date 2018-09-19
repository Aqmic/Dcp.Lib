using Dcp.Net.MQ.Rpc.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.Rpc.TestIn.Constract
{
    public interface IRpcTestApi: IDcpApi
    {
       Task<string> ConsoleTest();
    }
}
