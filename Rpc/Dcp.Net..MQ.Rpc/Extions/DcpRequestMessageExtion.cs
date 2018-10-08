using Dcp.Net.MQ.Rpc.Handler;
using Dynamic.Core.Comm;
using Dynamic.Core.Runtime;
using Geek.Net.MQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Extions
{
    public static class DcpRequestMessageExtion
    {
        public static MQMessage ToMQMessage(this DcpRequestMessage dcpRequestMessage)
        {
            MQMessage mQMessage = null;
            if (dcpRequestMessage != null)
            {
                var bytes = SerializationUtility.ToBytes(dcpRequestMessage);
                mQMessage = new MQMessage();
                mQMessage.Body = bytes;
                mQMessage.MsgId = IdentityHelper.NewSequentialGuid().ToString("N");
                mQMessage.Request = new MQMsgRequest()
                {
                    RequestRouteKey = dcpRequestMessage.ActionInfo.TargetTypeFullName
                };
            }
            return mQMessage;
        }
    }
}
