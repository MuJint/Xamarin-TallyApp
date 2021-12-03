using System;
using System.Collections.Generic;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.App.ViewModels;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Tally.Framework.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryDetail : ContentPage
    {
        readonly ISpendLogServices _instance = DependencyService.Get<ISpendLogServices>();
        readonly string InComeColor = $"#24C389";
        readonly string SpendColor = $"#3A3A62";
        readonly HistoryViewModel viewModel = null;
        public HistoryDetail(List<SpendLog> spends)
        {
            InitializeComponent();
            BindingContext = viewModel = new HistoryViewModel();
            LoadList(spends);
        }

        #region Method

        /// <summary>
        /// 重写页面消失
        /// </summary>
        protected override void OnDisappearing()
        {
            DisappearPage();
            base.OnDisappearing();
        }

        /// <summary>
        /// 重写返回事件
        /// <para>单指用户相应返回按键，并不响应Page的自带返回</para>
        /// </summary>
        /// <returns></returns>
        //protected override bool OnBackButtonPressed()
        //{
        //    DisappearPage();
        //    return base.OnBackButtonPressed();
        //}

        private void DisappearPage()
        {
            var enumSpend = viewModel?.ExpenseRecordsDetails?.FirstOrDefault()?.IsSpend == "+" ? EnumSpend.Income : EnumSpend.Spend;
            GlobalConfigExtensions.RestoreHistory(enumSpend);
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        /// <param name="spends"></param>
        private void LoadList(List<SpendLog> spends)
        {
            var result = StructList(spends);
            viewModel?.ExpenseRecordsDetails?.Clear();
            foreach (var record in result)
            {
                viewModel?.ExpenseRecordsDetails?.Add(record);
            }
        }

        /// <summary>
        /// 构造列表数据
        /// </summary>
        /// <param name="spendLogs"></param>
        /// <returns></returns>
        private List<ExpenseRecord> StructList(List<SpendLog> spendLogs)
        {
            return spendLogs?.OrderByDescending(s => s.DateTime)?.Select(s => new ExpenseRecord()
            {
                Icon = s.Icon.ToString(),
                IconTitle = s.Icon.GetDescription(),
                Description = s.Descrpition,
                Rmb = s.Rmb.Value,
                IsSpend = s.IsSpend == EnumSpend.Income ? "+" : "-",
                TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor,
                Id = s.Id,
                Time = $"{(s.DateTime >= DateTime.Now.AddDays(-3) ? s.DateTime.FormatDate() : s.DateTime.ToString("yyyy-MM-dd HH:mm:ss"))}"
            })?.ToList();
        }
        #endregion

        #region Event Handler
        /// <summary>
        /// edit clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn.TabIndex > 0)
            {
                var selectSpend = _instance.Query(t => t.Id == btn.TabIndex).FirstOrDefault();
                if (selectSpend != null)
                {
                    //跳转修改
                    await Navigation.PushAsync(new AddRecord(selectSpend.Id));
                }
            }
        }

        /// <summary>
        /// del clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn.TabIndex > 0)
            {
                var selectSpend = _instance.Query(t => t.Id == btn.TabIndex).FirstOrDefault();
                if (selectSpend != null)
                {
                    //触发删除
                    _instance.Delete(btn.TabIndex);
                    var atIndex = viewModel.ExpenseRecordsDetails.ToList().FindIndex(t => t.Id == btn.TabIndex);
                    viewModel.ExpenseRecordsDetails.RemoveAt(atIndex);
                }
            }
        }
        #endregion
    }
}