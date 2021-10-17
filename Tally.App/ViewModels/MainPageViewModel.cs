using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Tally.App.Helpers;
using Tally.App.Models;
using Tally.App.ViewModel;
using Tally.Framework.Interface;
using Xamarin.Forms;

namespace Tally.App.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly string InComeColor = $"#24C389";
        private readonly string SpendColor = $"#3A3A62";
        public MainPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            EventTypes = new ObservableCollection<EventType>();
            ExpenseCards = new ObservableCollection<ExpenseCard>();
            Dates = new ObservableCollection<DateItem>();
            SelectDateCommand = new Command<DateItem>((model) => ExecuteSelectDateCommand(model));
            SelectEventTypeCommand = new Command<EventType>((model) => ExecuteSelectEventTypeCommand(model));
            loadEventTypes();
            LoadEventItems();
            loadDates();
        }

        public Command SelectDateCommand { get; }
        public Command SelectEventTypeCommand { get; }
        public ObservableCollection<EventType> EventTypes { get; }
        public ObservableCollection<ExpenseCard> ExpenseCards { get; }
        public ObservableCollection<DateItem> Dates { get; }

        private DateItem _selectedDate;
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

        private void loadEventTypes()
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

        private void LoadEventItems()
        {
            var Instance = Dependcy.Provider.GetService<ISpendLogServices>();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "billApp.db3");
                    using (SQLiteConnection conn = new SQLiteConnection(path))
                    {
                        conn.CreateTable<Framework.Models.SpendLog>();
                        var model = new Framework.Models.SpendLog()
                        {
                            Id = Guid.NewGuid(),
                            DateTime = DateTime.Now,
                            Descrpition = new string[] { "早餐", "晚餐", "购物", "日常", "游戏" }[new Random().Next(0, 4)],
                            Icon = Framework.Enums.EnumIcon.Bus,
                            IsSpend = Framework.Enums.EnumSpend.Spend,
                            Pay = Framework.Enums.EnumPay.Alipay,
                            Rmb = (i + 1) * 1.1
                        };
                        conn.Insert(model);
                    }
                }
                catch (Exception c)
                {

                }
                
            }

            var data = Instance.Query(w => w.Id != null);

            var result = data.Select(s => new ExpenseRecord()
            {
                Icon = s.Icon.ToString(),
                IconTitle = s.Icon.GetDescription(),
                Description = s.Descrpition,
                Rmb = s.Rmb,
                TextColor = s.IsSpend == Framework.Enums.EnumSpend.Spend ? SpendColor : InComeColor
            });

            ExpenseCards.Add(new ExpenseCard()
            {
                Date = data.FirstOrDefault().DateTime.DateToMonthAndDay(),
                WeekOnDay = data.FirstOrDefault().DateTime.DateToWeekOnDay(),
                InCome = data.Sum(s => s.Rmb),
                Spend = 36.5,
                ExpenseRecords = result.ToList()
            });

            ExpenseCards.Add(new ExpenseCard()
            {
                Date = DateTime.Now.DateToMonthAndDay(),
                WeekOnDay = DateTime.Now.DateToWeekOnDay(),
                InCome = 14.00,
                Spend = 36.5,
                ExpenseRecords = new List<ExpenseRecord>()
                {
                    new ExpenseRecord()
                    {
                        Icon="restaurant",
                        IconTitle="餐饮",
                        Description="早餐",
                        Rmb=7,
                        TextColor=SpendColor
                    },
                    new ExpenseRecord()
                    {
                        Icon="restaurant",
                        IconTitle="餐饮",
                        Description="午餐",
                        Rmb=7.5,
                        TextColor=SpendColor
                    },
                    new ExpenseRecord()
                    {
                        Icon="transfer",
                        IconTitle="餐饮",
                        Description="午餐",
                        Rmb=11,
                        IsSpend="+",
                        TextColor=InComeColor
                    },
                }
            });

            ExpenseCards.Add(new ExpenseCard()
            {
                Date = $"{DateTime.Now.AddDays(-1):MM月dd日}",
                WeekOnDay = DateTime.Now.AddDays(-1).DateToWeekOnDay(),
                InCome = 14.00,
                Spend = 36.5,
                ExpenseRecords = new List<ExpenseRecord>()
                {
                    new ExpenseRecord()
                    {
                        Icon="",
                        IconTitle="餐饮",
                        Description="早餐",
                        Rmb=7,
                        TextColor=InComeColor
                    },
                }
            });
        }

        private void loadDates()
        {
            var dateInit = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var dateEnd = new DateTime(dateInit.Year, dateInit.Month, DateTime.DaysInMonth(dateInit.Year, dateInit.Month));

            for (int i = 1; i <= dateEnd.Day; i++)
            {
                Dates.Add(new DateItem()
                {
                    day = string.Format("{0:00}", i),
                    month = dateInit.ToString("MMM").FirstLetterUpperCase(),
                    dayWeek = new DateTime(dateInit.Year, dateInit.Month, i).DayOfWeek.ToString().Substring(0, 3),
                    selected = i == DateTime.Today.Day,
                    backgroundColor = i == DateTime.Today.Day ? "#FCCD00" : "Transparent",
                    textColor = i == DateTime.Today.Day ? "#000000" : "#FFFFFF",
                });
            }
        }

        private void ExecuteSelectDateCommand(DateItem model)
        {
            Dates.ToList().ForEach((item) =>
            {
                item.selected = false;
                item.backgroundColor = "Transparent";
                item.textColor = "#FFFFFF";
            });

            var index = Dates.ToList().FindIndex(p => p.day == model.day && p.dayWeek == model.dayWeek);
            if (index > -1)
            {
                Dates[index].backgroundColor = "#FCCD00";
                Dates[index].textColor = "#000000";
                Dates[index].selected = true;
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
    }
}
