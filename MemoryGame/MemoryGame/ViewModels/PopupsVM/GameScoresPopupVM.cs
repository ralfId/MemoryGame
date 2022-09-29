using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.DBService;
using MemoryGame.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MemoryGame.ViewModels.PopupsVM
{
    public class GameScoresPopupVM : BaseVM
    {
        private readonly INavigation _navigation;
        private readonly IDBServices _dBServices;

        public ICommand ClosePopupCommand { get; set; }

        public GameScoresPopupVM(INavigation navigation, GameScores Scores) : base(navigation)
        {
            _navigation = navigation;
            _dBServices = DependencyService.Get<IDBServices>();

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
            await _dBServices.AddScore(GameScores);
            await _navigation.PopPopupAsync();
        }

       
    }
}

