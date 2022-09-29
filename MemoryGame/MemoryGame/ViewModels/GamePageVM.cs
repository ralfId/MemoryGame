using MemoryGame.ApiService;
using MemoryGame.Helpers;
using MemoryGame.Models;
using MemoryGame.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
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
        public int Couples { get; set; }

        public GamePageVM(INavigation navigation, int level) : base(navigation)
        {
            _navigation = navigation;
            _pokeApi = DependencyService.Get<IPokemonApiService>();

            Level = level;
            SetSelectedPokemonCommand = new Command<Pokemon>(SetSelectedPokemon);
            GoBackCommand = new Command(GoBackCommandExecute);

            Init();
        }

        //TODO: for each move at the end of the game subtract the total seconds of waiting

        public ICommand SetSelectedPokemonCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

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
        private bool _isBlocked = true;
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

        private string _gameTimer = "00:00:00";
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


        private bool _isVisibleCountDown;
        public bool IsVisibleCountDown
        {
            get => _isVisibleCountDown;
            set { _isVisibleCountDown = value; OnPropertyChanged(); }
        }

        private bool _isRunning = true;
        public bool IsRunning
        {
            get => _isRunning;
            set { _isRunning = value; OnPropertyChanged(); }
        }

        private bool _playCountDown;
        public bool PlayCountDown
        {
            get => _playCountDown;
            set { _playCountDown = value; OnPropertyChanged(); }
        }
        #endregion

        public async void Init()
        {
            selectedPokemonLst = new List<Pokemon>();

            SetLocals();
            await LoadPokemons();
            await DelayCountDown();
            StartTheGame(true);
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
                    IsEnabledItem = false,
                    FlipItem = x.FlipItem

                }).ToList();

                var cardlimit2 = cardlimit.ConvertAll(x => new Pokemon
                {
                    PokeId = x.PokeId,
                    Name = x.Name,
                    Url = x.Url,
                    Image = Utils.Helpers.urlImage(x.Url),
                    IsEnabledItem = false,
                    FlipItem = x.FlipItem

                }).ToList();


                CardLstPokemon = CardLstPokemon.Concat(cardlimit).Concat(cardlimit2).ToList();

                CardLstPokemon.Shuffle();
                int id = 1;
                CardLstPokemon.ForEach(x => x.PokeId = id++);

                ObPokemon = new ObservableCollection<Pokemon>(CardLstPokemon);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

            }
        }

        private async Task DelayCountDown()
        {
            IsVisibleCountDown = true;
            await Task.Delay(500);
            PlayCountDown = true;
            await Task.Delay(2500);
            IsRunning = false;
            IsVisibleCountDown = false;
            PlayCountDown = false;
        }

        private async void StartTheGame(bool StartStopGame)
        {
            try
            {


                if (StartStopGame)
                {
                    if (Level == 1)
                        await Task.Delay(5000);
                    if (Level == 2)
                        await Task.Delay(9000);
                    if (Level == 3)
                        await Task.Delay(12000);

                    CardLstPokemon.ForEach(x => { x.FlipItem = true; x.IsEnabledItem = true; });
                    ObPokemon = new ObservableCollection<Pokemon>(CardLstPokemon);

                    stopWatch.Start();
                    IsBlocked = false;
                }
               if(!StartStopGame)
                    IsBlocked = true;
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
                    await Task.Delay(1000);
                    SaveScores();
                }

            }
            else
            {
                var poke1 = new Pokemon
                {
                    PokeId = selectedPokemonLst[0].PokeId,
                    Name = selectedPokemonLst[0].Name,
                    Url = selectedPokemonLst[0].Url,
                    Image = selectedPokemonLst[0].Image,
                    IsEnabledItem = true,
                    FlipItem = true
                };

                var poke2 = new Pokemon
                {
                    PokeId = selectedPokemonLst[1].PokeId,
                    Name = selectedPokemonLst[1].Name,
                    Url = selectedPokemonLst[1].Url,
                    Image = selectedPokemonLst[1].Image,
                    IsEnabledItem = true,
                    FlipItem = true
                };

                var newIndexPoke1 = ObPokemon.IndexOf(selectedPokemonLst[0]);
                var newIndexPoke2 = ObPokemon.IndexOf(selectedPokemonLst[1]);




                await Task.Delay(500);


                ObPokemon.Remove(selectedPokemonLst[0]);
                ObPokemon.Add(poke1);
                var oldIndexPoke1 = ObPokemon.IndexOf(poke1);
                ObPokemon.Move(oldIndexPoke1, newIndexPoke1);

                ObPokemon.Remove(selectedPokemonLst[1]);
                ObPokemon.Add(poke2);
                var oldIndexPoke2 = ObPokemon.IndexOf(poke2);
                ObPokemon.Move(oldIndexPoke2, newIndexPoke2);

                selectedPokemonLst.Clear();
                CounterMoves++;
            }
        }

        private async void SaveScores()
        {
            var myScores = new Models.GameScores()
            {
                Level = Level.ToString(),
                GameDate = DateTime.Now,
                GameTime = TimeSpan.Parse(GameTimer),
                Moves = CounterMoves.ToString()
            };

            await _navigation.PushPopupAsync(new GameScoresPopup(myScores));
            GoBackCommandExecute();
        }

        private async void GoBackCommandExecute()
        {
            stopWatch.Stop();
            stopWatch.Reset();
            await _navigation.PopToRootAsync();
        }
    }
}
