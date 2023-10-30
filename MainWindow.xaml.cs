using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StarcraftOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double SIZEOFGRID = 400.0;
        int defaultGridSize = 8;

        private int _gridSize;
        private GameType gameType;

        public int GridSize
        {
            get { return _gridSize; }
            
            set
            {
                if (_gridSize != value)
                {
                    _gridSize = value;
                    GridSizeTextBox.Text = _gridSize.ToString();
                }
            }
        }

        public Grid GetSOSGrid()
        {
            return SOSGrid;
        }

        public MainWindow()
        {
            InitializeComponent();
            gameType = new GameType(this);
            GridSize = defaultGridSize;
            DataContext = this;
            GridSizeTextBox.Text = GridSize.ToString();
            MakeGrid(GridSize);

            turnTextBlock.Inlines.Clear();
            Run runPlayerText = new Run($"Player Turn: {Player.Instance.PlayerTurnString()}");
            turnTextBlock.Inlines.Add(runPlayerText);

        }

        private void MakeGrid(int n)
        {
            gameType.CreateGrid(GridSize);

            double buttonSize = SIZEOFGRID / n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    //Make Sure it doesnt say there is a tie game
                    TieText.Visibility = Visibility.Hidden;

                    Button square = new Button();
                    square.Height = square.Width = buttonSize;

                    // Set the border color and thickness
                    square.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                    square.BorderThickness = new Thickness(1);

                    //Set Square Background Color
                    square.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#141414"));


                    var row = new RowDefinition();
                    var col = new ColumnDefinition();
                    row.Height = GridLength.Auto;
                    col.Width = GridLength.Auto;
                    SOSGrid.RowDefinitions.Add(row);
                    SOSGrid.ColumnDefinitions.Add(col);
                    square.Click += SquareClicked;
                    Grid.SetRow(square, i);
                    Grid.SetColumn(square, j);
                    SOSGrid.Children.Add(square);
                    
                }
            }
        }

        private void ClearGrid()
        { 
            SOSGrid.Children.Clear();
        }

        private void ClickUp(object sender, RoutedEventArgs e)
        {
            ClearGrid();
            GridSize++;
            MakeGrid(GridSize);
        }

        private void ClickDown(object sender, RoutedEventArgs e)
        {
            ClearGrid();
            GridSize--;
            MakeGrid(GridSize);
        }

        private void SizeTextChanged (object sender, RoutedEventArgs e)
        {
            //This line is taking the text in the box, checking to see if it can be converted to an integer (Through TryParse) and if its true, it assigns the text to newSize)
            if (int.TryParse(GridSizeTextBox.Text, out int newSize))
            {
                ClearGrid();
                GridSize = newSize;
                MakeGrid(GridSize);
            }
        }


        private void SquareClicked(object sender, RoutedEventArgs e)
        {
            Button square = (Button)sender;

            //Get the position of that square
            int x = Grid.GetRow(square);
            int y = Grid.GetColumn(square);

            //Check if the button has been clicked before
            if (gameType.gameBoard[x][y] == 0)
            {
                if (SButton.IsChecked == true)
                {
                    square.Content = "S";
                    square.FontSize = 24;
                    square.Foreground = Brushes.White;

                    gameType.UpdateGrid(x, y, (bool)SButton.IsChecked);
                    gameType.CheckForSOS(x, y, (bool)SButton.IsChecked);
                    Player.Instance.IncrementTurnCount();

                    turnTextBlock.Inlines.Clear();
                    Run runPlayerText = new Run($"Player Turn: {Player.Instance.PlayerTurnString()}");
                    turnTextBlock.Inlines.Add(runPlayerText);

                }
                
                else
                {
                    square.Content = "O";
                    square.FontSize = 24;
                    square.Foreground = Brushes.White;
                    gameType.UpdateGrid(x, y, (bool)SButton.IsChecked);
                    gameType.CheckForSOS(x, y, (bool)SButton.IsChecked);
                    Player.Instance.IncrementTurnCount();

                    turnTextBlock.Inlines.Clear();
                    Run runPlayerText = new Run($"Player Turn: {Player.Instance.PlayerTurnString()}");
                    turnTextBlock.Inlines.Add(runPlayerText);


                }
            }    
            
            if ((bool)SimpleButton.IsChecked)
            {
                if (gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize))
                {
                    gameType.SimpleDraw();
                }

                else
                {
                    string winner = gameType.Winner((bool)SimpleButton.IsChecked);
                    if (winner != "null")
                    {
                        Run runWinner = new Run($"{winner} has won!");
                        runWinner.Foreground = Brushes.White;
                        runWinner.FontSize = 40;
                        

                        TextBlock winnerTextBlock = new TextBlock();
                        winnerTextBlock.Inlines.Add(runWinner);
                        winnerTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        winnerTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                        winnerTextBlock.Margin = new Thickness(10);


                        MainGrid.Children.Add(winnerTextBlock);

                    }
                    
                }
            }

            else
            {
                if (gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize))
                {
                    string winner = gameType.Winner((bool)SimpleButton.IsChecked);
                    Run runWinner = new Run($"{winner} has won!");
                    runWinner.Foreground = Brushes.White;
                    runWinner.FontSize = 40;


                    TextBlock winnerTextBlock = new TextBlock();
                    winnerTextBlock.Inlines.Add(runWinner);
                    winnerTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    winnerTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                    winnerTextBlock.Margin = new Thickness(10);


                    MainGrid.Children.Add(winnerTextBlock);
                }
            }

        }

        public void TieGameText()
        {
            TieText.Visibility = Visibility.Visible;
        }

        public void WinText()
        {
            WinnerText.Visibility = Visibility.Visible;
        }

    }
}
