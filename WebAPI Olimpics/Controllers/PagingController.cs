using ApplicationCore.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_Olimpics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagingController : ControllerBase
    {
        private readonly OlimpicsContext _context;
        private readonly ILogger<PagingController> _logger;

        public PagingController(ILogger<PagingController> logger, OlimpicsContext context)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("Events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents(int page = 1, int pageSize = 10)
        {
            var events = await _context.Events
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(events);
        }


        [HttpGet("Sports")]
        public async Task<ActionResult<IEnumerable<Sport>>> GetSports(int page = 1, int pageSize = 10)
        {
            var sports = await _context.Sports
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(sports);
        }


        [HttpGet("Person")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons(int page = 1, int pageSize = 10)
        {
            var people = await _context.People
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(people);
        }

        [HttpGet("GamesCompetitor")]
        public async Task<ActionResult<IEnumerable<GamesCompetitor>>> GetGamesCompetitors(int page = 1, int pageSize = 10)
        {
            var gamesCompetitor = await _context.GamesCompetitors
                .Include(gc => gc.Games) // Ładowanie powiązanych obiektów Games
                .Include(gc => gc.Person) // Ładowanie powiązanych obiektów Person
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(gamesCompetitor);
        }
    }
}