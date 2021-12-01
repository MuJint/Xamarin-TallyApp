using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.Framework.Enums;
using Tally.Framework.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryDetail : ContentPage
    {
        readonly string InComeColor = $"#24C389";
        readonly string SpendColor = $"#3A3A62";
        public HistoryDetail(List<SpendLog> spends)
        {
            InitializeComponent();
            detailSource.ItemsSource = spends.OrderByDescending(s => s.DateTime).Select(s => new ExpenseRecord()
            {
                Icon = s.Icon.ToString(),
                IconTitle = s.Icon.GetDescription(),
                Description = s.Descrpition,
                Rmb = s.Rmb.Value,
                IsSpend = s.IsSpend == EnumSpend.Income ? "+" : "-",
                TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor,
                Id = s.Id,
                Time = $"{(s.DateTime >= DateTime.Now.AddDays(-3) ? s.DateTime.FormatDate() : s.DateTime.ToString("yyyy-MM-dd HH:mm:ss"))}"
            });
        }
    }
}