using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class BaseVM : BindableObject
    {
        private readonly INavigation _navigation;

        public ICommand BackToHomeCommand { get; set; }

        public BaseVM(INavigation navigation)
        {
            _navigation = navigation;

            BackToHomeCommand = new Command(BackToHome);
        }

        private async void BackToHome()
        {
            await _navigation.PopToRootAsync();
        }
    }
}
