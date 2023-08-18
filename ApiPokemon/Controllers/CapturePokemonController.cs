using System.Web.Http;
using System.Data.Entity;
using ApiPokemon.Data;
using System.Linq;

public class CapturePokemonController : ApiController
{
    private PokemonDbContext _dbContext;

    public CapturePokemonController()
    {
        _dbContext = new PokemonDbContext();
    }

    [HttpPost]
    [Route("api/capturepokemon")]
    public IHttpActionResult ReportPokemonCapture([FromBody] CapturedPokemonModel capturedPokemon)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        _dbContext.CapturedPokemon.Add(capturedPokemon);
        _dbContext.SaveChanges();

        return Ok();
    }

    [HttpGet]
    [Route("api/capturedpokemon/list")]
    public IHttpActionResult ListCapturedPokemons()
    {
        var capturedPokemons = _dbContext.CapturedPokemon.ToList();
        return Ok(capturedPokemons);
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
