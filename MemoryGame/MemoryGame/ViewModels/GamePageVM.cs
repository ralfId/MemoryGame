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
using System.Timers;
using Xamarin.Forms;

namespace MemoryGame.ViewModels
{
    public class GamePageVM : BaseVM
    {
        private readonly IPokemonApiService _pokeApi;
        private readonly INavigation _navigation;


        private int MaxNum;
        private int MaxNumOfCards;
        

        public List<Pokemon> selectedPokemonLst; //to store selected items
        List<Pokemon> CardLstPokemon = new List<Pokemon>(); //to store items from api
        Stopwatch stopWatch = new Stopwatch();
        private Timer timer = new Timer();

        public GamePageVM(INavigation navigation, int level) : base(navigation)
        {
            _navigation = navigation;
            _pokeApi = DependencyService.Get<IPokemonApiService>();

            Level = level;
            //PokeImageHeightRequest = (Level == 1) ? 120.00 : (Level == 2) ? 75.00 : 100.00;

            GoBackCommand = new Command(GoBackCommandExecute);

            Init();
        }


        public Command<Pokemon> SetSelectedPokemonCommand { get; set; }
        public Command GoBackCommand { get; set; }

        #region Bindable Properties

        private int _level;
        public int Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged(); }
        }

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


        private int _counterMoves;
        public int CounterMoves
        {
            get => _counterMoves;
            set { _counterMoves = value; OnPropertyChanged(); }
        }

        private string _gameTimer;
        public string GameTimer
        {
            get => _gameTimer;
            set { _gameTimer = value; OnPropertyChanged(); }
        }

        private int _counterCouples;
        public int CounterCouples
        {
            get => _counterCouples;
            set { _counterCouples = value; OnPropertyChanged(); }
        }

        private int _couples;
        public int Couples
        {
            get => _couples;
            set { _couples = value; OnPropertyChanged(); }
        }

        private bool _loading = true;
        public bool Loading
        {
            get => _loading;
            set { _loading = value; OnPropertyChanged(); }
        }

        private int _counterTimer;
        public int CounterTimer
        {
            get => _counterTimer;
            set { _counterTimer = value; OnPropertyChanged(); }
        }

        private double _pokeImageHeightRequest;
        public double PokeImageHeightRequest
        {
            get => _pokeImageHeightRequest;
            set { _pokeImageHeightRequest = value; OnPropertyChanged(); }
        }
        #endregion


        public async void Init()
        {
            selectedPokemonLst = new List<Pokemon>();

            SetLocals();
            await LoadPokemons();

            SetSelectedPokemonCommand = new Command<Pokemon>(SetSelectedPokemon);
        }

        private async Task LoadPokemons()
        {

            try
            {
                IsBlocked = true;

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

                SetTimerToStartGame();


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

            }
        }

        private void SetTimerToStartGame()
        {
            try
            {
                stopWatch.Start();
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    // do something every 1 second
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // interact with UI elements
                        CounterTimer++;
                    });

                    if (CounterTimer == 3)
                    {
                        stopWatch.Stop();
                        CounterMoves = 0;
                        StartTheGame(true);
                        Loading = false;
                        return false;
                    }
                    return true; // runs again, or false to stop
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void StartTheGame(bool StartStopGame)
        {
            try
            {
                stopWatch.Start();
                IsBlocked = false;
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    // do something every 60 seconds
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // interact with UI elements
                        GameTimer = stopWatch.Elapsed.ToString(@"hh\:mm\:ss");
                    });

                    if (StartStopGame)
                    {
                        return true;
                    }
                    else
                    {
                        stopWatch.Stop();
                        return false; // runs again, or false to stop
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void SetLocals()
        {
            SpanGrid = (Level == 1 || Level == 2) ? 4 : 6;

            MaxNum = (Level == 1) ? 100 : (Level == 2) ? 200 : (Level == 3) ? 300 : 100;

            MaxNumOfCards = (Level == 1) ? 8 : (Level == 2) ? 12 : (Level == 3) ? 15 : 8;
            Couples = (Level == 1) ? 8 : (Level == 2) ? 12 : (Level == 3) ? 15 : 8;

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
                CounterCouples++;
                CounterMoves++;

                if (CounterCouples == Couples)
                {
                    StartTheGame(false);
                }

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
                CounterMoves++;
                if (CounterCouples == Couples)
                {
                    StartTheGame(false);
                }
            }


        }

        private async void GoBackCommandExecute()
        {
            //TODO: Clean all properties in memory
            stopWatch.Stop();
            await _navigation.PopToRootAsync();
        }


    }
}
