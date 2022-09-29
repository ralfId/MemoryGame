using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryGame.Models;

namespace MemoryGame.DBService
{
    public interface IDBServices
    {
        Task AddScore(GameScores scores);
        Task DeleteScore(int id);
        Task<IEnumerable<GameScores>> GetGameScores();
        Task<IEnumerable<GameScores>> GetGameScoresByLevel(string Level);
    }
}