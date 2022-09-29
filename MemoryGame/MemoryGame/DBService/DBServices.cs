using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MemoryGame.DBService;
using MemoryGame.Models;
using MemoryGame.Utils;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(DBServices))]
namespace MemoryGame.DBService
{
    public class DBServices : IDBServices
    {
        SQLiteAsyncConnection _db;

        async Task Init()
        {
            if (_db != null) return;

            // Get an absolute path to the database file, using Xamarin.Essentials: File System Helpers
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseFilename);

            _db = new SQLiteAsyncConnection(dbPath);

            //create tables
            await _db.CreateTableAsync<GameScores>();

        }


        public async Task AddScore(GameScores scores)
        {
            await Init();

            var scoresList = await GetGameScores();

            if (scoresList.Count() >= 10)
            {
                await DeleteScore(scoresList.Last().Id);
            }

            await _db.InsertAsync(scores);
        }

        public async Task DeleteScore(int id)
        {
            await Init();

            await _db.DeleteAsync(id);
        }

        public async Task<IEnumerable<GameScores>> GetGameScores()
        {
            await Init();
            var itemsList = await _db.Table<GameScores>().ToListAsync();
            return itemsList;
        }

    }
}

