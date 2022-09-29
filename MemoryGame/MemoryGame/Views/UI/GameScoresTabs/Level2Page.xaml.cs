using System;
using System.Collections.Generic;
using MemoryGame.ViewModels;
using Xamarin.Forms;

namespace MemoryGame.Views.UI.GameScoresTabs
{
    public partial class Level2Page : ContentPage
    {
        public Level2Page()
        {
            InitializeComponent();
            BindingContext = new GameScoresPageVM(Navigation);

        }
    }
}

