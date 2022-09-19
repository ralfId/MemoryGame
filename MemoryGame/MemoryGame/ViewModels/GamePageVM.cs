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
        private int Level;

        public List<Pokemon> selectedPokemonLst;

        public GamePageVM(INavigation navigation, int level) : base(navigation)
        {
            _navigation = navigation;
            Level = level;
            _pokeApi = DependencyService.Get<IPokemonApiService>();
            selectedPokemonLst = new List<Pokemon>();
            Init();
        }


        public Command<Pokemon> SetSelectedPokemonCommand { get; set; }

        private ObservableCollection<Pokemon> _obPokemon = new ObservableCollection<Pokemon>() ;

        public ObservableCollection<Pokemon> ObPokemon 
        {
            get => _obPokemon;
            set { _obPokemon = value; OnPropertyChanged(); }
        }

        private int _spanGrid;
        public int SpanGrid
        {
            get => _spanGrid;
            set { _spanGrid = value; OnPropertyChanged(); }
        }


        //private IList<Pokemon> _selectedItemsLst;
        //public IList<Pokemon> SelectedItemsLst
        //{
        //    get => _selectedItemsLst;
        //    set { _selectedItemsLst = value; OnPropertyChanged(); }
        //}

        public async void Init()
        {
            SetLocals();
            await LoadPokemons();
            SetSelectedPokemonCommand = new Command<Pokemon>(SetSelectedPokemon);
        }

        

        private async Task LoadPokemons()
        {

            try
            {
                
                ListPokemon = new List<Pokemon>();

                //get list of images
                //MaxNum obtendra un rango de pokemons dependiendo el nivel
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

                //shuffle list for UI
                cardlimit.Shuffle();

                ObPokemon = new ObservableCollection<Pokemon>(cardlimit.ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

            }
        }

        private void SetLocals()
        {
            SpanGrid = (Level == 1 | Level == 2) ? 4 : 5;

            MaxNum = (Level == 1) ? 100 : (Level == 2) ? 200 : (Level == 3) ? 300 : 100;

            MaxNumOfCards = (Level == 1) ? 8 : (Level == 2) ? 12 : (Level == 3) ? 15 : 8;
        }

        private void SetSelectedPokemon(Pokemon obj)
        {

            if (selectedPokemonLst.Count >= 2) return;

            selectedPokemonLst.Add(obj);

            if (selectedPokemonLst.Count == 2)
                CheckPairs();

        }

        private void CheckPairs()
        {
            if (selectedPokemonLst[0].Name == selectedPokemonLst[1].Name)
            {
                ObPokemon.Where(x => x.Name == selectedPokemonLst[0].Name).Select(x => x.IsEnabled = false);
                ObPokemon.Where(x => x.Name == selectedPokemonLst[1].Name).Select(x => x.IsEnabled = false);

                selectedPokemonLst.Clear();
            }
        }
    }
}
