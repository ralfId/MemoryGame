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
        public int PokeId { get; set; }

        [JsonIgnore]
        public string Image { get; set; }

        [JsonIgnore]
        public bool IsEnabledItem { get; set; } = true;

        [JsonIgnore]
        public bool FlipItem { get; set; }

    }
}
