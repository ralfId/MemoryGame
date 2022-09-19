using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryGame.Models
{
    public class Pokemon
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]

        public string Url { get; set; }
        [JsonIgnore]
        public string Image => urlImage();
        public string urlImage()
        {
            string pokeUrl = Url;
            pokeUrl = pokeUrl.Substring(0, pokeUrl.Length - 1);
            pokeUrl = pokeUrl.Substring(pokeUrl.LastIndexOf("/"));

            return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon{pokeUrl}.png";
        }
    }
}
