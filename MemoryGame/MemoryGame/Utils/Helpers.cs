using System;
namespace MemoryGame.Utils
{
    public static class Helpers
    {
        public static string urlImage(string pokeUrl)
        {
            pokeUrl = pokeUrl.Substring(0, pokeUrl.Length - 1);
            pokeUrl = pokeUrl.Substring(pokeUrl.LastIndexOf("/"));

            return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon{pokeUrl}.png";
        }
    }
}

