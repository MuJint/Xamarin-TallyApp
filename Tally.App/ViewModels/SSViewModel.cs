using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.App.ViewModel;
using Tally.Framework.Enums;
using Tally.Framework.Interface;
using Tally.Framework.Models;
using Xamarin.Forms;

namespace Tally.App.ViewModels
{
    public class SSViewModel : NotifyPropertyChanged
    {
        readonly string InComeColor = $"#24C389";
        readonly string SpendColor = $"#3A3A62";
        readonly string BlueColor = $"#3388df";
        readonly string WhiteColor = $"#FFFFFF";
        readonly string BlackColor = $"#181819";

        readonly ISpendLogServices _instance = DependencyService.Get<ISpendLogServices>();
        public SSViewModel(INavigation navigation)
        {
            Navigation = navigation;
            SelectDateCommand = new Command<DateItem>((model) => ExecuteSelectDateCommand(model));
            SelectEventTypeCommand = new Command<EventType>((model) => ExecuteSelectEventTypeCommand(model));
            Initalize();
        }

        #region Property
        /// <summary>
        /// 计算卡片的总高
        /// </summary>
        public double? ExpenseCardCalculateHeight { get; set; }
        private PageStatisctical _statisctical;
        /// <summary>
        /// 首页统计
        /// </summary>
        public PageStatisctical Statisctical
        {
            get => _statisctical;
            set => SetProperty(ref _statisctical, value);
        }
        /// <summary>
        /// 日期选中Command
        /// </summary>
        public Command SelectDateCommand { get; set; }
        public Command SelectEventTypeCommand { get; set; }
        public ObservableCollection<EventType> EventTypes { get; } = new ObservableCollection<EventType>();
        /// <summary>
        /// 近七天使用
        /// </summary>
        public ObservableCollection<ExpenseCard> ExpenseCards { get; } = new ObservableCollection<ExpenseCard>();
        /// <summary>
        /// 支出icon集合
        /// </summary>
        public ObservableCollection<SpendImg> SpendImgs { get; set; } = new ObservableCollection<SpendImg>();
        private CostInfo _costInfo = new CostInfo()
        {
            Cost = "0",
            Icon = "food",
            Title = "餐饮"
        };
        /// <summary>
        /// 选中的ICON图标数据
        /// </summary>
        public CostInfo CostInfo
        {
            get => _costInfo;
            set => SetProperty(ref _costInfo, value);
        }
        private ExpenseCard _onDayCard;
        /// <summary>
        /// 当日消费
        /// </summary>
        public ExpenseCard OnDayCard
        {
            get => _onDayCard;
            set => SetProperty(ref _onDayCard, value);
        }
        /// <summary>
        /// 日期集合
        /// </summary>
        public ObservableCollection<DateItem> Dates { get; } = new ObservableCollection<DateItem>();

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
        /// 初始化
        /// </summary>
        private void Initalize()
        {
            LoadStatisctical();
            LoadEventTypes();
            LoadSevenDaySpend();
            LoadDates();
            LoadOnDaySpend(DateTime.Now);
            LoadSpendImgs();
        }

        #endregion

        #region Method
        /// <summary>
        /// 加载首页顶部统计
        /// </summary>
        public void LoadStatisctical()
        {
            var start = DateTime.Now.GetStartOfMonth();
            var end = DateTime.Now.GetEndOfMonth();
            var queryData = _instance.Query(w => w.DateTime <= end && w.DateTime >= start);
            Statisctical = new PageStatisctical()
            {
                InCome = queryData?.Where(w => w.IsSpend == EnumSpend.Income).Sum(s => s.Rmb),
                Spend = queryData?.Where(w => w.IsSpend == EnumSpend.Spend).Sum(s => s.Rmb),
            };
        }
        /// <summary>
        /// 加载支出List
        /// </summary>
        /// <returns></returns>
        public void LoadSpendImgs()
        {
            SpendImgs?.Clear();
            SpendImgs.Add(new SpendImg()
            {
                Icon = "food",
                Title = "餐饮",
                Color = "#181819",
                Opacity = 1
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "shopping",
                Title = "购物"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "bus",
                Title = "交通"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "transfer",
                Title = "转账"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "thephone",
                Title = "话费"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "takeout",
                Title = "外卖"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "redgift",
                Title = "红包"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "study",
                Title = "学习"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "travel",
                Title = "旅游"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "rent",
                Title = "房租"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "game",
                Title = "游戏"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "hospital",
                Title = "医疗"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "ktv",
                Title = "ktv"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "pet",
                Title = "宠物"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "utility",
                Title = "生活缴费"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "wine",
                Title = "酒水"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "cookingu",
                Title = "厨具"
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "other",
                Title = "其它"
            });
        }

