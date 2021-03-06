﻿namespace Dcp.Net.Configuration.Startup
{
    /// <summary>
    /// 模块配置接口
    /// </summary>
    public interface IModuleConfigurations
    {
        /// <summary>
        /// 系统全局配置接口
        /// </summary>
        IDcpStartupConfiguration Configuration { get; }
    }
}