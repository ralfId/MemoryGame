using System;
using System.Collections.Generic;
using MemoryGame.ViewModels;
using Xamarin.Forms;

namespace MemoryGame.Views.UI.GameScoresTabs
{
    public partial class Level1Page : ContentPage
    {
        public Level1Page()
        {
            InitializeComponent();
            BindingContext = new GameScoresPageVM(Navigation);
        }
    }
}

