﻿using LinkaWPF.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tobii.Interaction.Wpf;

namespace LinkaWPF
{
    /// <summary>
    /// Логика взаимодействия для CardBoard.xaml
    /// </summary>
    public partial class CardBoard : UserControl
    {
        private IList<CardButton> _buttons;
        private int _currentPage;
        private int _countPages;
        private int _gridSize;
        private CardButton _selectedCardButton;
        private Card _selectedCard;

        public CardBoard()
        {
            InitializeComponent();

            Init();
        }

        #region Properties
        // Properties
        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.Register("Cards", typeof(IList<Card>), typeof(CardBoard), new PropertyMetadata(null, new PropertyChangedCallback(OnCardsChanged)));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(CardBoard), new PropertyMetadata(3, new PropertyChangedCallback(GridSizeChanged)));

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(CardBoard), new PropertyMetadata(3, new PropertyChangedCallback(GridSizeChanged)));

        private static void OnCardsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CardBoard cardBoard = sender as CardBoard;
            var cards = (IList<Card>)args.NewValue;
            cardBoard.Update(cards);

            cardBoard.Edit();
        }

        private static void GridSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CardBoard cardBoard = sender as CardBoard;
            cardBoard?.Init();

            cardBoard?.Edit();
        }

        public IList<Card> Cards
        {
            get { return (IList<Card>)GetValue(CardsProperty); }
            set { SetValue(CardsProperty, value); }
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set
            {
                if (value <= 0) value = 1;
                SetValue(ColumnsProperty, value);
            }
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set
            {
                if (value <= 0) value = 1;
                SetValue(RowsProperty, value);
            }
        }

        public int CountPages
        {
            get { return _countPages; }
            private set
            {
                _countPages = value;
                CountPagesChanged?.Invoke(this, new EventArgs());
            }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                _currentPage = value;
                CurrentPageChanged?.Invoke(this, new EventArgs());
            }
        }

        public Card SelectedCard
        {
            get { return _selectedCard; }
            private set
            {
                _selectedCard = value;
                SelectedCardChanged?.Invoke(this, new EventArgs());
            }
        }

        public CardButton SelectedCardButton
        {
            get { return _selectedCardButton; }
            private set
            {
                _selectedCardButton = value;
                SelectedCard = _selectedCardButton != null ? _selectedCardButton.Card : null;
                SelectedCardButtonChanged?.Invoke(this, new EventArgs());
            }
        }

        public int SelectedIndex
        {
            get { return SelectedCard == null ? -1 : Cards.IndexOf(SelectedCard); }
        }

        public bool IsHasGazeEnabled
        {
            get { return (bool)GetValue(IsHasGazeEnabledProperty); }
            set { SetValue(IsHasGazeEnabledProperty, value); }
        }

        public bool IsAnimatedClickEnabled
        {
            get { return (bool)GetValue(IsAnimatedClickEnabledProperty); }
            set { SetValue(IsAnimatedClickEnabledProperty, value); }
        }

        public double ClickDelay
        {
            get { return (double)GetValue(ClickDelayProperty); }
            set { SetValue(ClickDelayProperty, value); }
        }

        public static readonly DependencyProperty ClickDelayProperty =
            DependencyProperty.Register("ClickDelay", typeof(double), typeof(CardBoard), new PropertyMetadata((double)3, new PropertyChangedCallback(ClickDelayChanged)));

        public static readonly DependencyProperty IsHasGazeEnabledProperty =
            DependencyProperty.Register("IsHasGazeEnabled", typeof(bool), typeof(CardBoard), new PropertyMetadata(new PropertyChangedCallback(IsHasGazeEnabledChanged)));

        public static readonly DependencyProperty IsAnimatedClickEnabledProperty =
            DependencyProperty.Register("IsAnimatedClickEnabled", typeof(bool), typeof(CardBoard), new PropertyMetadata(new PropertyChangedCallback(IsAnimatedClickEnabledChanged)));

        private static void ClickDelayChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var cardBoard = sender as CardBoard;
            cardBoard.ClickDelay = (double)args.NewValue;

            foreach (var cardButton in cardBoard._buttons)
            {
                cardButton.ClickDelay = cardBoard.ClickDelay;
            }
        }

        private static void IsHasGazeEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var cardBoard = sender as CardBoard;
            cardBoard.IsHasGazeEnabled = (bool)args.NewValue;

            foreach (var cardButton in cardBoard._buttons)
            {
                cardButton.IsHasGazeEnabled = cardBoard.IsHasGazeEnabled;
            }
        }

        private static void IsAnimatedClickEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var cardBoard = sender as CardBoard;
            cardBoard.IsAnimatedClickEnabled = (bool)args.NewValue;

            foreach (var cardButton in cardBoard._buttons)
            {
                cardButton.IsAnimatedClickEnabled = cardBoard.IsAnimatedClickEnabled;
            }
        }

        public bool IsMouseEnabled
        {
            get { return (bool)GetValue(IsMouseEnabledProperty); }
            set { SetValue(IsMouseEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsMouseEnabledProperty =
            DependencyProperty.Register("IsMouseEnabled",
                typeof(bool), typeof(CardBoard),
                new PropertyMetadata(false, new PropertyChangedCallback(IsMouseEnabledChanged)));

        private static void IsMouseEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var cardBoard = (CardBoard)sender;
            cardBoard.IsMouseEnabled = (bool)args.NewValue;

            foreach (var cardButton in cardBoard._buttons)
            {
                cardButton.IsMouseEnabled = cardBoard.IsMouseEnabled;
            }
        }
        #endregion

        #region Events
        // Events
        public event EventHandler ClickOnCardButton;

        public event EventHandler CountPagesChanged;

        public event EventHandler CurrentPageChanged;

        public event EventHandler SelectedCardChanged;

        public event EventHandler SelectedCardButtonChanged;

        public event EventHandler Edited;

        #endregion

        #region Methods
        // Methods

        private void Init()
        {
            InitGrid();
            InitCardButtons();
            InitPages();
            Render();
        }

        private void InitGrid()
        {
            CurrentPage = 0;
            _gridSize = Rows * Columns;

            if (grid.Children.Count != 0) grid.Children.Clear();
            if (grid.RowDefinitions.Count != 0) grid.RowDefinitions.Clear();
            if (grid.ColumnDefinitions.Count != 0) grid.ColumnDefinitions.Clear();

            for (var i = 0; i < Rows; i++)
            {
                var rowDefinition = new RowDefinition();
                grid.RowDefinitions.Add(rowDefinition);
            }

            for (var i = 0; i < Columns; i++)
            {
                var columnDefinition = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columnDefinition);
            }
        }

        private void InitCardButtons()
        {
            if (_buttons == null)
            {
                _buttons = new List<CardButton>();
            }
            else
            {
                _buttons.Clear();
            }

            // Создаем кнопки и раскладываем их по клеткам таблицы
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    var button = CreateCardButton();
                    button.Focusable = false;
                    button.Click += new RoutedEventHandler(CardButton_Click);
                    button.HasGazeChanged += CardButton_HasGazeChanged;
                    button.MouseEnter += CardButton_MouseEnter;
                    button.MouseLeave += CardButton_MouseLeave;

                    button.IsMouseEnabled = IsMouseEnabled;
                    button.IsHasGazeEnabled = IsHasGazeEnabled;
                    button.IsAnimatedClickEnabled = IsAnimatedClickEnabled;
                    button.ClickDelay = ClickDelay;

                    grid.Children.Add(button);
                    Grid.SetRow(button, i);

                    Grid.SetColumn(button, j);

                    _buttons.Add(button);
                }
            }
        }

        private void InitPages()
        {
            if (Cards == null || Cards.Count <= 0)
            {
                CountPages = 1;
                CurrentPage = 0;
                return;
            }

            // Рассчитываем максимальное количество страниц
            CountPages = Convert.ToInt32(Math.Ceiling((double)Cards.Count / _gridSize));

            if (CurrentPage >= CountPages) CurrentPage = CountPages - 1;
        }

        private void Render()
        {
            if (Cards == null)
            {
                for (var i = 0; i < _buttons.Count; i++)
                {
                    _buttons[i].Card = null;
                }
                return;
            }

            for (var i = CurrentPage * _gridSize; i < CurrentPage * _gridSize + _gridSize; i++)
            {
                Card card = null;
                if (i >= 0 && i < Cards.Count)
                {
                    card = Cards[i];
                }
                var count = i - CurrentPage * _gridSize;
                _buttons[count].Card = card;

                if (card == null)
                {
                    _buttons[count].Focusable = false;
                }
                else
                {
                    _buttons[count].Focusable = true;
                }
            }
        }

        private CardButton GetCardButtonFromIndex(int index)
        {
            if (Cards == null || index < 0 || index >= Cards.Count) return null;

            // Выясним на какой странице находится карточка
            var page = Convert.ToInt32(Math.Floor((double)index / _gridSize));

            if (page != CurrentPage) GoToPage(page);

            // Находится ли карточка на текущей странице
            if (page == CurrentPage)
            {
                // Вычисляем индекс кнопки на которой находится карточка
                var indexOfButtons = index - CurrentPage * _gridSize;

                return _buttons[indexOfButtons];
            }

            return null;
        }

        public void Update(IList<Card> cards)
        {
            if (Cards != cards)
            {
                Cards = cards;
            }

            InitPages();
            Render();

            Edit();
        }

        public void UpdateCard(int index, Card card)
        {
            var cardButton = GetCardButtonFromIndex(index);

            if (cardButton == null) return;

            // Обновляем карточку на кнопке            
            cardButton.Card = null;
            cardButton.Card = card;

            Edit();
        }

        public bool NextPage()
        {
            if (CountPages <= 1) return false;

            var index = CurrentPage + 1;

            if (index >= CountPages) index = 0;

            return GoToPage(index);
        }

        public bool PrevPage()
        {
            if (CountPages <= 1) return false;

            var index = CurrentPage - 1;

            if (index < 0) index = CountPages - 1;

            return GoToPage(index);
        }

        public bool GoToPage(int index)
        {
            if (CountPages <= 1) return false;

            if (index < 0 || index >= CountPages) return false;

            RemoveSelectionCard();

            CurrentPage = index;

            Render();

            GC.Collect();

            return true;
        }

        public void SelectCard(int index)
        {
            var cardButton = GetCardButtonFromIndex(index);

            SelectCard(cardButton);
        }

        public void SelectCard(CardButton cardButton)
        {
            RemoveSelectionCard();

            SelectedCardButton = cardButton;

            if (SelectedCardButton != null) SelectedCardButton.Background = Brushes.Yellow;
        }

        public void RemoveSelectionCard()
        {
            if (SelectedCardButton == null) return;

            SelectedCardButton.Background = Brushes.White;
            SelectedCardButton = null;
        }

        public void RemoveCard()
        {
            if (SelectedCardButton == null || SelectedCardButton.Card == null) return;

            Cards.Remove(SelectedCardButton.Card);

            RemoveSelectionCard();

            Update(Cards);

            Edit();
        }

        public void MoveSelectorLeft()
        {
            if (Cards.Count == 0) return;

            var prevIndex = SelectedIndex >= 0 ? SelectedIndex - 1 : 0;

            if (prevIndex < 0) prevIndex = Cards.Count - 1;

            SelectCard(prevIndex);
        }

        public void MoveSelectorRight()
        {
            if (Cards.Count == 0) return;

            var nextIndex = SelectedIndex >= 0 ? SelectedIndex + 1 : 0;

            if (nextIndex >= Cards.Count) nextIndex = 0;

            SelectCard(nextIndex);
        }

        public void MoveSelectorUp()
        {
            if (Cards.Count == 0) return;

            if (SelectedIndex == -1)
            {
                SelectCard(0);
                return;
            }

            var index = SelectedIndex - Columns;

            if (index < 0) return;

            SelectCard(index);
        }

        public void MoveSelectorDown()
        {
            if (Cards.Count == 0) return;

            if (SelectedIndex == -1)
            {
                SelectCard(0);
                return;
            }

            // Вычисляем строку в которой находится выделенная карточка
            var row = Convert.ToInt32(Math.Floor((double)SelectedIndex / Columns));

            var index = SelectedIndex + Columns;

            if (row >= Rows - 1 || index >= Cards.Count) return;

            SelectCard(index);
        }

        public void MoveCardLeft()
        {
            // Переместить карточку влево
            if (SelectedCardButton == null || SelectedCardButton.Card == null || Cards.Count == 0) return;

            var index = Cards.IndexOf(SelectedCardButton.Card);

            if (index <= 0 || index >= Cards.Count) return;

            var prevCard = Cards[index - 1];
            Cards[index - 1] = Cards[index];
            Cards[index] = prevCard;

            Update(Cards);

            Edit();
        }

        public void MoveCardRight()
        {
            // Переместить карточку вправо
            if (SelectedCardButton == null || SelectedCardButton.Card == null) return;

            var index = Cards.IndexOf(SelectedCardButton.Card);

            // Если текущая карточка последняя, то перемещаем ее и подставляем фейковую карточку перед ней
            if (index == Cards.Count - 1)
            {
                Cards.Add(Cards[index]);
                Cards[index] = new Card() { CardType = CardType.Fake };
            }
            else
            {
                var nextCard = Cards[index + 1];
                Cards[index + 1] = Cards[index];
                Cards[index] = nextCard;
            }

            Update(Cards);

            Edit();
        }


        public void MoveCardUp()
        {
            // Переместить карточку влево
            if (SelectedCardButton == null || SelectedCardButton.Card == null || Cards.Count == 0) return;

            var index = Cards.IndexOf(SelectedCardButton.Card);

            if (index - Columns < 0 || index >= Cards.Count) return;

            var prevCard = Cards[index - Columns];
            Cards[index - Columns] = Cards[index];
            Cards[index] = prevCard;

            Update(Cards);

            Edit();
        }

        public void MoveCardDown()
        {
            // Переместить карточку вправо
            if (SelectedCardButton == null || SelectedCardButton.Card == null) return;

            var index = Cards.IndexOf(SelectedCardButton.Card);
            var sindex = index;
            if ((index + Columns) / Columns > Rows - 1) return;
            var newIndex = index + Columns;
            if (Cards.Count < newIndex)
            {
                while (index < newIndex)
                {
                    MoveCardRight();
                    index++;
                    SelectedCardButton = _buttons[index];
                }
            }
            else
            {
                var tmp = Cards[newIndex];
                Cards[newIndex] = Cards[index];
                Cards[index] = tmp;
            }
            Update(Cards);

            Edit();
        }

        protected virtual CardButton CreateCardButton()
        {
            return new CardButton();
        }

        protected virtual void CardButton_Click(object sender, RoutedEventArgs e)
        {
            PressOnCardButton(sender);
        }

        protected void PressOnCardButton(object sender)
        {
            ClickOnCardButton?.Invoke(sender, new EventArgs());
        }

        private void Edit()
        {
            Edited?.Invoke(this, new EventArgs());
        }

        protected virtual void CardButton_HasGazeChanged(object sender, HasGazeChangedRoutedEventArgs e)
        {
            var cardButton = sender as CardButton;

            if (cardButton.Card == null) return;

            if (IsHasGazeEnabled == true)
            {
                if (e.HasGaze == true)
                {
                    SelectCard(cardButton);
                }
                else
                {
                    RemoveSelectionCard();
                }
            }
        }

        protected virtual void CardButton_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        protected virtual void CardButton_MouseLeave(object sender, MouseEventArgs e)
        {
        }
        #endregion
    }
}
