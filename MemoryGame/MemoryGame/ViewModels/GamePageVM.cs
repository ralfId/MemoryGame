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


        private int MaxNum;
        private int MaxNumOfCards;
        private int Level;
        public List<Pokemon> selectedPokemonLst; //to store selected items
        List<Pokemon> CardLstPokemon = new List<Pokemon>(); //to store items from api

        public GamePageVM(INavigation navigation, int level) : base(navigation)
        {
            _navigation = navigation;
            _pokeApi = DependencyService.Get<IPokemonApiService>();

            Level = level;

            selectedPokemonLst = new List<Pokemon>();

            Init();
        }


        public Command<Pokemon> SetSelectedPokemonCommand { get; set; }

        private ObservableCollection<Pokemon> _obPokemon;

        public ObservableCollection<Pokemon> ObPokemon
        {
            get => _obPokemon;
            set { _obPokemon = value; OnPropertyChanged(); }
        }

        //determinate how many columns for grid
        private int _spanGrid;
        public int SpanGrid
        {
            get => _spanGrid;
            set { _spanGrid = value; OnPropertyChanged(); }
        }

        //block the collectionview
        private bool _isBlocked = false;
        public bool IsBlocked
        {
            get => _isBlocked;
            set { _isBlocked = value; OnPropertyChanged(); }
        }


    


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

                var pokeList = await _pokeApi.GetPokemonList<PokemonList>(MaxNum);
                pokeList.Pokemons.Shuffle();

                var cardlimit = pokeList.Pokemons.GetRange(0, MaxNumOfCards).Select(x => new Pokemon
                {
                    PokeId = x.PokeId,
                    Name = x.Name,
                    Url = x.Url,
                    Image = Utils.Helpers.urlImage(x.Url),
                    IsEnabledItem = x.IsEnabledItem,
                    FlipItem = x.FlipItem

                }).ToList();

                var cardlimit2 = cardlimit.ConvertAll(x => new Pokemon
                {
                    PokeId = x.PokeId,
                    Name = x.Name,
                    Url = x.Url,
                    Image = Utils.Helpers.urlImage(x.Url),
                    IsEnabledItem = x.IsEnabledItem,
                    FlipItem = x.FlipItem

                }).ToList();


                CardLstPokemon = CardLstPokemon.Concat(cardlimit).Concat(cardlimit2).ToList();

                CardLstPokemon.Shuffle();
                int id = 1;
                CardLstPokemon.ForEach(x => x.PokeId = id++);

                //TODO: Delete this line, is just to test 
                CardLstPokemon.ForEach(x => Console.WriteLine($"{x.PokeId}-->{x.Name}"));

                ObPokemon = new ObservableCollection<Pokemon>(CardLstPokemon);

                await Task.Delay(3000);

                CardLstPokemon.ForEach(x => x.FlipItem = !x.FlipItem);
                ObPokemon = new ObservableCollection<Pokemon>(CardLstPokemon);
                
               


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
            if (IsBlocked) return;

            if (selectedPokemonLst.Any())
            {
                if (selectedPokemonLst.Contains(obj))
                {
                    selectedPokemonLst.Remove(obj);
                    return;
                }
            }

            selectedPokemonLst.Add(obj);

            if (selectedPokemonLst.Count == 2)
            {
                IsBlocked = true;
                CheckPairs();
                IsBlocked = false;
            }


        }

        private async void CheckPairs()
        {

            if (selectedPokemonLst[0].Name == selectedPokemonLst[1].Name)
            {

                var poke1 = selectedPokemonLst[0];
                poke1.IsEnabledItem = false;



                var poke2 = selectedPokemonLst[1];
                poke2.IsEnabledItem = false;


                ObPokemon[ObPokemon.IndexOf(selectedPokemonLst[0])] = poke1;
                ObPokemon[ObPokemon.IndexOf(selectedPokemonLst[1])] = poke2;

                selectedPokemonLst.Clear();

            }
            else
            {
                var poke1 = selectedPokemonLst[0];
                poke1.FlipItem = true;



                var poke2 = selectedPokemonLst[1];
                poke2.FlipItem = true;

                await Task.Delay(1000);

                ObPokemon[ObPokemon.IndexOf(selectedPokemonLst[0])] = poke1;
                ObPokemon[ObPokemon.IndexOf(selectedPokemonLst[1])] = poke2; 

                selectedPokemonLst.Clear();

            }


        }
    }
}
