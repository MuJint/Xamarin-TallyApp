using System.ComponentModel;

namespace Tally.Framework.Enums
{
    public enum EnumError
    {
        [Description("错误")]
        Error = 1,
        [Description("警告")]
        Warning = 2,
        [Description("提示")]
        Info = 3,
    }
}
