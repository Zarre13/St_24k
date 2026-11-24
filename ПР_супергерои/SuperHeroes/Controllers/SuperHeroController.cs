using Golovkin_Maltseva_SUPERHEROES.Data;
using Golovkin_Maltseva_SUPERHEROES.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Golovkin_Maltseva_SUPERHEROES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Superhero>>> GetAllHeroes()
        {
            var heroes = await _context.Superheroes.ToListAsync();
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Superhero>>> GetHero(int id)
        {
            var hero = await _context.Superheroes.FindAsync(id);
            if (hero == null)
                return NotFound("Герой не найден");

            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<Superhero>> AddHero(Superhero hero)
        {
            _context.Superheroes.Add(hero);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHero), new { id = hero.id }, hero);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Superhero>> UpdateHero(int id, Superhero hero)
        {
            var dbhero = await _context.Superheroes.FindAsync(id);
            if (dbhero == null)
                return NotFound("Герой не найден");

            if (hero.id != 0 && hero.id != id)
                return BadRequest("ID в маршруте и теле не совпадают");

            dbhero.Name = hero.Name;
            dbhero.FirstName = hero.FirstName;
            dbhero.LastName = hero.LastName;
            dbhero.Place = hero.Place;

            await _context.SaveChangesAsync();
            return Ok(dbhero);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHero(int id)
        {
            var dbhero = await _context.Superheroes.FindAsync(id);
            if (dbhero == null)
                return NotFound("Герой не найден");
            _context.Superheroes.Remove(dbhero);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
