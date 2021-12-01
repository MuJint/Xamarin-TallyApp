using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Java.IO;
using Passingwind.UserDialogs;
using Tally.App.Helpers;
using Xamarin.Essentials;

namespace Tally.App.Droid
{
    //去掉MainLauncher = true
    //https://www.cnblogs.com/LuoCore/p/8044964.html
    [Activity(Label = "Tally.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int REQUEST_PERMISSIONS_CODE = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //委托
            GlobalConfigExtensions.ApplyPermissions = ApplyPermissions;
            GlobalConfigExtensions.OpenFile = OpenFileByIntent;

            UserDialogs.Init(this);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case REQUEST_PERMISSIONS_CODE:
                    {
                        //检查授权
                        if (grantResults[0] == Permission.Granted && grantResults[1] == Permission.Granted)
                        {
                            GlobalConfigExtensions.IsPermission = true;
                            Snackbar.Make(FindViewById(Android.Resource.Id.Content), "存储权限已开启现在可以正常使用", Snackbar.LengthShort).Show();
                        }
                        else
                            Snackbar.Make(FindViewById(Android.Resource.Id.Content), "拒绝了存储授权", Snackbar.LengthShort).Show();
                    }
                    break;
                default:
                    break;
            }
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void ApplyPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.ManageExternalStorage };
            if (CheckSelfPermission(Manifest.Permission.ReadExternalStorage) == Permission.Granted && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == Permission.Granted)
            {
                //
                return;
            }

            if (ShouldShowRequestPermissionRationale(Manifest.Permission.ReadExternalStorage) || ShouldShowRequestPermissionRationale(Manifest.Permission.WriteExternalStorage) || ShouldShowRequestPermissionRationale(Manifest.Permission.ManageExternalStorage))
            {
                Snackbar.Make(FindViewById(Android.Resource.Id.Content), "记录账单及导出需要申请您的存储权限", Snackbar.LengthIndefinite)
                        .SetAction("OK", v => RequestPermissions(permissions, REQUEST_PERMISSIONS_CODE))
                        .Show();
                return;
            }
            RequestPermissions(permissions, REQUEST_PERMISSIONS_CODE);
            await System.Threading.Tasks.Task.Delay(1);
        }


        public void OpenFileByIntent(string file)
        {
            try
            {
                Intent intent = new Intent();
                intent.AddFlags(ActivityFlags.NewTask);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                intent.AddFlags(ActivityFlags.GrantWriteUriPermission);
                intent.SetAction(Intent.ActionView);
                //在安卓9以上以下代码才进行工作，需要加入GrantReadUriPermission
                File apkFile = new File(file);
                Uri uri = AndroidX.Core.Content.FileProvider.GetUriForFile(Application.Context, $"{AppInfo.PackageName}.provider", apkFile);
                intent.SetDataAndType(uri, "application/vnd.ms-excel");
                Application.Context.StartActivity(intent);
                Intent.CreateChooser(intent, "请选择能打开Excel的软件");
            }
            catch (ActivityNotFoundException e)
            {

            }
            catch (Java.Lang.Exception c)
            {

            }
        }
    }
}