using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Windose.UI.Xaml.Controls
{
    [ContentProperty(Name = "Child")]
    public class ClassicBorder : Control
    {
        protected const string NormalState = "Normal";
        protected const string ReverseState = "Pressed";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassicBorder"/> class.
        /// </summary>
        public ClassicBorder() => DefaultStyleKey = typeof(ClassicBorder);

        #region Child

        /// <summary>
        /// Identifies the <see cref="Child"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register(
                nameof(Child),
                typeof(UIElement),
                typeof(ClassicBorder),
                null);

        public UIElement Child
        {
            get => (UIElement)GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }

        #endregion

        #region ShadowBorderBrush

        /// <summary>
        /// Identifies the <see cref="ShadowBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowBorderBrushProperty =
            DependencyProperty.Register(
                nameof(ShadowBorderBrush),
                typeof(Brush),
                typeof(ClassicBorder),
                null);

        public Brush ShadowBorderBrush
        {
            get => (Brush)GetValue(ShadowBorderBrushProperty);
            set => SetValue(ShadowBorderBrushProperty, value);
        }

        #endregion

        #region IsPressed

        /// <summary>
        /// Identifies the <see cref="IsPressed"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register(
                nameof(IsPressed),
                typeof(bool),
                typeof(ClassicBorder),
                new PropertyMetadata(false, OnIsPressedPropertyChanged));

        public bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedProperty, value);
        }

        private static void OnIsPressedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is ClassicBorder border)
            {
                border.OnIsPressedPropertyChanged((bool)args.NewValue);
            }
        }

        #endregion

        private void OnIsPressedPropertyChanged(bool newValue)
        {
            VisualStateManager.GoToState(this, newValue ? ReverseState : NormalState, true);
        }
    }
}
