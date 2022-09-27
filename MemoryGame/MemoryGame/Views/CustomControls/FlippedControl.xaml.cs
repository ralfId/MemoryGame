using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MemoryGame.Views.CustomControls
{
    public partial class FlippedControl : ContentView
    {
        public FlippedControl()
        {
            InitializeComponent();
            FrontLayout.IsVisible = false;
            BackLayout.IsVisible = true;
        }


        /// <summary>
        /// This property will store the front content
        /// </summary>
        public static readonly BindableProperty FrontContentProperty = BindableProperty.Create(nameof(FrontContent), typeof(View), typeof(FlippedControl), new ContentView());
        public View FrontContent
        {
            get => (View)GetValue(FrontContentProperty);
            set => SetValue(FrontContentProperty, value);
        }

        /// <summary>
        /// This property will store the back content
        /// </summary>
        public static readonly BindableProperty BackContentProperty = BindableProperty.Create(nameof(BackContent), typeof(View), typeof(FlippedControl), new ContentView());
        public View BackContent
        {
            get => (View)GetValue(BackContentProperty);
            set => SetValue(BackContentProperty, value);
        }

        /// <summary>
        /// OPTIONAL PROPERTY
        /// This property is to notify when control is flipped
        /// </summary>
        //public static readonly BindableProperty IsFlippedProperty = BindableProperty.Create(nameof(IsFlipped), typeof(bool), typeof(FlippedControl), defaultValue: false, propertyChanged: IsFlippedPropertyChanged);
        //public bool IsFlipped
        //{
        //    get => (bool)GetValue(IsFlippedProperty);
        //    set => SetValue(IsFlippedProperty, value);
        //}

        /// <summary>
        /// Command property to execute when is necesary
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand),  typeof(FlippedControl), null);
        public ICommand Command
        {
            get => (ICommand)GetValue(property: CommandProperty);
            set => SetValue(property: CommandProperty, value: value);
        }

        /// <summary>
        /// This property can enable or disabled the containers: The Main Container Layout, Front Container Layout and Back Container Layout
        /// when this property fire to FALSE all controls will be disabled and Front Layout and Back Layout visible properties will be as user default
        /// </summary>
        public static readonly BindableProperty IsEnabledControlProperty = BindableProperty.Create(nameof(IsEnabledControl), typeof(bool), typeof(FlippedControl), defaultValue: true, propertyChanged: IsEnabledControlPropertyChanged);
        public bool IsEnabledControl
        {
            get => (bool)GetValue(IsEnabledControlProperty);
            set => SetValue(IsEnabledControlProperty, value);
        }

        /// <summary>
        /// This property can flip the control to back or front content
        /// TRUE => will flip to front content
        /// FALSE => will flip to back content
        /// </summary>
        public static readonly BindableProperty FlipControlProperty = BindableProperty.Create(nameof(FlipControl), typeof(bool), typeof(FlippedControl), defaultValue: false, propertyChanged: FlipControlPropertyChanged);
        public bool FlipControl
        {
            get => (bool)GetValue(FlipControlProperty);
            set => SetValue(FlipControlProperty, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //base.OnPropertyChanged(propertyName);

            if (propertyName.Equals(FrontContentProperty.PropertyName))
            {
                FrontLayout.Content = FrontContent;
            }

            if (propertyName.Equals(BackContentProperty.PropertyName))
            {
                BackLayout.Content = BackContent;
            }

            if (propertyName.Equals(IsEnabledControlProperty.PropertyName))
            {
                ContentLayout.IsEnabled = IsEnabledControl;
                BackLayout.IsEnabled = IsEnabledControl;
                FrontLayout.IsEnabled = IsEnabledControl;
            }
        }


        private void FlipToBackExecute(Object sender, EventArgs e)
        {
            FlipToBack();
            Command?.Execute(BindingContext);
        }

        private void FlipToFrontExecute(Object sender, EventArgs e)
        {
            FlipToFront();
            Command?.Execute(BindingContext);
        }

        private async void FlipToFront()
        {
            await Flip(BackLayout, FrontLayout);
            FrontLayout.IsVisible = true;
            BackLayout.IsVisible = false;
        }

        private async void FlipToBack()
        {
            await Flip(FrontLayout, BackLayout);
            FrontLayout.IsVisible = false;
            BackLayout.IsVisible = true;
        }

        private async Task Flip(VisualElement from, VisualElement to)
        {
            await from.RotateYTo(-90, 300, Easing.SpringIn);
            to.RotationY = 90;
            to.IsVisible = true;
            from.IsVisible = false;
            await to.RotateYTo(0, 300, Easing.SpringOut);
        }

        private static void IsEnabledControlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var mainLayout = (FlippedControl)bindable;

            if (!(bool)newValue)
            {
                mainLayout.IsEnabled = false; //disabled Container Layout
                mainLayout.FrontLayout.IsEnabled = false; //disabled Front Container Layout
                mainLayout.BackLayout.IsEnabled = false; //disabled Back Container Layout
            }

            if ((bool)newValue)
            {
                mainLayout.IsEnabled = true; //disabled Container Layout
                mainLayout.FrontLayout.IsEnabled = true; //disabled Front Container Layout
                mainLayout.BackLayout.IsEnabled = true; //disabled Back Container Layout
            }
        }

        private static void FlipControlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var mainContent = (FlippedControl)bindable;

            if (mainContent.IsEnabled)
            {
                if ((bool)newValue)
                {
                    mainContent.FlipToFront();
                }

                if (!(bool)newValue)
                {
                    mainContent.FlipToBack();
                }
            }
        }

    }
}


