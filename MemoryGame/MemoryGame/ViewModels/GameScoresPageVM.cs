using System;
using System.Collections.ObjectModel;
using System.Linq;
using MemoryGame.DBService;
using MemoryGame.Models;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class GameScoresPageVM : BaseVM
    {
        private readonly INavigation _navigation;
        private readonly IDBServices _dBService;

        public GameScoresPageVM(INavigation navigation ) : base(navigation)
        {
            _navigation = navigation;
            _dBService = DependencyService.Get<IDBServices>();

            Init();
        }


        private ObservableCollection<GameScores> _ob_Level_1;
        public ObservableCollection<GameScores> Ob_Level_1
        {
            get=> _ob_Level_1;
            set { _ob_Level_1 = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GameScores> _ob_Level_2;
        public ObservableCollection<GameScores> Ob_Level_2
        {
            get => _ob_Level_2;
            set { _ob_Level_2 = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GameScores> _ob_Level_3;
        public ObservableCollection<GameScores> Ob_Level_3
        {
            get => _ob_Level_3;
            set { _ob_Level_3 = value; OnPropertyChanged(); }
        }

        private async void Init()
        {
            var listL1 = await _dBService.GetGameScoresByLevel("1");
            var listL2 = await _dBService.GetGameScoresByLevel("2");
            var listL3 = await _dBService.GetGameScoresByLevel("3");

            if (listL1 != null)
                Ob_Level_1 = new ObservableCollection<GameScores>(listL1.OrderByDescending(x => x.Id));

            if (listL2 != null)
                Ob_Level_2 = new ObservableCollection<GameScores>(listL2.OrderByDescending(x => x.Id));

            if (listL3 != null)
                Ob_Level_3 = new ObservableCollection<GameScores>(listL3.OrderByDescending(x => x.Id));
        }
    }
}

