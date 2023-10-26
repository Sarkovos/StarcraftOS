using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarcraftOS
{
    internal class Player
    {
        //Red Player starts first
        //red is false, blue is true
        int turnCount;
        public Player()
        {
            turnCount = 0;
        }
       
        public bool PlayerTurn()
        {
            turnCount++;
            if (turnCount % 2 == 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }






    }
}
