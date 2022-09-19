using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryGame.Models
{
    public class PokemonList
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
        
        [JsonProperty("previos")]
        public object Previous { get; set; }
       
        [JsonProperty("results")]
        public List<Pokemon> Pokemons { get; set; }

    }
}
