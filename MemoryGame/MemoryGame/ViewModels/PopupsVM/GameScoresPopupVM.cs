using System;
using System.Windows.Input;
using MemoryGame.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MemoryGame.ViewModels.PopupsVM
{
    public class GameScoresPopupVM : BaseVM
    {
        private readonly INavigation _navigation;


        public ICommand ClosePopupCommand { get; set; }

        public GameScoresPopupVM(INavigation navigation, GameScores Scores) : base(navigation)
        {
            _navigation = navigation;
            ClosePopupCommand = new Command(ClosePopupExecute);
            GameScores = Scores;
        }

        private GameScores _gameScores;
        public GameScores GameScores
        {
            get => _gameScores;
            set { _gameScores = value; OnPropertyChanged(); }
        }

        private async void ClosePopupExecute()
        {
            await _navigation.PopPopupAsync();
        }
    }
}

