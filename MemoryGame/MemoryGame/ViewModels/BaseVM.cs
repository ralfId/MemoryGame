using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class BaseVM : BindableObject
    {

        public BaseVM(INavigation navigation)
        {

        }

        private int _selectedLevel;
        public int SelectedLevel
        {
            get => _selectedLevel;
            set { _selectedLevel = value;  OnPropertyChanged(nameof(SelectedLevel)); }
        }

    }
}
