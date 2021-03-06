﻿namespace Dcp.Net.Configuration.Startup
{
    /// <summary>
    /// 模块配置
    /// </summary>
    internal class ModuleConfigurations : IModuleConfigurations
    {
        /// <summary>
        /// 系统启动配置
        /// </summary>
        public IDcpStartupConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpConfiguration"></param>
        public ModuleConfigurations(IDcpStartupConfiguration abpConfiguration)
        {
            Configuration = abpConfiguration;
        }
    }
}