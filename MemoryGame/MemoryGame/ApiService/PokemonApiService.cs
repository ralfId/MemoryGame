using MemoryGame.ApiService;
using MemoryGame.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(PokemonApiService))]
namespace MemoryGame.ApiService
{
    public class PokemonApiService : IPokemonApiService
    {
        HttpClient httpClient;
        public PokemonApiService()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(Constants.PokeBaseUrl)
            };
        }

        public async Task<PokemonList> GetPokemonList<PokemonList>(int limit)
        {

            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                Console.WriteLine("No internet conection");
                return default; ;
            }

            try
            {
                var url = $"?limit={limit}";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var pokeObject = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PokemonList>(pokeObject);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error to get api pokemons list");
                Console.WriteLine(ex.ToString());
                return default;

            }

            return default;
        }

    }
}
