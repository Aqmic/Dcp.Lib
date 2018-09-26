using Dynamic.Core.Runtime;
using Geek.Net.MQ;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Geek.Net.MQ.Extions
{
    public static class MQMessageExtion
    {
       
        public static byte[] ToData(this MQMessage message)
        {
            byte[] buffer;
            buffer = SerializationUtility.ToBytes(message);
            return buffer;
        }
        
        public static MQMessage ToMessage(this byte[] data)
        {
            MQMessage message;
            if (data == null)
            {
                return null;
            }
            else
            {
                message = SerializationUtility.BytesToObject<MQMessage>(data);
            }   
            return message;
        }

        
        public static byte[] ToUTF8Bytes(this string utf8String)
        {
            byte[] buffer;
            buffer = Encoding.UTF8.GetBytes(utf8String);
            return buffer;
        }

        
        public static string ToUTF8String(this byte[] utf8Bytes)
        {
            string str;
            str = Encoding.UTF8.GetString(utf8Bytes);
            return str;
        }
    }
}

