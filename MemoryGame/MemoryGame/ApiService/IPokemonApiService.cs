using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.ApiService
{
    public interface IPokemonApiService
    {
        Task<PokemonList> GetPokemonList<PokemonList>(int limit);
    }
}
