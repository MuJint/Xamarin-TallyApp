using System.Collections.Generic;

namespace Tally.App.Models
{
    /// <summary>
    /// 消费卡片
    /// </summary>
    public class ExpenseCard
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 周几
        /// </summary>
        public string WeekOnDay { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public double InCome { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public double Spend { get; set; }
        /// <summary>
        /// 卡片的消费记录
        /// </summary>
        public ICollection<ExpenseRecord> ExpenseRecords { get; set; } = new HashSet<ExpenseRecord>();
        /// <summary>
        /// 计算卡片的高度
        /// <para>一个ExpenseRecord算作55</para>
        /// </summary>
        public int CalculateHeight => 30 + (ExpenseRecords.Count * 55);
    }

    public class ExpenseRecord
    {
        public string Icon { get; set; }
        public string IconTitle { get; set; }
        public string Description { get; set; }
        public double Rmb { get; set; }
        public string TextColor { get; set; }
        public string IsSpend { get; set; } = "-";
    }
}
