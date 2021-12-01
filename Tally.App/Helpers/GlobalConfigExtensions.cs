using System;

namespace Tally.App.Helpers
{
    public static class GlobalConfigExtensions
    {
        /// <summary>
        /// 是否初次启动
        /// </summary>
        public static bool IsFirstStart = false;

        /// <summary>
        /// 是否有权限
        /// </summary>
        public static bool IsPermission = false;

        /// <summary>
        /// 申请权限
        /// </summary>
        public static Action ApplyPermissions { get; set; }

        /// <summary>
        /// 打开第三方应用
        /// </summary>

        public static Action<string> OpenFile { get; set; }
    }
}
