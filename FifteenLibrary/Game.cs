using System;

namespace FifteenLibrary
{
    public class Game: IGame
    {
        private Map _map;
        private Coordinates _emptyCell;
        private bool _firstGame = true;

        private readonly int _size;
        public int Moves { get; set; }

        public Game(int size)
        {
            this._size = size;
            _map = new Map(size);
        }

        public void Start()
        {
            if (_firstGame)
            { 
                Shuffle();
                _firstGame = false;
            }
        }

        public void Shuffle()
        {
            int digit = 0;

            foreach (var xyCoordinates in new Coordinates().YieldCoordinates(_size))
            {
                _map.Set(xyCoordinates, ++digit);
            }

            _emptyCell = new Coordinates(_size);

            Random rand = new Random();

            for (int i = 0; i < rand.Next(); i++)
            {
                CLickAt(rand.Next(_size), rand.Next(_size));
            }

            Moves = 0;
        }

        public int CLickAt(int x, int y)
        {
            return CLickAt(new Coordinates(x, y));
        }

        private int CLickAt(Coordinates xyCoordinates)
        {
            if (_emptyCell.Equals(xyCoordinates))
            {
                return 0;
            }

            if (xyCoordinates.X != _emptyCell.X && xyCoordinates.Y != _emptyCell.Y)
            {
                return 0;
            }

            int steps = Math.Abs(xyCoordinates.X - _emptyCell.X) + Math.Abs(xyCoordinates.Y - _emptyCell.Y);

            while (xyCoordinates.X != _emptyCell.X)
            {
                Shift(Math.Sign(xyCoordinates.X - _emptyCell.X), 0);
            }

            while (xyCoordinates.Y != _emptyCell.Y)
            {
                Shift(0, Math.Sign(xyCoordinates.Y - _emptyCell.Y));
            }

            Moves += steps;
            
            return steps;
        }

        public void Shift(int sx, int sy)
        {
            Coordinates next = _emptyCell.Add(sx, sy);

            _map.Copy(next, _emptyCell);

            _emptyCell = next;
        }

        public int GetDigitAt(int x, int y)
        {
            return GetDigitAt(new Coordinates(x, y));
        }
            
        private int GetDigitAt(Coordinates xyCoordinates)
        {
            if (_emptyCell.Equals(xyCoordinates))
            {
                return 0;
            }
            
            return _map.Get(xyCoordinates);
        }

        public bool IsSolved()
        {
            if (!_emptyCell.Equals(new Coordinates(_size)))
            {
                return false;
            }


            int digit = 0;

            foreach (Coordinates xyCoordinates in new Coordinates().YieldCoordinates(_size))
            {
                if (_map.Get(xyCoordinates) != ++digit)
                {
                    return _emptyCell.Equals(xyCoordinates);
                }
            }

            return true;
        }
    }
}