        /// <summary>
        /// 加载收入List
        /// </summary>
        public void LoadInComeImgs()
        {
            SpendImgs?.Clear();
            SpendImgs.Add(new SpendImg()
            {
                Icon = "wage",
                Title = "工资",
                Color = "#181819",
                Opacity = 1
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "transfer",
                Title = "转账",
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "bonus",
                Title = "奖金",
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "redgift",
                Title = "红包",
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "stock",
                Title = "股票",
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "invest",
                Title = "理财",
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "house",
                Title = "房产(房租)",
            });
            SpendImgs.Add(new SpendImg()
            {
                Icon = "othermoney",
                Title = "其它",
            });
        }

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
        /// 近七日数据
        /// </summary>
        /// <returns></returns>
        public List<SpendLog> LoadSevenDayList()
        {
            var startNow = DateTime.Now.AddDays(-7);
            var endNow = DateTime.Now.AddDays(-1);
            var start = new DateTime(startNow.Year, startNow.Month, startNow.Day);
            var end = new DateTime(endNow.Year, endNow.Month, endNow.Day, 23, 59, 59);
            return _instance.Query(w => w.DateTime >= start && w.DateTime <= end);
        }

        /// <summary>
        /// 加载近七日的明细
        /// </summary>
        private void LoadSevenDaySpend()
        {
            var data = LoadSevenDayList();
            var groupResult = data.OrderByDescending(s => s.DateTime).GroupBy(g => g.DateTime.Day);
            foreach (var item in groupResult)
            {
                ExpenseCards.Add(new ExpenseCard()
                {
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, item.Key).DateToMonthAndDay(),
                    WeekOnDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, item.Key).DateToWeekOnDay(),
                    InCome = item?.Where(w => w.IsSpend == EnumSpend.Income)?.Sum(s => s.Rmb),
                    Spend = item?.Where(w => w.IsSpend == EnumSpend.Spend)?.Sum(s => s.Rmb),
                    IsDisplay = item?.Count(w => w.Id != null) <= 0,
                    ExpenseRecords = item?.OrderByDescending(s => s.DateTime)?.Select(s => new ExpenseRecord()
                    {
                        Icon = s.Icon.ToString(),
                        IconTitle = s.Icon.GetDescription(),
                        Description = s.Descrpition,
                        Rmb = s.Rmb.Value,
                        IsSpend = s.IsSpend == EnumSpend.Income ? "+" : "-",
                        TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor
                    }).ToList()
                });
            }

            //去掉ExpenseRecords集合的第一个标题的高度，如果它的集合大于1
            //if (ExpenseCards?.FirstOrDefault()?.ExpenseRecords?.Count() > 1)
            //    ExpenseCards.FirstOrDefault().CalculateHeight -= 30;
            //计算的卡片的高度+卡片padding
            ExpenseCardCalculateHeight = ExpenseCards?.Sum(s => s.CalculateHeight) + (ExpenseCards?.Count() * 20);
        }

        /// <summary>
        /// 加载日期
        /// </summary>
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

        /// <summary>
        /// 日期选择Command
        /// </summary>
        /// <param name="model"></param>
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
        public void LoadOnDaySpend(DateTime dateTime)
        {
            var start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            var end = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
            var result = _instance.Query(w => w.DateTime >= start && w.DateTime <= end).OrderByDescending(o => o.DateTime);
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
                    Rmb = s.Rmb.Value,
                    IsSpend = s.IsSpend == EnumSpend.Income ? "+" : "-",
                    TextColor = s.IsSpend == EnumSpend.Spend ? SpendColor : InComeColor
                }).ToList()
            };
            //去掉ExpenseRecords集合的第一个标题的高度，如果它的集合大于1
            //if (OnDayCard.ExpenseRecords.Count() > 1)
            //    OnDayCard.CalculateHeight -= 30;
        }
        #endregion

        #region Delegate
        /// <summary>
        /// 委托
        /// 恢复为首页
        /// </summary>
        public Action Restore { get; set; }
        /// <summary>
        /// 委托
        /// 回到设置页面
        /// </summary>
        public Action ReturnSetting { get; set; }
        #endregion
    }
}
