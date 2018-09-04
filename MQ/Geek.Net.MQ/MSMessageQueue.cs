//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Text.RegularExpressions;
//using System.IO;
//using System.Threading;


//namespace Geek.Net.MQ
//{
//    /// <summary>
//    /// 微软消息队列
//    /// </summary>
//    public class MSMessageQueue : MessageQueueBase
//    {
//        MessageQueue msMQ = null;
//        string mqPath = "";
//        bool isOS = false;
//        string randomGuid = Guid.NewGuid().ToString().ToLower();   

//        public MSMessageQueue(string mqName, string mqServer, bool isPrivate) :
//            base(mqName, mqServer, isPrivate)
//        {
//            Regex regEx = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");
//            if (!regEx.IsMatch(mqServer))
//            {
//                isOS = true;
//                mqPath = String.Format(@"FormatName:DIRECT=OS:{0}\{1}{2}", MQServerIP, IsPrivate ? @"PRIVATE$\" : "", MQName);
//            }
//            else
//            {
//                isOS = false;
//                mqPath = String.Format(@"FormatName:DIRECT=TCP:{0}\{1}{2}", MQServerIP, IsPrivate ? @"PRIVATE$\" : "", MQName);
//            }
//            msMQ = new MessageQueue(mqPath, true);
            
//            msMQ.Formatter = new BinaryMessageFormatter();
//        }

//        public override bool Send(string message, string label)
//        {
//            MessageQueue myQueue = new MessageQueue(mqPath);
//            Message myMessage = new Message();
//            myQueue.Formatter = new ActiveXMessageFormatter();
//            myMessage.Formatter = new System.Messaging.ActiveXMessageFormatter();
//            myMessage.Body = message;
//            myMessage.Label = label;
//            myQueue.Send(myMessage);
//            return true;
//        }

//        /// <summary>
//        /// MSMQ不支持异步发送和超时设置
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="label"></param>
//        /// <param name="isAsync"></param>
//        /// <param name="timeOut"></param>
//        /// <returns></returns>
//        public override bool Send(string message, string label, bool isAsync, long timeOut)
//        {
//            return Send(message, label);
//        }

//        public override MQMessage ReceiveMQ()
//        {
//            Message msg = msMQ.Receive();
//            BinaryReader reader = new BinaryReader(msg.BodyStream);
//            string msgBody = new string(System.Text.Encoding.Unicode.GetChars(reader.ReadBytes((int)msg.BodyStream.Length)));
//            return new MQMessage(msgBody, msg.Label); 
//        }

//        public override MQMessage ReceiveMQ(int timeOut)
//        {
//            if (timeOut <= 0)
//                return ReceiveMQ();
//            try
//            {

//                Message msg = msMQ.Receive(new TimeSpan(0, 0, timeOut));
//                BinaryReader reader = new BinaryReader(msg.BodyStream);
//                string msgBody = new string(System.Text.Encoding.Unicode.GetChars(reader.ReadBytes((int)msg.BodyStream.Length)));
//                if (msg.Label == "CONTROL" && msgBody == randomGuid)
//                {
//                    return null;
//                }
//                return new MQMessage(msgBody, msg.Label);
//            }
//            catch (MessageQueueException e)
//            {
//                if (e.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
//                    return null;
//                if (e.MessageQueueErrorCode == MessageQueueErrorCode.SharingViolation)
//                {
//                    Thread.Sleep(timeOut);
//                    return null;
//                }
//                if (e.MessageQueueErrorCode == MessageQueueErrorCode.QueueDeleted)
//                {
//                    CreateMQ();
//                    return null;
//                }
//                throw;
//            }
//        }

//        public override bool CreateMQ()
//        {
//            if (!isOS)
//                return false;
//            string mqName = String.Format(@"{0}\{1}{2}", MQServerIP, IsPrivate ? @"PRIVATE$\" : "", MQName);

//            if (!MessageQueue.Exists(mqName))
//            {
//                msMQ = MessageQueue.Create(mqName);
//                msMQ.DenySharedReceive = true;
//                return true;
//            }

//            return true;
//        }

//        public override bool CloseMQ()
//        {

//            if (msMQ != null)
//            {
//                Send(randomGuid, "CONTROL");
//                msMQ.Close();
//            }

//            return true;

//        }
//    }
//}
