using Tally.Framework.Enums;

namespace Tally.Framework.Models
{
    /// <summary>
    /// 错误日志
    /// </summary>
    public class ErrorLog : BaseModel
    {
        /// <summary>
        /// 等级
        /// </summary>
        public EnumError Level { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        public string Logs { get; set; }
    }
}
