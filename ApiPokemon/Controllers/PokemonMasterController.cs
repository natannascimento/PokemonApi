using System.Web.Http;
using System.Data.Entity;
using ApiPokemon.Data;
using ApiPokemon.Models;

public class PokemonMasterController : ApiController
{
    private PokemonDbContext _dbContext;

    public PokemonMasterController()
    {
        _dbContext = new PokemonDbContext();
    }

    [HttpPost]
    [Route("api/pokemonMaster")]
    public IHttpActionResult RegisterPokemonMaster([FromBody] PokemonMasterModel mestrePokemon)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _dbContext.PokemonMaster.Add(mestrePokemon);
        _dbContext.SaveChanges();

        return Ok();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }

        base.Dispose(disposing);
    }
}
