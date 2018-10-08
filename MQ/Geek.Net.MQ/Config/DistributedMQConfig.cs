namespace Geek.Net.MQ.Config
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class DistributedMQConfig
    {


        public DistributedMQConfig()
        {

            this.ConsumeThreadNums = Environment.ProcessorCount * 2;
            this.DefaultEncoding = Encoding.UTF8;
            this.Name = base.GetType().GetType().FullName;
            this.AutoCreate = true;
            this.AutoDelete = true;
            this.IsDurable = true;
            this.MsgSendType = 0;
            return;
        }

        public string AccessKey
        {
            get; set;
        }

        public bool AutoCreate
        {
            get; set;
        }

        public bool AutoDelete
        {
            get; set;
        }

        public string Channel
        {
            get; set;
        }

        public string ConsumerID
        {
            get; set;
        }

        public int ConsumeThreadNums
        {
            get; set;
        }

        public Encoding DefaultEncoding
        {
            get; set;
        }

        public bool IsDurable
        {
            get; set;
        }

        public bool IsPrivate
        {
            get; set;
        }

        public bool IsWriteLog
        {
            get; set;
        }

        public MessageSendType MsgSendType
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string ProducerID
        {
            get; set;
        }

        public string SecretKey
        {
            get; set;
        }

        public string ServerAddress
        {
            get; set;
        }

        public int TimeOut
        {
            get; set;
        }

        public string Exchange
        {
            get; set;
        }
    }
}

