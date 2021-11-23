using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Passingwind.UserDialogs;

namespace Tally.App.Droid
{
    //去掉MainLauncher = true
    //https://www.cnblogs.com/LuoCore/p/8044964.html
    [Activity(Label = "Tally.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            UserDialogs.Init(this);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            // Using the Android Support Library v4
            //FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(context);

            //// Using API level 23:
            //FingerprintManager fingerprintManager = context.GetSystemService(Context.FingerprintService) as FingerprintManager;
        }
    }
}