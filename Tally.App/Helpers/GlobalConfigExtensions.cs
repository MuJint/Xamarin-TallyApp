using System;
using System.Runtime.CompilerServices;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Tally.Framework.Models;
using Xamarin.Forms;

namespace Tally.App.Helpers
{
    public static class GlobalConfigExtensions
    {

        static IErrorLogServices _errorLogServices = DependencyService.Get<IErrorLogServices>();
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

        /// <summary>
        /// 重置history统计
        /// </summary>
        public static Action<EnumSpend> RestoreHistory { get; set; }

        /// <summary>
        /// 重置下方按钮状态
        /// </summary>
        public static Action RestoreHomeFrame { get; set; }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="enumError"></param>
        /// <param name="memberName">方法名</param>
        /// <param name="lineNumber">行号</param>
        /// <param name="filePath">文件</param>
        /// <returns></returns>
        public static bool WriteLog(string logs, EnumError enumError = EnumError.Info, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = "")
        {
            return _errorLogServices.Insert(new ErrorLog()
            {
                DateTime = DateTime.Now,
                Level = enumError,
                Method = memberName,
                LineNumber = lineNumber,
                FilePath = filePath,
                Logs = logs
            });
        }

        /// <summary>
        /// 拼接异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetLogBuilder(Exception ex, string msg = "")
        {
            return $"{msg}.msg:{ex.Message}.StackTrace:{ex.StackTrace}.";
        }
    }
}
