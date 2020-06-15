namespace FifteenLibrary
{
    public interface IGame
    {
        public int Moves { get; set; }

        public void Start();//int seed = 0);

        public void Shuffle();//int seed); 

        public int CLickAt(int x, int y);

        public void Shift(int sx, int sy);

        public int GetDigitAt(int x, int y);

        public bool IsSolved();

        //TODO Добавить таймер


    }
}