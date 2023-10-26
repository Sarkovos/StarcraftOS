using System;
using System.Collections.Generic;
using System.Linq;
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
        private Player player;
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
            player = new Player();
            gameType = new GameType(this);
            GridSize = defaultGridSize;
            DataContext = this;
            GridSizeTextBox.Text = GridSize.ToString();
            MakeGrid(GridSize);
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

                    gameType.UpdateGrid(x, y);
                    gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize);
                }
                
                else
                {
                    square.Content = "O";
                    square.FontSize = 24;
                    square.Foreground = Brushes.White;
                    gameType.UpdateGrid(x, y);
                    gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize);
                }
            }

        }

        public void TieGameText()
        {
            TieText.Visibility = Visibility.Visible;
        }

        private void TurnText()
        {

        }
    }
}
