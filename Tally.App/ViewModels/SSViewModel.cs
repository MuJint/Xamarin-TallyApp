using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.App.ViewModel;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Xamarin.Forms;

namespace Tally.App.ViewModels
{
    public class SSViewModel : NotifyPropertyChanged
    {
        private readonly string InComeColor = $"#24C389";
        private readonly string SpendColor = $"#3A3A62";
        private readonly string BlueColor = $"#3388df";
        private readonly string WhiteColor = $"#FFFFFF";
        private readonly string BlackColor = $"#181819";

        private readonly ISpendLogServices _instance = Dependcy.Provider.GetService<ISpendLogServices>();
        public SSViewModel(INavigation navigation)
        {
            Navigation = navigation;
            EventTypes = new ObservableCollection<EventType>();
            ExpenseCards = new ObservableCollection<ExpenseCard>();
            Dates = new ObservableCollection<DateItem>();
            SelectDateCommand = new Command<DateItem>((model) => ExecuteSelectDateCommand(model));
            SelectEventTypeCommand = new Command<EventType>((model) => ExecuteSelectEventTypeCommand(model));
            InitPageStatisctical();
            LoadEventTypes();
            LoadSevenDaySpend();
            LoadDates();
            LoadOnDaySpend(DateTime.Now);
        }

        #region Property
        /// <summary>
        /// 计算卡片的总高
        /// </summary>
        public double ExpenseCardCalculateHeight { get; set; }
        /// <summary>
        /// 首页统计
        /// </summary>
        public PageStatisctical PageStatisctical { get; set; }
        public Command SelectDateCommand { get; }
        public Command SelectEventTypeCommand { get; }
        public ObservableCollection<EventType> EventTypes { get; }
        /// <summary>
        /// 近七天使用
        /// </summary>
        public ObservableCollection<ExpenseCard> ExpenseCards { get; }
        private ExpenseCard _onDayCard;
        /// <summary>
        /// 当日消费
        /// </summary>
        public ExpenseCard OnDayCard
        {
            get => _onDayCard;
            set => SetProperty(ref _onDayCard, value);
        }
        public ObservableCollection<DateItem> Dates { get; }

        private DateItem _selectedDate;
        /// <summary>
        /// 选中的日期
        /// <para>selectedDate</para>
        /// </summary>
        public DateItem SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private EventType _selectedEventType;
        public EventType SelectedEventType
        {
            get => _selectedEventType;
            set => SetProperty(ref _selectedEventType, value);
        }

        #endregion


        #region Func

        /// <summary>
        /// 初始化首页顶部统计
        /// </summary>
        private void InitPageStatisctical()
        {
            var start = DateTime.Now.GetStartOfMonth();
            var end = DateTime.Now.GetEndOfMonth();
            var queryData = _instance.Query(w => w.DateTime <= end && w.DateTime >= start);
            PageStatisctical = new PageStatisctical()
            {
                InCome = queryData?.Where(w => w.IsSpend == EnumSpend.Income).Sum(s => s.Rmb),
                Spend = queryData?.Where(w => w.IsSpend == EnumSpend.Spend).Sum(s => s.Rmb),
            };
        }
        #endregion

        private void LoadEventTypes()
        {
            EventTypes.Add(new EventType()
            {
                name = "Concert",
                image = "mic.png",
                selected = true,
                backgroundColor = "#FCCD00",
                textColor = "#000000"
            });

            EventTypes.Add(new EventType()
            {
                name = "Sports",
                image = "ping_pong.png",
                backgroundColor = "#29404E",
                textColor = "#FFFFFF"
            });

            EventTypes.Add(new EventType()
            {
                name = "Education",
                image = "graduation.png",
                backgroundColor = "#29404E",
                textColor = "#FFFFFF"
            });
        }

        /// <summary>
        /// 加载近七日的明细
        /// </summary>
        private void LoadSevenDaySpend()
        {
            var startNow = DateTime.Now.AddDays(-8);
            var endNow = DateTime.Now.AddDays(-1);
            var start = new DateTime(startNow.Year, startNow.Month, startNow.Day);
            var end = new DateTime(endNow.Year, endNow.Month, endNow.Day, 23, 59, 59);
            var data = _instance.Query(w => w.DateTime >= start && w.DateTime <= end);
            if (data == null || data.Count <= 0)
            {
                for (int i = 1; i <= 15; i++)
                {
                    var model = new Framework.Models.SpendLog()
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.Now.AddDays(1 - i),
                        Descrpition = new string[] { "早餐", "晚餐", "购物", "日常", "游戏" }[new Random().Next(0, 4)],
                        Icon = (EnumIcon)new Random().Next(1, 11),
                        IsSpend = EnumSpend.Spend,
                        Pay = EnumPay.Alipay,
                        Rmb = 1.1 * i
                    };
                    _instance.Insert(model);
                }
            }
            var groupResult = data.GroupBy(g => g.DateTime);
            foreach (var item in groupResult)
            {
                ExpenseCards.Add(new ExpenseCard()
                {
                    Date = item?.Key.DateToMonthAndDay(),
                    WeekOnDay = item?.Key.DateToWeekOnDay(),
                    InCome = item?.Where(w => w.IsSpend == EnumSpend.Income)?.Sum(s => s.Rmb),
                    Spend = item?.Where(w => w.IsSpend == EnumSpend.Spend)?.Sum(s => s.Rmb),
                    IsDisplay = item?.Count(w => w.Id != null) <= 0,
                    ExpenseRecords = item?.Select(s => new ExpenseRecord()
                    {
                        Icon = s.Icon.ToString(),
                        IconTitle = s.Icon.GetDescription(),
                        Description = s.Descrpition,
                        Rmb = s.Rmb,
                        TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor
                    }).ToList()
                });
            }

