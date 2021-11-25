using System;
using System.Collections.Generic;
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
                    if(result.FileName.EndsWith("xlsx",StringComparison.OrdinalIgnoreCase))
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
