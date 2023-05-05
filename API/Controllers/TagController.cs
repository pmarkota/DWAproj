using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {

        private DwaprojectContext _db;
        public TagController(DwaprojectContext db)
        {
            _db = db;
        }
        // GET: api/<TagController>
        [HttpGet]
        public IActionResult Get()
        {
            var tags = _db.Tags.Include(g => g.Videos).ToList();
            return Ok(tags);
        }

        // GET api/<TagController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tag = _db.Tags
                .Include(v => v.Videos)
                .SingleOrDefault(x => x.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // POST api/<TagController>
        [HttpPost]
        public IActionResult Post([FromBody] Tag tag)
        {
            var dbTag = new Tag
            {
                Name = tag.Name
            };
            _db.Tags.Add(dbTag);
            _db.SaveChanges();
            return Ok(dbTag);
        }

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,  string name)
        {
            var dbTag = _db.Tags.Include(v => v.Videos).FirstOrDefault(t => t.Id == id);

            if (dbTag == null)
            {
                return NotFound();
            }

            dbTag.Name = name;
            _db.SaveChanges();

            return Ok(dbTag);
        }

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tag = _db.Tags.FirstOrDefault(t => t.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            // Remove the genre from all associated videos
            foreach (var video in tag.Videos)
            {
                video.Tags.Remove(tag);
            }

            _db.Tags.Remove(tag);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
