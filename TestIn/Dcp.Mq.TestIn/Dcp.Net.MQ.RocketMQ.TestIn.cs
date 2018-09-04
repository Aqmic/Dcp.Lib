using Dcp.Mq.TestIn.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcp.Mq.TestIn
{
    [TestClass]
    public class RocketMqTest
    {
        RocketMqMessageQueue rocketMqMessageQueue;
        public RocketMqTest()
        {
            rocketMqMessageQueue = new RocketMqMessageQueue();
        }
        [TestMethod]
        public void Send()
        {
            rocketMqMessageQueue.Send();
           
        }
    }
}
