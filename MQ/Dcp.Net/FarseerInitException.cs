using System;
using System.Runtime.Serialization;

namespace Dcp.Net
{
    /// <summary>
    ///     系统初始化异常
    /// </summary>
    [Serializable]
    public class DcpInitException : DcpException
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public DcpInitException() { }
#if !CORE
        /// <summary>
        ///     构造函数
        /// </summary>
        public DcpInitException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { }
#endif
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="message">Exception message</param>
        public DcpInitException(string message) : base(message) { }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DcpInitException(string message, Exception innerException) : base(message, innerException) { }
    }
}