            //去掉ExpenseRecords集合的第一个标题的高度，如果它的集合大于1
            if (ExpenseCards.First().ExpenseRecords.Count() > 1)
                ExpenseCards.First().CalculateHeight -= 30;
            //计算的卡片的高度+卡片padding
            ExpenseCardCalculateHeight = ExpenseCards.Sum(s => s.CalculateHeight) + (ExpenseCards.Count() * 20);
        }

        private void LoadDates()
        {
            var dateInit = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dateEnd = new DateTime(dateInit.Year, dateInit.Month, DateTime.DaysInMonth(dateInit.Year, dateInit.Month));

            for (int i = 1; i <= dateEnd.Day; i++)
            {
                Dates.Add(new DateItem()
                {
                    Day = string.Format("{0:00}", i),
                    Month = dateInit.ToString("MMM").FirstLetterUpperCase(),
                    DayWeek = new DateTime(dateInit.Year, dateInit.Month, i).DateToWeekOnDay(),
                    Selected = i == DateTime.Today.Day,
                    BackgroundColor = i == DateTime.Today.Day ? BlueColor : WhiteColor,
                    TextColor = i == DateTime.Today.Day ? WhiteColor : BlackColor,
                });
            }
        }

        private void ExecuteSelectDateCommand(DateItem model)
        {
            Dates.ToList().ForEach((item) =>
            {
                item.Selected = false;
                item.BackgroundColor = WhiteColor;
                item.TextColor = BlackColor;
            });

            int index = Dates.ToList().FindIndex(p => p.Day == model.Day && p.DayWeek == model.DayWeek);
            if (index > -1)
            {
                Dates[index].BackgroundColor = BlueColor;
                Dates[index].TextColor = WhiteColor;
                Dates[index].Selected = true;

                LoadOnDaySpend(new DateTime(DateTime.Now.Year, DateTime.Now.Month, model.Day.ObjToInt()));
            }
        }

        private void ExecuteSelectEventTypeCommand(EventType model)
        {
            EventTypes.ToList().ForEach((item) =>
            {
                item.selected = false;
                item.backgroundColor = "#29404E";
                item.textColor = "#FFFFFF";
            });

            var index = EventTypes.ToList().FindIndex(p => p.name == model.name);
            if (index > -1)
            {
                EventTypes[index].backgroundColor = "#FCCD00";
                EventTypes[index].textColor = "#000000";
                EventTypes[index].selected = true;
            }
        }

        /// <summary>
        /// 加载当日数据
        /// </summary>
        /// <param name="dateTime">日期</param>
        private void LoadOnDaySpend(DateTime dateTime)
        {
            var start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            var end = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
            var result = _instance.Query(w => w.DateTime >= start && w.DateTime <= end);
            OnDayCard = new ExpenseCard()
            {
                Date = new DateTime(DateTime.Now.Year, dateTime.Month, dateTime.Day).DateToMonthAndDay(),
                InCome = result?.Where(s => s.IsSpend == EnumSpend.Income)?.Sum(s => s.Rmb),
                Spend = result?.Where(s => s.IsSpend == EnumSpend.Spend)?.Sum(s => s.Rmb),
                WeekOnDay = dateTime.DateToWeekOnDay(),
                IsDisplay = result?.Count() <= 0,
                ExpenseRecords = result?.Select(s => new ExpenseRecord()
                {
                    Icon = s.Icon.ToString(),
                    IconTitle = s.Icon.GetDescription(),
                    Description = s.Descrpition,
                    Rmb = s.Rmb,
                    TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor
                }).ToList()
            };
            //去掉ExpenseRecords集合的第一个标题的高度，如果它的集合大于1
            if (OnDayCard.ExpenseRecords.Count() > 1)
                OnDayCard.CalculateHeight -= 30;
        }
    }
}
