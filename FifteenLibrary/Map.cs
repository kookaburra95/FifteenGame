namespace FifteenLibrary
{
    struct Map
    {
        private int size;
        private int[,] map;

        public Map(int size)
        {
            this.size = size;
            map = new int[size,size];
        }

        public void Set(Coordinates xyCoordinates, int value)
        {
            if (xyCoordinates.OnBoard(size))
            {
                map[xyCoordinates.x, xyCoordinates.y] = value;
            }
        }

        public int Get(Coordinates xyCoordinates)
        {
            if (xyCoordinates.OnBoard(size))
            {
                return map[xyCoordinates.x, xyCoordinates.y];
            }

            return 0;
        }

        public void Copy(Coordinates from, Coordinates to)
        {
            Set(to, Get(from));
        }
    }
}
