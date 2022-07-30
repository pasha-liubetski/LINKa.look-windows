﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Tobii.Interaction.Wpf;

namespace LinkaWPF
{
    public class AnimatedButton : Button
    {
        private CircularProgressBar _progress;
        private Storyboard _sb;
        private Grid _grid;
        private Brush _prevBackground;
        private DoubleAnimation _animation;
        private bool _isHasGaze;

        static AnimatedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedButton), new FrameworkPropertyMetadata(typeof(AnimatedButton)));
        }

        public AnimatedButton()
        {
            IsHasGaze = true;
            IsMouseEnabled = true;
            // Behaviors.SetIsGazeAware(this, true);
            // Behaviors.SetIsActivatable(this, true);
            // Behaviors.SetGazeAwareDelayTime(this, 200);
            // Behaviors.SetGazeAwareMode(this, Tobii.Interaction.Framework.GazeAwareMode.Normal);
            // Behaviors.SetIsTentativeFocusEnabled(this, true);

            Behaviors.SetIsGazeAware(this, true);
            Behaviors.AddHasGazeChangedHandler(this, OnHasGazeChanged);

            _progress = new CircularProgressBar()
            {
                StrokeThickness = 5,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden
            };

            _animation = new DoubleAnimation();
            _animation.From = 0;
            _animation.To = 100;
            _animation.Duration = TimeSpan.FromSeconds(3);

            _animation.Completed += new EventHandler((sender, e) =>
            {
                _progress.Visibility = Visibility.Hidden;
                OnClick();
            });

            Storyboard.SetTarget(_animation, _progress);
            Storyboard.SetTargetProperty(_animation, new PropertyPath(CircularProgressBar.PercentageProperty));

            _sb = new Storyboard();
            _sb.Children.Add(_animation);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (IsMouseEnabled == false)
                e.Handled = true;
            base.OnMouseDown(e);
        }

        protected virtual void OnHasGazeChanged(object sender, HasGazeChangedRoutedEventArgs e)
        {
            var button = sender as Button;
            _isHasGaze = e.HasGaze;

            if (e.HasGaze == true)
            {
                StartClick();
            }
            else
            {
                StopClick();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _prevBackground = Background;
            _grid = Template.FindName("grid", this) as Grid;
            _grid.Children.Add(_progress);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            StopClick();
            Background = _prevBackground;
            base.OnLostFocus(e);
        }

        protected void StartClick()
        {
            if (IsEnabled == true && IsHasGazeEnabled == true && IsHasGaze == true)
            {
                Focus();
                // Background = Brushes.Yellow;

                if (IsAnimatedClickEnabled == true)
                {
                    _progress.Visibility = Visibility.Visible;
                    _progress.Radius = ActualHeight < ActualWidth ? Convert.ToInt32((ActualHeight - 20) / 2) : Convert.ToInt32((ActualWidth - 20) / 2);
                    _sb.Begin();
                }
            }
        }

        protected void StopClick()
        {
            // if (_isHasGaze == false || (_isHasGaze == true && IsHasGazeEnabled == false)) Background = _prevBackground;
            _progress.Visibility = Visibility.Hidden;
            _sb.Stop();
        }

        public double ClickDelay
        {
            get { return (double)GetValue(ClickDelayProperty); }
            set { SetValue(ClickDelayProperty, value); }
        }

        public static readonly DependencyProperty ClickDelayProperty =
            DependencyProperty.Register("ClickDelay",
                typeof(double), typeof(AnimatedButton),
                new PropertyMetadata((double)3, new PropertyChangedCallback(ClickDelayChanged)));

        private static void ClickDelayChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var button = sender as AnimatedButton;
            button.ClickDelay = (double)args.NewValue;
            button._animation.Duration = TimeSpan.FromSeconds((double)args.NewValue);
        }

        public bool IsHasGazeEnabled
        {
            get { return (bool)GetValue(IsHasGazeEnabledProperty); }
            set { SetValue(IsHasGazeEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsHasGazeEnabledProperty =
            DependencyProperty.Register("IsHasGazeEnabled",
                typeof(bool), typeof(AnimatedButton),
                new PropertyMetadata(new PropertyChangedCallback(IsHasGazeEnabledChanged)));

        private static void IsHasGazeEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var button = sender as AnimatedButton;
            button.IsHasGazeEnabled = (bool)args.NewValue;
            if (button.IsHasGazeEnabled == false)
            {
                button.StopClick();
            }
        }

        public bool IsAnimatedClickEnabled
        {
            get { return (bool)GetValue(IsAnimatedClickEnabledProperty); }
            set { SetValue(IsAnimatedClickEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatedClickEnabledProperty =
            DependencyProperty.Register("IsAnimatedClickEnabled",
                typeof(bool), typeof(AnimatedButton),
                new PropertyMetadata(false, new PropertyChangedCallback(IsAnimatedClickEnabledChanged)));

        private static void IsAnimatedClickEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var button = sender as AnimatedButton;
            button.IsAnimatedClickEnabled = (bool)args.NewValue;
            if (button.IsAnimatedClickEnabled == false)
            {
                button.StopClick();
            }
        }

        public bool IsMouseEnabled
        {
            get { return (bool)GetValue(IsMouseEnabledProperty); }
            set { SetValue(IsMouseEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsMouseEnabledProperty =
            DependencyProperty.Register("IsMouseEnabled",
                typeof(bool), typeof(AnimatedButton),
                new PropertyMetadata(false, new PropertyChangedCallback(IsMouseEnabledChanged)));

        private static void IsMouseEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var button = sender as AnimatedButton;
            button.IsMouseEnabled = (bool)args.NewValue;
        }

        protected bool IsHasGaze { get; set; }
    }
}
