using System;
using SQLite;

namespace MemoryGame.Models
{
    public class GameScores
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Level { get; set; }
        public DateTime GameDate { get; set; }
        public string Moves { get; set; }
        public TimeSpan GameTime { get; set; }
    }
}

