using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Tally.App.Droid
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash",
   ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        //app启动动画
        //https://www.cnblogs.com/LuoCore/p/8044964.html
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}