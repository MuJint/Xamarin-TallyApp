using System;
using System.Threading.Tasks;
using Tally.App.ViewModel;
using Tally.App.Views;
using Xamarin.Forms;

namespace Tally.App.ViewModels
{
    public class StartPageViewModel : NotifyPropertyChanged
    {
        public StartPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            NavigateToMainPageCommand = new Command(async () => await ExecuteNavigateToMainPageCommand());
        }

        public Command NavigateToMainPageCommand { get; }

        private async Task ExecuteNavigateToMainPageCommand()
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
