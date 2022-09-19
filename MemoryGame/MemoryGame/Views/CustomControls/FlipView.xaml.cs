using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Models;
using Xamarin.Forms;

namespace MemoryGame.Views.CustomControls
{
    public partial class FlipView : Grid
    {
        public ICommand BackViewCommand { get; set; }

        public FlipView()
        {
            InitializeComponent();

            BindingContext = this;

            //BackViewCommand = new Command<Pokemon>(BackViewExecuteCommand);

            FrontView.IsVisible = true;
            BackView.IsVisible = false;
        }

        public View FrontContent
        {
            get => (View)GetValue(FrontContentProperty);
            set => SetValue(FrontContentProperty, value);
        }
        public static readonly BindableProperty FrontContentProperty =
            BindableProperty.Create(nameof(FrontContent),
            typeof(View),
            typeof(FlipView),
            new ContentView());


        public View BackContent
        {
            get => (View)GetValue(BackContentProperty);
            set => SetValue(BackContentProperty, value);
        }
        public static readonly BindableProperty BackContentProperty =
            BindableProperty.Create(nameof(BackContent),
            typeof(View),
            typeof(FlipView),
            new ContentView());



        //private bool FrontIsVisible
        //{
        //    get => (bool)GetValue(FrontIsVisibleProperty);
        //    set => SetValue(FrontIsVisibleProperty, value);
        //}

        //private static readonly BindableProperty FrontIsVisibleProperty =
        //BindableProperty.Create(nameof(FrontIsVisible),
        //                        typeof(bool),
        //                        typeof(FlipView),
        //                        defaultValue: true,
        //                        defaultBindingMode: BindingMode.TwoWay);


        //private bool BackIsVisible
        //{
        //    get => (bool)GetValue(BackIsVisibleProperty);
        //    set => SetValue(BackIsVisibleProperty, value);
        //}

        //private static readonly BindableProperty BackIsVisibleProperty =
        //BindableProperty.Create(nameof(BackIsVisible),
        //                        typeof(bool),
        //                        typeof(FlipView),
        //                        defaultValue: false,
        //                        defaultBindingMode: BindingMode.TwoWay);

        //public int CountViews
        //{
        //    get => (int)GetValue(CountViewsProperty);
        //    set => SetValue(CountViewsProperty, value);
        //}

        //public static readonly BindableProperty CountViewsProperty =
        //BindableProperty.Create(nameof(CountViews),
        //                        typeof(int),
        //                        typeof(FlipView),
        //                        defaultValue: 0,
        //                        defaultBindingMode: BindingMode.TwoWay);


        public bool FlipViewIsEnabled
        {
            get => (bool)GetValue(FlipViewIsEnabledProperty);
            set => SetValue(FlipViewIsEnabledProperty, value);
        }

        public static readonly BindableProperty FlipViewIsEnabledProperty =
        BindableProperty.Create(nameof(FlipViewIsEnabled),
                                typeof(bool),
                                typeof(FlipView),
                                defaultValue: true,
                                defaultBindingMode: BindingMode.TwoWay,
                                propertyChanged: OnPropChanged );

        public ICommand Command
        {
            get => (ICommand)GetValue(property: CommandProperty);
            set => SetValue(property: CommandProperty, value: value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command),
                                    typeof(ICommand),
                                    typeof(FlipView),
                                    null);



        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName.Equals(FrontContentProperty.PropertyName))
            {
                FrontView.Content = FrontContent;
            }
            if (propertyName.Equals(BackContentProperty.PropertyName))
            {
                BackView.Content = BackContent;
            }
            //if (propertyName.Equals(BackIsVisibleProperty.PropertyName))
            //{
            //    BackView.IsVisible = BackIsVisible;
            //}
            //if (propertyName.Equals(FrontIsVisibleProperty.PropertyName))
            //{
            //    FrontView.IsVisible = FrontIsVisible;
            //}
            if (propertyName.Equals(FlipViewIsEnabled))
            {
                FlipViewGrid.IsEnabled = FlipViewIsEnabled;
            }
            //if (propertyName.Equals(CountViewsProperty.PropertyName))
            //{
                
            //}
        }

        private async void FronView_Tapped(object sender, EventArgs e)
        {
            //FrontView.IsVisible = false;
            //BackView.IsVisible = true;

            try
            {
                await Flip(FrontView, BackView);
                Command?.Execute(BindingContext);
                //if (FlipViewIsEnabled)
                //{
                //    await Flip(BackView, FrontView);
                //}

            }
            catch (Exception ex)
            {

            }
            //if (BindingContext is Pokemon pokeItem)
            //{

            //}


        }

        private async void BackView_Tapped(object sender, EventArgs e)
        {
            //FrontView.IsVisible = true;
            //BackView.IsVisible = false;
            await Flip(BackView, FrontView);

            //if (BindingContext is Pokemon pokeItem)
            //{

            //}


        }


        private async Task Flip(VisualElement from, VisualElement to)
        {
            await from.RotateYTo(-90, 300, Easing.SpringIn);
            to.RotationY = 90;
            to.IsVisible = true;
            from.IsVisible = false;
            await to.RotateYTo(0, 300, Easing.SpringOut);

        }

        private void BackViewExecuteCommand(Pokemon obj)
        {
            if (obj is Pokemon pokeItem)
            {

            }
        }

        private static void OnPropChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var isFalse = (bool)newValue;
            if (isFalse)
            {

            }
        }
    }
}

