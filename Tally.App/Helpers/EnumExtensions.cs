using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Tally.App.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取Description
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>枚举显示名称</returns>
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var m = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (m == null) return enumValue.ToString();
            var da = m.GetCustomAttribute<DescriptionAttribute>();
            return da == null ? enumValue.ToString() : da.Description;
        }
    }
}
