using System.Collections.Generic;

namespace FifteenLibrary
{
    struct Coordinates
    {
        public int X; 
        public int Y;

        public Coordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
            
        public Coordinates(int size)
        {
            X = size - 1;
            Y = size - 1;
        }

        public bool OnBoard(int size)
        {
            if (X < 0 || X > size - 1)
            {
                return false;
            }

            if (Y < 0 || Y > size - 1)
            {
                return false;
            }
            
            return true;
        }

        public IEnumerable<Coordinates> YieldCoordinates(int size)
        {
            for (Y = 0; Y < size; Y++)
            {
                for (X = 0; X < size; X++)
                {
                    yield return this;
                }
            }
        }

        public Coordinates Add(int sx, int sy)
        {
            return new Coordinates(X + sx, Y + sy);
        }
    }
}
