using Tally.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tally.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly Color selectBackGroundColor = Color.FromHex("#3388df");
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new SSViewModel(Navigation);
            SetLayoutFrame();
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
                }
                else if (sender is Frame frmPlus && frmPlus.StyleId.Equals("framePlus"))
                {
                    AllInitalize();
                    SetGridLength(1);
                    SetDisplayPage(1);
                    SetFrameColor(frmPlus, lbPlus, lbIconPlus);
                }
                else if (sender is Frame frmFavorite && frmFavorite.StyleId.Equals("frameSetting"))
                {
                    AllInitalize();
                    SetGridLength(2);
                    SetFrameColor(frmFavorite, lbSetting, lbIconSetting);

                }
            });
        }

        /// <summary>
        /// 初始化设置Tab 颜色
        /// </summary>
        private void SetLayoutFrame()
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

        private void SetDisplayPage(int page)
        {
            AllDisablePage();
            switch (page)
            {
                case 0: HomePage.IsVisible = true; break;
                case 1: SpendPage.IsVisible = true; break;
                default:
                    HomePage.IsVisible = true;
                    break;
            }
        }

        private void AllDisablePage()
        {
            HomePage.IsVisible = false;
            SpendPage.IsVisible = false;
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
    }
}