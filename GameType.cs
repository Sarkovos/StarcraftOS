using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StarcraftOS
{
    class GameType
    {

        public List<List<int>> gameBoard { get; set; }
        Player player;
        MainWindow gameWindow;

        public GameType(MainWindow window)
        {
            gameBoard = new List<List<int>>();
            player = new Player();
            gameWindow = window;
        }

        public void CreateGrid(int gridSize)
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

        public void UpdateGrid(int x, int y)
        {
            if (player.PlayerTurn())
            gameBoard[x][y] = 1; else gameBoard[x][y] = 2;
        }

        public void IsGridFull(bool SimpleGame, int gridSize)
        {

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (gameBoard[i][j] == 0)
                    {
                        return;
                    }
                }
            }

            if (SimpleGame) 
            {
                SimpleDraw();
            }

            else
            {
                //put function to handle scoring general game
                return;
            }

        }

        public void SimpleDraw()
        {
            gameWindow.TieGameText();
        }






    }
}
