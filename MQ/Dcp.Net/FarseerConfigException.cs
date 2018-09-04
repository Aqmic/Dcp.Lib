// ********************************************
// 作者：何达贤（steden） QQ：11042427
// 时间：2017-01-17 10:37
// ********************************************

using System;
using System.Runtime.Serialization;

namespace Dcp.Net
{
	/// <summary>
	/// 配置文件不正确
	/// </summary>
	public class DcpConfigException : DcpException
    {
		/// <summary>
		///     构造函数
		/// </summary>
		public DcpConfigException() { }
#if !CORE
        /// <summary>
        ///     构造函数
        /// </summary>
        public DcpConfigException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { }
#endif
		/// <summary>
		///     构造函数
		/// </summary>
		/// <param name="message">Exception message</param>
		public DcpConfigException(string message) : base(message) { }

		/// <summary>
		///     构造函数
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public DcpConfigException(string message, Exception innerException) : base(message, innerException) { }
	}
}