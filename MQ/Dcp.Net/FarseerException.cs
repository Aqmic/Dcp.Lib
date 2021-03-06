﻿using System;
using System.Runtime.Serialization;

namespace Dcp.Net
{
    /// <summary>
    /// 系统异常基类
    /// </summary>
    [Serializable]
    public class DcpException : System.Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DcpException() { }
#if !CORE
		/// <summary>
		/// 构造函数
		/// </summary>
		public DcpException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        public DcpException(string message) : base(message) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">异常堆栈</param>
        public DcpException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// 实体化DcpException
        /// </summary>
        public static DcpException Instance(string message) => new DcpException(message);
    }
}
