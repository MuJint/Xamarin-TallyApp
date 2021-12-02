using System.IO;
using Tally.App.Helpers;
using Tally.App.Interface;
using Tally.App.Views;
using Tally.Framework.Interface;
using Tally.Framework.Services;
using Xamarin.Forms;

namespace Tally.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Initalize();
            //UnitWork.Restore();
            StartUp();
            //Page 包裹进入NavigationPage，以使用page跳转
            MainPage = new NavigationPage(new MainPage());
            if (Device.RuntimePlatform == Device.iOS)
                DependencyService.Get<IStatusbarColor>().ChangeStatusbarColor();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartUp();
        }

        protected override void OnSleep()
        {
            //休眠释放
            //UnitWork.Dispose();
        }

        protected override void OnResume()
        {
            //休眠启动
            //UnitWork.Restart();
            //StartUp();
        }

        private void Initalize()
        {
            //判断是否是初次启动
            //没有读出配置文件就新增
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "config.txt");
            //File.Delete(backingFile);
            if (backingFile == null || !File.Exists(backingFile))
            {
                WriteConfig(backingFile);
            }
            else
            {
                var count = 0;
                using (var reader = new StreamReader(backingFile, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (int.TryParse(line, out var newcount))
                        {
                            count = newcount;
                        }
                    }
                }
                if (count == 0)
                {
                    File.Delete(backingFile);
                    WriteConfig(backingFile);
                }
            }
        }

        /// <summary>
        /// IOC注入
        /// </summary>
        private void StartUp()
        {
            DependencyService.Register<ISpendLogServices, SpendLogServices>();
            //DependencyService.Register<ILiteDbContext, LiteDbContext>();
        }

        /// <summary>
        /// 写入配置文件 以及其他配置初始化
        /// </summary>
        /// <param name="path"></param>
        private void WriteConfig(string path)
        {
            using (StreamWriter writer = File.CreateText(path))
            {
                GlobalConfigExtensions.IsFirstStart = true;
                writer.WriteLine($"1"); //表示已经不是初次启动了
                UnitWork.Initalize();
            }
        }
    }
}