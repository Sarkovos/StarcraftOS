namespace StarcraftOS
{
    internal class Player
    {
        private static Player instance;
        private int turnCount = 0;

        // Private constructor to prevent external instantiation
        private Player() { }

        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        //Bool Version of Player Turn - Red is false, Blue is true
        public bool PlayerTurn()
        {
            return turnCount % 2 == 0;
        }

        // Expose a property to get the turnCount
        public int TurnCount
        {
            get { return turnCount; }
            set { turnCount = value; }
        }

        // Expose a method to increment the turnCount
        public void IncrementTurnCount()
        {
            turnCount++;
        }

        //String Version of Player Turn
        public string PlayerTurnString()
        {
            if (turnCount % 2 == 0)
            {
                return "Red";
            }

            else
            {
                return "Blue";
            }
        }


    }
}
