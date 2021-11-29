using Passingwind.UserDialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tally.App.Helpers
{
    public static class EssentialsExtensions
    {
        public static async ValueTask SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        /// <summary>
        /// 选择Excel的文件
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<Stream> SelectExcel()
        {
            try
            {
                //https://docs.microsoft.com/zh-cn/xamarin/essentials/file-picker?context=xamarin%2Fandroid&tabs=android
                //自定义文件类型
                var customFileType =
                    new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "*.xlsx","*.xls" } },
                        //{ DevicePlatform.Android, new[] { "application/comics" } },
                    });
                var options = new PickOptions
                {
                    FileTypes = customFileType,
                    PickerTitle = "请选择Excel文件"
                };
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        return await result.OpenReadAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// 递归获取权限
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<bool> ApplyPermission()
        {
            var result = false;
            try
            {
                //Android 上，可以调用 ShouldShowRationale 来检测用户过去是否已拒绝该权限
                var isDenied = Permissions.ShouldShowRationale<Permissions.StorageRead>();
                if (isDenied)
                {
                    UserDialogs.Instance.Toast(new ToastConfig()
                    {
                        Message = "请前往设置中赋予软件读写文件权限",
                        Position = ToastPosition.Default,
                        Duration = new TimeSpan(0, 0, 0, 5),
                        TextColor = Color.Red
                    });
                    return result;
                }
                var readStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                var writeStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
                switch (readStatus)
                {
                    case PermissionStatus.Unknown:
                        DisplayNoPermission();
                        break;
                    case PermissionStatus.Denied:
                        DisplayNoPermission();
                        break;
                    case PermissionStatus.Disabled:
                        DisplayNoPermission();
                        break;
                    case PermissionStatus.Granted:
                        result = true;
                        GlobalConfig.IsPermission = true;
                        break;
                    case PermissionStatus.Restricted:
                        DisplayNoPermission();
                        break;
                    default:
                        break;
                }
                switch (writeStatus)
                {
                    case PermissionStatus.Unknown:
                        DisplayNoPermission();
                        break;
                    case PermissionStatus.Denied:
                        DisplayNoPermission();
                        break;
                    case PermissionStatus.Disabled:
                        DisplayNoPermission();
                        break;
                    case PermissionStatus.Granted:
                        result = true;
                        GlobalConfig.IsPermission = true;
                        break;
                    case PermissionStatus.Restricted:
                        DisplayNoPermission();
                        break;
                    default:
                        break;
                }
            }
            catch (PermissionException ex)
            {
                //没有权限声明
                //申请权限
                //记录日志
                await ApplyPermission();
            }
            catch (Exception c)
            {
                //记录日志
            }
            return result;
        }

        /// <summary>
        /// 权限申请展示
        /// </summary>
        private static void DisplayNoPermission()
        {
            UserDialogs.Instance.Alert(new AlertConfig()
                .SetMessage("为了正常记录账单数据以及导入导出账单，需要申请您的文件读写权限。请放心本软件为单机软件，不使用网络请求。您也可以前往关于->查看隐私政策或者查看源码")
                .SetTitle("权限申请")
                .AddOkButton("确认", () =>
                 {
                     UserDialogs.Instance.Toast("您暂时不能使用核心功能");
                 })
                .AddCancelButton("取消", () =>
                {
                    UserDialogs.Instance.Toast("您暂时不能使用核心功能");
                }));
        }

        public static VeresionInfo GetAppInfo()
        {
            return new VeresionInfo
            {
                Version = AppInfo.VersionString,
                Build = AppInfo.BuildString
            };
        }

        #region Property
        public class VeresionInfo
        {
            public string Version { get; set; }
            public string Build { get; set; }
        }
        #endregion
    }
}
