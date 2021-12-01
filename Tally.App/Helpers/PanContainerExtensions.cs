using System.Diagnostics;
using Xamarin.Forms;

namespace Tally.App.Helpers
{
    /// <summary>
    /// PanContainerMethod
    /// </summary>
    public class PanContainerExtensions : ContentView
    {
        double translationX;
        double maringRight;
        public PanContainerExtensions()
        {
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += PanGesture_PanUpdated;
            GestureRecognizers.Add(panGesture);
        }


        //https://docs.microsoft.com/zh-cn/xamarin/xamarin-forms/app-fundamentals/gestures/pan
        void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            //获取手势开始后 X 方向的总体更改。TotalX
            Trace.TraceWarning($"获取手势开始后 Y 方向的总体更改。TotalY:{e.TotalY}");
            Trace.TraceError($"获取手势开始后 X 方向的总体更改。TotalX:{e.TotalX}");
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    TotalXChange(e.TotalX);
                    break;
                case GestureStatus.Running:
                    TotalXChange(e.TotalX);
                    break;
                case GestureStatus.Completed:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }
        }

        void TotalXChange(double x)
        {
            Trace.TraceInformation("已执行方法");
            if (x > 0 && x > 40)
            {
                //平移之后X轴方向右移
                //复原
                Content.TranslateTo(0, 0, 500, Easing.CubicInOut);
                Content.Margin = new Thickness(0, 0, -120, 0);
            }
            if (x < 0 && x < -40)
            {
                //平移之后X轴方向左移
                Content.TranslateTo(-80, 0, 500, Easing.SpringIn);
                Content.Margin = new Thickness(0, 0, -80, 0);
            }
        }
    }
}
