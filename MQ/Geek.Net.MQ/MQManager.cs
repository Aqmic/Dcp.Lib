//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;

//namespace Geek.Net.MQ
//{
//    public class MQManager
//    {
//        IMessageQueue mq = null;
//        Thread receiveThread = null;
//        bool isRunning = false;

//        public MessageQueueTypeEnum MQType { get; private set; }

//        /// <summary>
//        /// 循环接收消息时的间隔时间(毫秒)
//        /// </summary>
//        public int PauseTime { get; set; }

//        public event EventHandler<MQReceivedEventArgs> ReceivedMessage;
//        public event EventHandler StartReceive;
//        public event EventHandler StopReceive;

//        public MQManager(MessageQueueTypeEnum mqType, string mqName, string mqServerIP, bool isPrivate)
//        {
//            MQType = mqType;

//            if (mqType == MessageQueueTypeEnum.GeekMQ)
//            {
//              //  mq = new MSMessageQueue(mqName, mqServerIP, isPrivate);
//            }
//            else if (mqType == MessageQueueTypeEnum.GeekMQ)
//            {
//             //   mq = new GeekMessageQueue(mqName, mqServerIP, isPrivate);
//            }
//            else
//            {
//                throw new NotSupportedException("不支持此类型的消息队列!");
//            }
//        }

//        public bool SendMessage(string message, string label)
//        {
//            return mq.Send(message, label);
//        }

//        public bool SendMessage(string message, string label, bool isAync)
//        {
//            return mq.Send(message, label, isAync, 1);
//        }

//        public bool CreateMQ()
//        {
//            return mq.CreateMQ();
//        }

//        public bool IsReceiving
//        {
//            get { return isRunning; }
//        }

//        public bool StartReceiveMessage()
//        {
//            if (receiveThread == null)
//            {
//                //创建队列
//                bool isSuccess = mq.CreateMQ();
//                if (!isSuccess)
//                    return false;
//                receiveThread = new Thread(receiveMQ);
//                receiveThread.IsBackground = true;
//                receiveThread.Name = "接收队列消息_" + mq.MQConfig.Topic;
//                isRunning = true;
//                receiveThread.Start();

//                if (StartReceive != null)
//                {
//                    StartReceive(this, EventArgs.Empty);
//                }
//            }
//            return true;
//        }

//        public bool StopReceiveMessage()
//        {
//            isRunning = false;
//            if (receiveThread != null)
//            {
//                try
//                {
//                    if (mq != null)
//                    {
//                        mq.CloseMQ();
//                        Thread.Sleep(100);
//                    }
//                    receiveThread.Abort();
//                }
//                catch (ThreadAbortException e)
//                {
//                    receiveThread = null;
//                }

//                if (StopReceive != null)
//                {
//                    StopReceive(this, EventArgs.Empty);
//                }
//            }

//            return true;
//        }

//        private void receiveMQ()
//        {
//            while (isRunning)
//            {
//                MQMessage msg = mq.ReceiveMQ(30);
//                if (msg != null && ReceivedMessage != null)
//                {
//                    MQReceivedEventArgs args = new MQReceivedEventArgs(msg.Body, msg.Label);
//                    ReceivedMessage(this, args);
//                }
//                if (PauseTime > 0)
//                {
//                    Thread.Sleep(PauseTime);
//                }
//            }
//        }
//    }
//}
