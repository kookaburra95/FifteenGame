using System;

namespace FifteenLibrary
{
    public class Game: IGame
    {
        private Map map;
        private Coordinates emptyCell;

        private int size;
        public int Moves { get; set; }

        public Game(int size)
        {
            this.size = size;
            map = new Map(size);
        }

        public void Start(int seed = 0)
        {
            int digit = 0;
            
            foreach (var xyCoordinates in new Coordinates().YieldCoordinates(size))
            {
                map.Set(xyCoordinates, ++digit);
            }

            emptyCell = new Coordinates(size);

            if (seed > 0)
            {
                Shuffle(seed);
            }

            Moves = 0;
        }

        public void Shuffle(int seed)
        {
            Random rand = new Random(seed);

            for (int i = 0; i < seed; i++)
            {
                CLickAt(rand.Next(size), rand.Next(size));
            }
        }

        public int CLickAt(int x, int y)
        {
            return CLickAt(new Coordinates(x, y));
        }

        private int CLickAt(Coordinates xyCoordinates)
        {
            if (emptyCell.Equals(xyCoordinates))
            {
                return 0;
            }

            if (xyCoordinates.x != emptyCell.x && xyCoordinates.y != emptyCell.y)
            {
                return 0;
            }

            int steps = Math.Abs(xyCoordinates.x - emptyCell.x) + Math.Abs(xyCoordinates.y - emptyCell.y);

            while (xyCoordinates.x != emptyCell.x)
            {
                Shift(Math.Sign(xyCoordinates.x - emptyCell.x), 0);
            }

            while (xyCoordinates.y != emptyCell.y)
            {
                Shift(0, Math.Sign(xyCoordinates.y - emptyCell.y));
            }

            Moves += steps;
            
            return steps;
        }

        public void Shift(int sx, int sy)
        {
            Coordinates next = emptyCell.Add(sx, sy);

            map.Copy(next, emptyCell);

            emptyCell = next;
        }

        public int GetDigitAt(int x, int y)
        {
            return GetDigitAt(new Coordinates(x, y));
        }
            
        private int GetDigitAt(Coordinates xyCoordinates)
        {
            if (emptyCell.Equals(xyCoordinates))
            {
                return 0;
            }
            
            return map.Get(xyCoordinates);
        }

        public bool IsSolved()
        {
            if (!emptyCell.Equals(new Coordinates(size)))
            {
                return false;
            }


            int digit = 0;

            foreach (Coordinates xyCoordinates in new Coordinates().YieldCoordinates(size))
            {
                if (map.Get(xyCoordinates) != ++digit)
                {
                    return emptyCell.Equals(xyCoordinates);
                }
            }

            return true;
        }
    }
}
