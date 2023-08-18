using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PokeAPI.Controllers
{
    public class PokemonController : ApiController
    {

        private const string PokeApiBaseUrl = "https://pokeapi.co/api/v2/";

        [HttpGet]
        [Route("api/pokemon/random")]
        public async Task<IHttpActionResult> GetRandomPokemons()
        {
            List<string> pokemonNames = await GetRandomPokemonNames(10);

            List<PokemonDetails> pokemonDetails = new List<PokemonDetails>();

            foreach (string name in pokemonNames)
            {
                PokemonDetails details = await GetPokemonDetails(name);
                pokemonDetails.Add(details);
            }

            return Ok(pokemonDetails);
        }

        [HttpGet]
        [Route("api/pokemon/{name}")]
        public async Task<IHttpActionResult> GetSpecificPokemon(string name)
        {
            PokemonDetails details = await GetPokemonDetails(name);

            return Ok(details);
        }

        private async Task<List<string>> GetRandomPokemonNames(int count)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{PokeApiBaseUrl}pokemon?limit={count}");
                string json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                List<string> pokemonNames = new List<string>();
                foreach (var item in data.results)
                {
                    pokemonNames.Add(item.name.ToString());
                }

                return pokemonNames;
            }
        }

        private async Task<PokemonDetails> GetPokemonDetails(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{PokeApiBaseUrl}pokemon/{name}");
                string json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                List<string> types = new List<string>();
                JArray typesArray = ((JArray)data.types).ToObject<JArray>();
                foreach (var typeData in typesArray)
                {
                    string typeName = typeData["type"]["name"].ToString();
                    types.Add(typeName);
                }

                PokemonDetails details = new PokemonDetails
                {
                    Name = data.name,
                    SpriteBase64 = await GetSpriteBase64(data.sprites.front_default.ToString()),
                    Number = data.order,
                    Types = types,
                    Height = data.height,
                    Weight = data.weight,
                    
                };

                JObject evolutionChainResponse = await GetEvolutionChain(data.species.url.ToString());
                List<string> evolutionNames = ParseEvolutionChain(evolutionChainResponse);
                details.Evolutions = evolutionNames;

                return details;
            }
        }

        private async Task<string> GetSpriteBase64(string spriteUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(spriteUrl);
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private async Task<JObject> GetEvolutionChain(string speciesUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(speciesUrl);
                string json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                string evolutionChainUrl = data.evolution_chain.url;
                response = await client.GetAsync(evolutionChainUrl);
                json = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject(json);

                return data;
            }
        }

        private List<string> ParseEvolutionChain(JObject evolutionChain)
        {
            List<string> evolutionNames = new List<string>();

            dynamic currentEvolution = evolutionChain["chain"];
            while (currentEvolution != null)
            {
                string name = currentEvolution["species"]["name"].ToString();
                evolutionNames.Add(name);

                var evolvesTo = currentEvolution["evolves_to"];
                if (evolvesTo != null && evolvesTo.Count > 0)
                {
                    currentEvolution = evolvesTo[0];
                }
                else
                {
                    currentEvolution = null;
                }
            }

            return evolutionNames;
        }


        public class PokemonDetails
        {
            public string Name { get; set; }
            public string SpriteBase64 { get; set; }
            public int Number { get; set; }
            public List<string> Types { get; set; }
            public double Height { get; set; }
            public double Weight { get; set; }
            public List<string> Evolutions { get; internal set; }
            
        }
    }
}
