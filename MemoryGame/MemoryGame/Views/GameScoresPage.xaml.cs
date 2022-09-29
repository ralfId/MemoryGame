using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MemoryGame.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameScoresPage : TabbedPage
    {
        public GameScoresPage()
        {
            InitializeComponent();
            BindingContext = new GameScoresPageVM(Navigation);
        }
    }
}
