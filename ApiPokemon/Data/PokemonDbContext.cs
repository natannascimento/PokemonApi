using ApiPokemon.Models;
using System.Data.Entity;

namespace ApiPokemon.Data
{
    public class PokemonDbContext : DbContext
    {
        public DbSet<PokemonMasterModel> PokemonMaster { get; set; }
        public DbSet<CapturedPokemonModel> CapturedPokemon { get; set; }

    }
}