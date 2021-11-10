﻿using System.Collections.Generic;
using Tally.App.ViewModel;

namespace Tally.App.Models
{
    /// <summary>
    /// 消费卡片
    /// </summary>
    public class ExpenseCard : NotifyPropertyChanged
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
        public decimal? InCome { get; set; } = 0;
        /// <summary>
        /// 支出
        /// </summary>
        public decimal? Spend { get; set; } = 0;
        /// <summary>
        /// 是否展示【暂无数据】
        /// <para>默认false</para>
        /// </summary>
        public bool? IsDisplay { get; set; } = false;
        /// <summary>
        /// 卡片的消费记录
        /// </summary>
        public ICollection<ExpenseRecord> ExpenseRecords { get; set; } = new HashSet<ExpenseRecord>();
        private int calculateHeight;
        /// <summary>
        /// 计算卡片的高度
        /// <para>一个ExpenseRecord算作55</para>
        /// <para><seealso cref="Controls.SevenDaySpend"/>需要去掉ExpenseRecords集合的第一个标题的高度</para>
        /// </summary>
        public int CalculateHeight
        {
            //即使没有数据也显示高度撑起布局
            get => ExpenseRecords.Count <= 0
                    ? calculateHeight <= 0 ? 30 + 55 : calculateHeight
                    : calculateHeight <= 0 ? 30 + (ExpenseRecords.Count * 55) : calculateHeight;
            set => calculateHeight = value;
        }
    }

    public class ExpenseRecord
    {
        public string Icon { get; set; }
        public string IconTitle { get; set; }
        public string Description { get; set; }
        public decimal Rmb { get; set; }
        public string TextColor { get; set; }
        public string IsSpend { get; set; } = "-";
    }
}
