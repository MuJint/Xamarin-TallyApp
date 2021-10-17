using System.ComponentModel;

namespace Tally.Framework.Enums
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum EnumPay
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 1,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WechatPay =2,
        /// <summary>
        /// 现金
        /// </summary>
        [Description("现金")]
        Cash = 3
    }
}
