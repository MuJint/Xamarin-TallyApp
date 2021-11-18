﻿using Passingwind.UserDialogs;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentView
    {
        public Setting()
        {
            InitializeComponent();
        }

        #region Method
        /// <summary>
        /// 初始化
        /// </summary>
        private void LoadRestore()
        {
            UserDialogs.Instance.Alert(new AlertConfig("确定要初始化吗？操作不可逆。")
                                        .AddOkButton(text: "确认", () =>
                                        {
                                            // ok handle
                                            Application.Current.Quit();
                                        })
                                        .AddCancelButton(text: "取消", () =>
                                        {
                                            //cancel handle
                                        })
                                        .SetTitle("警告")
                                        );
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
            Device.BeginInvokeOnMainThread(() =>
            {
                var frame = sender as Frame;
                //校验枚举
                if (Enum.IsDefined(typeof(FrameTab), frame.TabIndex) is false)
                    return;
                switch ((FrameTab)frame.TabIndex)
                {
                    case FrameTab.Sevenday:
                        break;
                    case FrameTab.Security:
                        break;
                    case FrameTab.History:
                        break;
                    case FrameTab.FileImport:
                        break;
                    case FrameTab.Bug:
                        break;
                    case FrameTab.Restore:
                        LoadRestore();
                        break;
                    case FrameTab.Info:
                        Application.Current.MainPage = new About();
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