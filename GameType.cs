using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace StarcraftOS
{
    class GameType
    {

        public List<List<int>> gameBoard { get; set; }
        MainWindow gameWindow;
        int redScore = 0;
        int blueScore = 0;


        public GameType(MainWindow window)
        {
            gameBoard = new List<List<int>>();
            gameWindow = window;
        }

        public void CreateGridMatrix(int gridSize)
        {
            gameBoard.Clear();
            for (int i = 0; i < gridSize; i++)
            {
                gameBoard.Add(new List<int>());
                for (int j = 0; j < gridSize; j++)
                {
                    gameBoard[i].Add(0);
                }
            }
        }

        public void UpdateGrid(int x, int y, bool isS)
        {
            if (isS)
            gameBoard[x][y] = 1; else gameBoard[x][y] = 2;
        }

        public bool IsGridFull(bool SimpleGame, int gridSize)
        {

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (gameBoard[i][j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;

        }


        public void Scoring()
        {
            if (Player.Instance.PlayerTurn())
            {
                blueScore++;
            }

            else
            {
                redScore++;
            }
        }

        public string Winner(bool SimpleGame)
        {
            if (SimpleGame)
            {
                if (blueScore >= 1)
                {
                    return "Blue";
                }

                if (redScore >= 1)
                {
                    return "Red";
                }

                return "null";


            }

            else
            {
                if (blueScore == redScore)
                {
                    return "null";
                }
                else if (blueScore > redScore) 
                {
                    return "Blue";
                }
                else
                {
                    return "Red";
                } 
            }
        }

        public void ResetScore()
        {
            blueScore = 0;
            redScore = 0;
        }

        public void CheckForSOS(int x, int y, bool isS)
        {
            //1 is S, 2 is O

            //S Conditions
            if (isS)
            {
                // Check if the current position is within bounds
                if (x - 2 >= 0 && y - 2 >= 0)
                {
                    // S * * * *
                    // * O * * *
                    // * * S * *
                    // * * * * *
                    // * * * * *
                    if (gameBoard[x - 1][y - 1] == 2 && gameBoard[x - 2][y - 2] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x - 2 >= 0)
                {
                    // * * S * *
                    // * * O * *
                    // * * S * *
                    // * * * * *
                    // * * * * *
                    if (gameBoard[x - 1][y] == 2 && gameBoard[x - 2][y] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x - 2 >= 0 && y + 2 < gameBoard[0].Count)
                {
                    // * * * * S
                    // * * * O *
                    // * * S * *
                    // * * * * *
                    // * * * * *
                    if (gameBoard[x - 1][y + 1] == 2 && gameBoard[x - 2][y + 2] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (y + 2 < gameBoard[0].Count)
                {
                    // * * * * *
                    // * * * * *
                    // * * S O S
                    // * * * * *
                    // * * * * *
                    if (gameBoard[x][y + 1] == 2 && gameBoard[x][y + 2] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x + 2 < gameBoard.Count && y + 2 < gameBoard[0].Count)
                {
                    // * * * * *
                    // * * * * *
                    // * * S * *
                    // * * * O *
                    // * * * * S
                    if (gameBoard[x + 1][y + 1] == 2 && gameBoard[x + 2][y + 2] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x + 2 < gameBoard.Count)
                {
                    // * * * * *
                    // * * * * *
                    // * * S * *
                    // * * O * *
                    // * * S * *
                    if (gameBoard[x + 1][y] == 2 && gameBoard[x + 2][y] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x + 2 < gameBoard.Count && y - 2 >= 0)
                {
                    // * * * * *
                    // * * * * *
                    // S O S * *
                    // * * * * *
                    // * * * * *
                    if (gameBoard[x + 1][y - 1] == 2 && gameBoard[x + 2][y - 2] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (y - 2 >= 0 && x + 2 < gameBoard.Count && y - 2 < gameBoard[x].Count)
                {
                    // * * * * *
                    // * * * * *
                    // * * S * *
                    // * O * * *
                    // S * * * *
                    if (gameBoard[x + 1][y - 1] == 2 && gameBoard[x + 2][y - 2] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (y - 2 >= 0)
                {
                    // * * * * *
                    // * * * * *
                    // S O S * *
                    // * * * * *
                    // * * * * *
                    if (gameBoard[x][y - 1] == 2 && gameBoard[x][y - 2] == 1)
                    {
                        Scoring();
                    }
                }
            }

            // O Conditions
            else
            {
                // Check if the current position is within bounds
                if (x - 1 >= 0 && y - 1 >= 0 && x + 1 < gameBoard.Count && y + 1 < gameBoard[0].Count)
                {
                    // S * *
                    // * O *
                    // * * S
                    if (gameBoard[x - 1][y - 1] == 1 && gameBoard[x + 1][y + 1] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x - 1 >= 0 && y + 1 < gameBoard[0].Count && x + 1 < gameBoard.Count && y - 1 >= 0)
                {
                    // * * S
                    // * O *
                    // S * *
                    if (gameBoard[x + 1][y - 1] == 1 && gameBoard[x - 1][y + 1] == 1)
                    {
                        Scoring();
                    }
                }

                // Check if the current position is within bounds
                if (x - 1 >= 0 && x + 1 < gameBoard.Count)
                {
                    // * S *
                    // * O *
                    // * S *
                    if (gameBoard[x - 1][y] == 1 && gameBoard[x + 1][y] == 1)
                    {

                        Scoring();

                    }

                    // Check if the current position is within bounds
                    if (y - 1 >= 0 && y + 1 < gameBoard[0].Count)
                    {
                        // * * *
                        // S O S
                        // * * *
                        if (gameBoard[x][y - 1] == 1 && gameBoard[x][y + 1] == 1)
                        {

                            Scoring();

                        }
                    }
                }
            }
        }
    }
}
