using MemoryGame.ApiService;
using MemoryGame.Helpers;
using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class GamePageVM : BaseVM
    {
        private readonly IPokemonApiService _pokeApi;
        private readonly INavigation _navigation;

        //public ObservableCollection<Pokemon> ObPokemon { get; set; }
        public List<Pokemon> ListPokemon;

        private int MaxNum;
        private int MaxNumOfCards;


        public GamePageVM(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;

            _pokeApi = DependencyService.Get<IPokemonApiService>();

            Init();
        }

        private ObservableCollection<Pokemon> _obPokemon ;

        public ObservableCollection<Pokemon> ObPokemon 
        {
            get => _obPokemon;
            set { _obPokemon = value; OnPropertyChanged(nameof(ObPokemon)); }
        }


        private string _url;

        public string Url
        {
            get => _url;
            set { _url = value; OnPropertyChanged(nameof(Url)); }
        }

        private string _image;

        public string Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(nameof(Image)); }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        //private Pokemon _pokemon;

        //public Pokemon pokemon
        //{
        //    get => _pokemon;
        //    set { _pokemon = value; OnPropertyChanged(nameof(pokemon)); }
        //}

        public async void Init()
        {

            await LoadPokemons();
        }

        private async Task LoadPokemons()
        {

            try
            {
                SetLocals();

                ListPokemon = new List<Pokemon>();

                var pokeList = await _pokeApi.GetPokemonList<PokemonList>(MaxNum);

                //add pokelist to local list
                ListPokemon.AddRange(pokeList.Pokemons);
                //shuffle complet list
                ListPokemon.Shuffle();
                //select max num of cards depens on level
                var cardlimit = ListPokemon.GetRange(0, MaxNumOfCards);
                //duplicate cardlimit
                var duplicatedLst = cardlimit.ToList();

                cardlimit.AddRange(duplicatedLst);

                ObPokemon = new ObservableCollection<Pokemon>(cardlimit.ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

            }
        }

        private void SetLocals()
        {
            MaxNum = (SelectedLevel == 1) ? 100 : (SelectedLevel == 2) ? 200 : (SelectedLevel == 3) ? 300 : 100;

            MaxNumOfCards = (SelectedLevel == 1) ? 8 : (SelectedLevel == 2) ? 12 : (SelectedLevel == 3) ? 15 : 8;
        }
    }
}
