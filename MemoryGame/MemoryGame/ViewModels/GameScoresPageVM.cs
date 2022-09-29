using System;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class GameScoresPageVM : BaseVM
    {
        private readonly INavigation _navigation;

        public GameScoresPageVM(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
        }

        private string _name = "This will be my name";
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
    }
}

