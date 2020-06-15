using System.Collections.Generic;

namespace FifteenLibrary
{
    struct Coordinates
    {
        public int x; 
        public int y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coordinates(int size)
        {
            x = size - 1;
            y = size - 1;
        }

        public bool OnBoard(int size)
        {
            if (x < 0 || x > size - 1)
            {
                return false;
            }

            if (y < 0 || y > size - 1)
            {
                return false;
            }
            
            return true;
        }

        public IEnumerable<Coordinates> YieldCoordinates(int size)
        {
            for (y = 0; y < size; y++)
            {
                for (x = 0; x < size; x++)
                {
                    yield return this;
                }
            }
        }

        public Coordinates Add(int sx, int sy)
        {
            return new Coordinates(x + sx, y + sy);
        }
    }
}
