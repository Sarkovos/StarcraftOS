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

        //Bool Version of Player Turn - Red is true, Blue is false
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
            if (PlayerTurn())
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
