using Tally.Framework.Enums;

namespace Tally.Framework.Models
{
    /// <summary>
    /// 消费记录表
    /// </summary>
    public class SpendLog : BaseModel
    {
        /// <summary>
        /// 金额
        /// </summary>
        public double Rmb { get; set; }
        /// <summary>
        /// 支出还是收入
        /// <para>true收入</para>
        /// </summary>
        public EnumSpend IsSpend { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public EnumPay Pay { get; set; }
        /// <summary>
        /// icon
        /// </summary>
        public EnumIcon Icon { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Descrpition { get; set; }
    }
}
