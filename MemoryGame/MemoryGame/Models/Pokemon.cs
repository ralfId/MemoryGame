using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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

        [JsonIgnore]
        public bool IsEnabled { get; set; } = true;

        [JsonIgnore]
        public bool IsFlyped { get; set; } = false;


        [JsonIgnore]
        public ICommand CardCommand { get; set; }

        public string urlImage()
        {
            string pokeUrl = Url;
            pokeUrl = pokeUrl.Substring(0, pokeUrl.Length - 1);
            pokeUrl = pokeUrl.Substring(pokeUrl.LastIndexOf("/"));

            return $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon{pokeUrl}.png";
        }


    }
}
