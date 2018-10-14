using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Extions
{
    public static class DcpApiConfigExtion
    {
        public static void BatInitProperty(this DcpApiConfig dcpApiConfig, DcpApiConfig dcpApiConfigNew)
        {
            dcpApiConfig.Exchange = dcpApiConfigNew.Exchange;
            dcpApiConfig.MqAddress = dcpApiConfigNew.MqAddress;
        }
    }
}
