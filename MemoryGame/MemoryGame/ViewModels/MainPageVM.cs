using MemoryGame.Views;
using MemoryGame.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class MainPageVM : BaseVM
    {
        private readonly INavigation _navigation;

        public ICommand NavToGamePageCommand { get; }
        public ICommand NavToScoresPagesCommand { get; }

        public MainPageVM(INavigation navigation):base(navigation)
        {
            _navigation = navigation;

            NavToGamePageCommand = new Command<string>(NavToGamePage);
            NavToScoresPagesCommand = new Command(NavToScoresPages);
        }

        private async void NavToScoresPages()
        {
            await _navigation.PushAsync(new GameScoresPage());
        }

        private async void NavToGamePage(string i)
        {
            if (int.TryParse(i, out int selection))
            {
                if (selection < 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Select a game level", "OK");
                }

                await _navigation.PushAsync(new GamePage(selection), true); ;
            }
            

        }
    }
}
