namespace FifteenLibrary
{
    struct Map
    {
        private readonly int _size;
        private readonly int[,] _map;

        public Map(int size)
        {
            this._size = size;
            _map = new int[size,size];
        }

        public void Set(Coordinates xyCoordinates, int value)
        {
            if (xyCoordinates.OnBoard(_size))
            {
                _map[xyCoordinates.X, xyCoordinates.Y] = value;
            }
        }

        public int Get(Coordinates xyCoordinates)
        {
            if (xyCoordinates.OnBoard(_size))
            {
                return _map[xyCoordinates.X, xyCoordinates.Y];
            }

            return 0;
        }

        public void Copy(Coordinates from, Coordinates to)
        {
            Set(to, Get(from));
        }
    }
}
