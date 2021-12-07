using Passingwind.UserDialogs;
using System;
using Tally.App.Helpers;
using Tally.App.ViewModels;
using Tally.Framework.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentView
    {
        readonly SSViewModel sSViewModel = null;
        public Setting(SSViewModel sSView = null)
        {
            InitializeComponent();
            sSViewModel = sSView;
        }

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        private void LoadRestore()
        {
            UserDialogs.Instance.Alert(new AlertConfig("确定要初始化吗，请确认已备份账单？操作不可逆。")
                                        .AddOkButton(text: "确认", () =>
                                        {
                                            // ok handle
                                            UnitWork.Restore();
                                            Application.Current.Quit();
                                        })
                                        .AddCancelButton(text: "取消", () =>
                                        {
                                            //cancel handle
                                        })
                                        .SetTitle("警告")
                                        );
        }

        private void Development()
        {
            UserDialogs.Instance.Toast("正在开发中");
        }
        #endregion

        #region Event
        /// <summary>
        /// frame 点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var frame = sender as Frame;
                //校验枚举
                if (Enum.IsDefined(typeof(FrameTab), frame.TabIndex) is false)
                    return;
                switch ((FrameTab)frame.TabIndex)
                {
                    case FrameTab.Sevenday:
                        await Navigation.PushAsync(new SevenDayAnalysis(sSViewModel), true);
                        break;
                    case FrameTab.Security:
                        Development();
                        break;
                    case FrameTab.History:
                        await Navigation.PushAsync(new History(), true);
                        break;
                    case FrameTab.FileImport:
                        var stream = await EssentialsExtensions.SelectExcel();
                        break;
                    case FrameTab.Bug:
                        Development();
                        break;
                    case FrameTab.Restore:
                        LoadRestore();
                        break;
                    case FrameTab.Info:
                        await Navigation.PushAsync(new About(), true);
                        break;
                    default:
                        break;
                }
            });
        }
        #endregion

        #region Identity
        /// <summary>
        /// framTab
        /// </summary>
        private enum FrameTab
        {
            /// <summary>
            /// 七天图表
            /// </summary>
            Sevenday = 1,
            /// <summary>
            /// 安全中心
            /// </summary>
            Security = 2,
            /// <summary>
            /// 账单记录
            /// </summary>
            History = 3,
            /// <summary>
            /// 导入Excel
            /// </summary>
            FileImport = 4,
            /// <summary>
            /// 错误日志
            /// </summary>
            Bug = 5,
            /// <summary>
            /// 初始化
            /// </summary>
            Restore = 6,
            /// <summary>
            /// 关于
            /// </summary>
            Info = 7
        }
        #endregion
    }
}