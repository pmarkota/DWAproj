using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {

        private DwaprojectContext _db;
        public GenreController(DwaprojectContext db)
        {
            _db = db;
        }
        // GET: api/<GenreController>
        [HttpGet]
        public IActionResult Get()
        {
            var genres = _db.Genres.Include(g => g.Videos).ToList();
            return Ok(genres);
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var genre = _db.Genres
                .Include(v => v.Videos)
                .SingleOrDefault(x => x.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        // POST api/<GenreController>
        [HttpPost]
        public IActionResult Post([FromBody] Genre genre)
        {
            var dbGenre = new Genre
            {
                Name = genre.Name,
                Description = genre.Description
            };
            _db.Genres.Add(dbGenre);
            _db.SaveChanges();
            return Ok(dbGenre);
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, string description)
        {
            var dbGenre = _db.Genres.Include(v => v.Videos).FirstOrDefault(g => g.Id == id);

            if (dbGenre == null)
            {
                return NotFound();
            }

            dbGenre.Description = description;
            _db.SaveChanges();

            return Ok(dbGenre);
        }



        // DELETE api/<GenreController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var genre = _db.Genres.FirstOrDefault(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            // Remove the genre from all associated videos
            foreach (var video in genre.Videos)
            {
                video.Genres.Remove(genre);
            }

            _db.Genres.Remove(genre);
            _db.SaveChanges();

            return NoContent();
        }

    }
}
