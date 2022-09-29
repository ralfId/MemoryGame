using System;
using System.Collections.Generic;
using MemoryGame.ViewModels;
using Xamarin.Forms;

namespace MemoryGame.Views.UI.GameScoresTabs
{
    public partial class Level3Page : ContentPage
    {
        public Level3Page()
        {
            InitializeComponent();
            BindingContext = new GameScoresPageVM(Navigation);
        }
    }
}

