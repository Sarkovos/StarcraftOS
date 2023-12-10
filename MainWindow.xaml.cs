using System;
using System.IO;
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
using Path = System.IO.Path;

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

        //Pass the filepath and filename to the StreamWriter Constructor
        


        string winner = "null";

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

        private void InitializeFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            // Check if the file already exists
            if (File.Exists(filePath))
            {
                // If not, create a new file with some initial content
                using (StreamWriter initialWriter = new StreamWriter(filePath, false))
                {
                    initialWriter.WriteLine("==== New Run ====");
                }
            }
        }

        public void WriteLinesToFile(string fileName, string line)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                // Use StreamWriter to write lines to the file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeFile("Recording.txt");
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
            gameType.CreateGridMatrix(GridSize);

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
            Player.Instance.TurnCount = 0;
            
            turnTextBlock.Inlines.Clear();
            Run runPlayerText = new Run($"Player Turn: {Player.Instance.PlayerTurnString()}");
            turnTextBlock.Inlines.Add(runPlayerText);
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
            if (winner == "null")
            {
                buttonLogic(sender);
            }
        }

        private void buttonLogic(object sender)
        {
            Button square = (Button)sender;

            //Get the position of that square
            int x = Grid.GetRow(square);
            int y = Grid.GetColumn(square);

            //Check if the button has been clicked before
            if (gameType.gameBoard[x][y] == 0)
            {
                //S Text Details
                if (SButton.IsChecked == true)
                {
                    square.Content = "S";
                    square.FontSize = 24;
                    square.Foreground = Brushes.White;
                }

                //O Text Details
                else
                {
                    square.Content = "O";
                    square.FontSize = 24;
                    square.Foreground = Brushes.White;
                }

                gameType.UpdateGrid(x, y, (bool)SButton.IsChecked);

                Run runPlayerText = new Run(" ");
                Player.Instance.IncrementTurnCount();                

                turnTextBlock.Inlines.Clear();
                runPlayerText = new Run($"Player Turn: {Player.Instance.PlayerTurnString()}");

                if (SButton.IsChecked == true)
                {
                    WriteLinesToFile("Recording.txt", $"Player Turn: {Player.Instance.PlayerTurnString()} playing S on {x}, {y}");
                }

                else
                {
                    WriteLinesToFile("Recording.txt", $"Player Turn: {Player.Instance.PlayerTurnString()} playing O on {x}, {y}");
                }

                turnTextBlock.Inlines.Add(runPlayerText);
                gameType.CheckForSOS(x, y, (bool)SButton.IsChecked);
                winRules();

                if ((bool)BlueComputer.IsChecked || (bool)RedComputer.IsChecked)
                {

                    //red is true, blue is false
                    if (Player.Instance.PlayerTurn())
                    {

                        if ((bool)RedComputer.IsChecked)
                        {
                            computerMove();
                        }
                    }
                    else
                    {
                        if ((bool)BlueComputer.IsChecked)
                        {
                            computerMove();
                        }
                    }
                }
            }

            

        }

        private void winRules()
        {
             

            //If Simple Game...
            if ((bool)SimpleButton.IsChecked)
            {
                //If the grid is full in a simple game, it is a draw since no one got a point
                if (gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize))
                {
                    TieGameText();
                }

                //If it is not full...
                else
                {
                    //Set the winner as a string
                    winner = gameType.Winner((bool)SimpleButton.IsChecked);

                    //If a winner has been determined...
                    if (winner != "null")
                    {
                        WinText(winner);
                    }
                }
            }

            //If a general game...
            else
            {
                if (gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize))
                {
                    //Set the winner as a string
                    winner = gameType.Winner((bool)SimpleButton.IsChecked);
                    if (winner != "null")
                    {
                        WinText((winner));
                    }

                    else
                    {
                        TieGameText();
                    }
                }
            }
        }


        //Make the "It's a draw!" text visible.
        public void TieGameText()
        {
            TieText.Visibility = Visibility.Visible;
        }

        private TextBlock winnerTextBlock;

        private void WinText(string winner)
        {
            // Check if winnerTextBlock is not null, and winner is not "null"
            if (winnerTextBlock != null && winner != "null")
            {
                // Remove existing winnerTextBlock
                MainGrid.Children.Remove(winnerTextBlock);
                winnerTextBlock = null;
            }

            if (winner != "null")
            {
                Run runWinner = new Run($"{winner} has won!");
                runWinner.Foreground = Brushes.White;
                runWinner.FontSize = 40;

                winnerTextBlock = new TextBlock();
                winnerTextBlock.Inlines.Add(runWinner);
                winnerTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                winnerTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                winnerTextBlock.Margin = new Thickness(10);

                MainGrid.Children.Add(winnerTextBlock);
            }
            else
            {
                // Remove the winnerTextBlock when winner is "null"
                MainGrid.Children.Remove(winnerTextBlock);
                winnerTextBlock = null;
            }
        }



        private void RestartClicked(object sender, RoutedEventArgs e)
        {
            ClearGrid();
            MakeGrid(GridSize);
            Run runPlayerText = new Run(" ");
            gameType.ResetScore();
            winner = "null";
            WinText(winner);
            if ((bool)RedComputer.IsChecked)
            {
                computerMove();
            }
            
        }

        private void computerMove()
        {
            Random random = new Random();
            int x = random.Next(0, GridSize);
            int y = random.Next(0, GridSize);

            //Make sure the computer selects a move that is possible
            while (gameType.gameBoard[x][y] != 0 && !gameType.IsGridFull((bool)SimpleButton.IsChecked, GridSize))
            {
                x = random.Next(0, GridSize);
                y = random.Next(0, GridSize);
            }


            // Get the button at the specified row and column
            Button buttonToClick = SOSGrid.Children
                .OfType<Button>()
                .FirstOrDefault(b => Grid.GetRow(b) == x && Grid.GetColumn(b) == y);

            if (buttonToClick != null)
            {
                // Select a random radio button
                Random randomRadioButton = new Random();
                bool randomSButtonChecked = randomRadioButton.Next(2) == 0;

                // Set the corresponding radio button
                SButton.IsChecked = randomSButtonChecked;
                OButton.IsChecked = !randomSButtonChecked;
                buttonToClick.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            }
        }
    }
}
