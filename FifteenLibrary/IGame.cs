namespace FifteenLibrary
{
    public interface IGame
    {
        public int Moves { get; set; }

        public void Start();

        public void Shuffle();

        public int CLickAt(int x, int y);

        public void Shift(int sx, int sy);

        public int GetDigitAt(int x, int y);

        public bool IsSolved();
    }
}