

namespace Fifteen
{
    class Records
    {
        public string Name { get; set; }
        public int Moves { get; set; }
        public string Time { get; set; }
        public double TimeTotalMilliseconds { get; set; }

        public Records(string name, int moves, string time, double timeTotalMilliseconds)
        {
            Name = name;
            Moves = moves;
            Time = time;
            TimeTotalMilliseconds = timeTotalMilliseconds;
        }
    }
}
    