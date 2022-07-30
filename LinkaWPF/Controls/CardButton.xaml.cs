using LinkaWPF.Models;
using System;
using System.Windows;
using Tobii.Interaction.Wpf;

namespace LinkaWPF
{
    public partial class CardButton : AnimatedButton
    {
        public CardButton()
        {
            InitializeComponent();

            IsHasGaze = false;
        }

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public event EventHandler<HasGazeChangedRoutedEventArgs> HasGazeChanged;

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register("Card", typeof(Card), typeof(CardButton), new PropertyMetadata(null, new PropertyChangedCallback(OnCardChanged)));

        private static void OnCardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var button = sender as CardButton;
            button.Card = (Card)args.NewValue;

            button.IsHasGaze = button.Card == null ? false : true;
        }

        protected override void OnHasGazeChanged(object sender, HasGazeChangedRoutedEventArgs e)
        {
            base.OnHasGazeChanged(sender, e);

            HasGazeChanged?.Invoke(sender, e);
        }
    }
}
