using Passingwind.UserDialogs;
using System;
using System.Threading.Tasks;
using Tally.App.Controls;
using Tally.App.Helpers;
using Tally.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        readonly Color selectBackGroundColor = Color.FromHex("#3388df");
        private SSViewModel sSView = null;
        private Setting settingPage = null;
        public MainPage()
        {
            InitializeComponent();
            //首先初始化Tab
            Initplace();
            //再进行数据的调用，避免白屏
            Initalize();
        }

        /// <summary>
        /// 初始占位Tab
        /// <para>避免因数据加载导致的白屏</para>
        /// </summary>
        private void Initplace()
        {
            //添加tab
            gridFrames.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            gridFrames.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            gridFrames.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            AllInitalize();
            SetGridLength(0);
            SetFrameColor(frameHome, lbHome, lbIconHome);
        }

        /// <summary>
        /// 90ms之后进行数据加载
        /// <para>避免因数据加载导致的白屏</para>
        /// </summary>
        private void Initalize()
        {
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 90), () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    BindingContext = sSView = new SSViewModel(Navigation);
                    GlobalConfigExtensions.RestoreHomeFrame = Restore;
                    settingPage = new Setting(sSView);
                    await SetLayoutFrame();
                });
                return false;
            });
        }

        /// <summary>
        /// Frame点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (sender is Frame frmHome && frmHome.StyleId.Equals("frameHome"))
                {
                    AllInitalize();
                    SetGridLength(0);
                    SetDisplayPage(0);
                    SetFrameColor(frmHome, lbHome, lbIconHome);
                    //重新计算首页数据
                    sSView.Initalize();
                }
                else if (sender is Frame frmPlus && frmPlus.StyleId.Equals("framePlus"))
                {
                    //AllInitalize();
                    //SetGridLength(1);
                    //SetFrameColor(frmPlus, lbPlus, lbIconPlus);
                    //最后跳转page
                    SetDisplayPage(1);
                }
                else if (sender is Frame frmFavorite && frmFavorite.StyleId.Equals("frameSetting"))
                {
                    AllInitalize();
                    SetGridLength(2);
                    SetDisplayPage(2);
                    SetFrameColor(frmFavorite, lbSetting, lbIconSetting);
                }
            });
        }

        /// <summary>
        /// 初始化添加展示模块
        /// </summary>
        private async ValueTask<bool> SetLayoutFrame()
        {
            SetDisplayPage();
            //申请权限
            await EssentialsExtensions.ApplyPermission();
            Display();
            return true;
        }

        /// <summary>
        /// 设置GridLength的Star状态
        /// <para>0 1 2</para>
        /// </summary>
        /// <param name="column"></param>
        private void SetGridLength(int column)
        {
            gridFrames.ColumnDefinitions[column].Width = GridLength.Star;
        }

        /// <summary>
        /// 设置选中的Frame边框及颜色，Label标签的显示
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="txtLabel"></param>
        /// <param name="iconLabel"></param>
        private void SetFrameColor(Frame frame, Label txtLabel, Label iconLabel)
        {
            frame.Padding = new Thickness(22, 5);
            frame.BackgroundColor = selectBackGroundColor;
            txtLabel.IsVisible = true;
            txtLabel.TextColor = Color.White;
            iconLabel.TextColor = Color.White;
        }

        private void SetDisplayPage(int page = 0)
        {
            switch (page)
            {
                case 0:
                    ContentViewPage.Content = new HomePage();
                    break;
                case 1:
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushAsync(new AddRecord());
                    });
                    break;
                case 2:
                    ContentViewPage.Content = settingPage;
                    break;
                default:
                    ContentViewPage.Content = new HomePage();
                    break;
            }
        }

        /// <summary>
        /// 重置所有Tab
        /// </summary>
        private void AllInitalize()
        {
            gridFrames.ColumnDefinitions[0].Width = GridLength.Auto;
            gridFrames.ColumnDefinitions[1].Width = GridLength.Auto;
            gridFrames.ColumnDefinitions[2].Width = GridLength.Auto;

            //frameHome
            frameHome.Padding = new Thickness(0);
            frameHome.BackgroundColor = Color.Transparent;
            lbHome.IsVisible = false;
            lbHome.TextColor = Color.White;
            lbIconHome.TextColor = selectBackGroundColor;

            //framePlus
            framePlus.Padding = new Thickness(0);
            framePlus.BackgroundColor = Color.Transparent;
            lbPlus.IsVisible = false;
            lbPlus.TextColor = Color.White;
            lbIconPlus.TextColor = selectBackGroundColor;

            //frameSetting
            frameSetting.Padding = new Thickness(0);
            frameSetting.BackgroundColor = Color.Transparent;
            lbSetting.IsVisible = false;
            lbSetting.TextColor = Color.White;
            lbIconSetting.TextColor = selectBackGroundColor;
        }

        //
        private void Display()
        {
            if (GlobalConfigExtensions.IsFirstStart)
                UserDialogs.Instance.Alert(new AlertConfig("为切实保护你的个人信息，未经你同意，我们不会从第三方获取、共享或对外提供你的信息。你可前往设置>关于>阅读《隐私政策》并了解详细信息")
                                        .AddOkButton(text: "确认", () =>
                                         {
                                             // ok handle
                                         })
                                        .SetTitle("隐私政策")
                                        );
        }

        /// <summary>
        /// 恢复首页
        /// 委托调用
        /// </summary>
        private void Restore()
        {
            //重新计算
            sSView.Initalize();
            ////仅当是在添加账单按钮Tab生效
            //if (lbPlus.IsVisible)
            //{
            //    //所有tab恢复默认值
            //    AllInitalize();
            //    //重置gridlenght
            //    SetGridLength(0);
            //    //重置frame
            //    SetFrameColor(frameHome, lbHome, lbIconHome);
            //}
        }
    }
}