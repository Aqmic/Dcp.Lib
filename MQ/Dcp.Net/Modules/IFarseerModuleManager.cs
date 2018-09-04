using System;
using System.Collections.Generic;

namespace Dcp.Net.Modules
{
    /// <summary>
    ///     模块管理器接口
    /// </summary>
    public interface IDcpModuleManager
    {
        /// <summary>
        ///     启动模块
        /// </summary>
        DcpModuleInfo StartupModule { get; }

        /// <summary>
        ///     模块列表
        /// </summary>
        IList<DcpModuleInfo> Modules { get; }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="startupModule"></param>
        void Initialize(Type startupModule);

        /// <summary>
        ///     启动模块
        /// </summary>
        void StartModules();

        /// <summary>
        ///     关闭模块
        /// </summary>
        void ShutdownModules();
    }
}