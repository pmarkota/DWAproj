using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private DwaprojectContext _db;
        public VideoController(DwaprojectContext db)
        {
            _db = db;
        }
        // GET: api/<VideoController>
        //[HttpGet]
        //public IActionResult Get(string videoNameFilter = null, string genreNameFilter=null, int? page = 1, int pageSize = 10, string orderBy = "id")
        //{
        //    var query = _db.Videos
        //        .Where(v => videoNameFilter == null || v.Name.Contains(videoNameFilter))
        //        .Where(v => genreNameFilter == null || v.Genres.Any(g => g.Name.Contains(genreNameFilter)))
        //        .Select(v => new
        //        {
        //            v.Id,
        //            v.Name,
        //            v.Description,
        //            v.Image,
        //            v.TotalTime,
        //            v.StreamingUrl,
        //            Genres = v.Genres.Select(g => new { g.Id, g.Name, g.Description }),
        //            Tags = v.Tags.Select(t => new { t.Id, t.Name })
        //        });


        //    switch (orderBy.ToLower())
        //    {
        //        case "name":
        //            query = query.OrderBy(v => v.Name);
        //            break;
        //        case "totaltime":
        //            query = query.OrderBy(v => v.TotalTime);
        //            break;
        //        default:
        //            query = query.OrderBy(v => v.Id);
        //            break;
        //    }

        //    var count = query.Count();
        //    var totalPages = (int)Math.Ceiling((double)count / pageSize);

        //    if (page.HasValue)
        //    {
        //        query = query.Skip(pageSize * (page.Value - 1));
        //    }

        //    query = query.Take(pageSize);

        //    var result = new
        //    {
        //        Count = count,
        //        TotalPages = totalPages,
        //        Videos = query.ToList()
        //    };

        //    return Ok(result);
        //}

        // GET api/<VideoController>/5

        [HttpGet]
        public IActionResult Get(string videoNameFilter = null, string genreNameFilter = null, int? page = 1, int pageSize = 10, string orderBy = "id")
        {
            try
            {
                var query = _db.Videos
                    .Where(v => videoNameFilter == null || v.Name.Contains(videoNameFilter))
                    .Where(v => genreNameFilter == null || v.Genres.Any(g => g.Name.Contains(genreNameFilter)))
                    .Select(v => new
                    {
                        v.Id,
                        v.Name,
                        v.Description,
                        v.Image,
                        v.TotalTime,
                        v.StreamingUrl,
                        Genres = v.Genres.Select(g => new { g.Id, g.Name, g.Description }),
                        Tags = v.Tags.Select(t => new { t.Id, t.Name })
                    });


                switch (orderBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(v => v.Name);
                        break;
                    case "totaltime":
                        query = query.OrderBy(v => v.TotalTime);
                        break;
                    default:
                        query = query.OrderBy(v => v.Id);
                        break;
                }

                var count = query.Count();
                var totalPages = (int)Math.Ceiling((double)count / pageSize);

                if (page.HasValue)
                {
                    query = query.Skip(pageSize * (page.Value - 1));
                }

                query = query.Take(pageSize);

                var result = new
                {
                    Count = count,
                    TotalPages = totalPages,
                    Videos = query.ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var video = _db.Videos
                .Include(v => v.Genres)
                .Include(v => v.Tags)
                .SingleOrDefault(x => x.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }


        // POST api/<VideoController>
        //{
        //    "id": 5,
        //    "name": "Star Wards",
        //    "description": "boom",
        //    "image": "string",
        //    "totalTime": 333,
        //    "streamingUrl": "string",
        //    "genres": [
        //    {
        //        "id": 1,
        //        "name": "str22ing",
        //        "video": [
        //        "string"
        //            ]
        //    }
        //    ],
        //    "tags": [
        //    {
        //        "id": 1,
        //        "name": "string",
        //        "video": [
        //        "string"
        //            ]
        //    }
        //    ]
        //}
        [HttpPost]
        public IActionResult Post([FromBody] Video video)
        {
            var dbVideo = new Video
            {
                Name = video.Name,
                Description = video.Description,
                Image = video.Image,
                TotalTime = video.TotalTime,
                StreamingUrl = video.StreamingUrl
            };

            // Add genres to the video
            foreach (var genreId in video.Genres.Select(g => g.Id))
            {
                var genre = _db.Genres.Find(genreId);
                if (genre != null)
                {
                    dbVideo.Genres.Add(genre);
                }
            }

            // Add tags to the video
            foreach (var tagId in video.Tags.Select(t => t.Id))
            {
                var tag = _db.Tags.Find(tagId);
                if (tag != null)
                {
                    dbVideo.Tags.Add(tag);
                }
            }

            _db.Videos.Add(dbVideo);
            _db.SaveChanges();

            return Ok();
        }


        // PUT api/<VideoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Video video)
        {
            var dbVideo = _db.Videos
                .Include(v => v.Genres)
                .Include(v => v.Tags)
                .FirstOrDefault(v => v.Id == id);

            if (dbVideo != null)
            {
                // Update video properties
                dbVideo.Name = video.Name;
                dbVideo.Description = video.Description;
                dbVideo.Image = video.Image;
                dbVideo.TotalTime = video.TotalTime;
                dbVideo.StreamingUrl = video.StreamingUrl;

                // Update genres
                dbVideo.Genres.Clear();
                foreach (var genre in video.Genres)
                {
                    var dbGenre = _db.Genres.Find(genre.Id);
                    if (dbGenre != null && dbVideo.Genres.All(g => g.Id != dbGenre.Id))
                    {
                        dbVideo.Genres.Add(dbGenre);
                    }
                }

                // Update tags
                dbVideo.Tags.Clear();
                foreach (var tag in video.Tags)
                {
                    var dbTag = _db.Tags.Find(tag.Id);
                    if (dbTag != null && dbVideo.Tags.All(t => t.Id != dbTag.Id))
                    {
                        dbVideo.Tags.Add(dbTag);
                    }
                }

                _db.Entry(dbVideo).State = EntityState.Modified;
                _db.SaveChanges();

                return Ok(dbVideo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/genres")]
        public IActionResult PutGenres(int id, [FromBody] List<int> genreIds)
        {
            var video = _db.Videos
                .Include(v => v.Genres)
                .FirstOrDefault(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            // Clear existing genres
            video.Genres.Clear();

            // Add new genres
            foreach (var genreId in genreIds)
            {
                var genre = _db.Genres.Find(genreId);
                if (genre != null)
                {
                    video.Genres.Add(genre);
                }
            }

            _db.SaveChanges();

            return Ok(video);
        }



        // DELETE api/<VideoController>/5



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var video = _db.Videos.Include(v => v.Tags)
                .Include(v => v.Genres)
                .SingleOrDefault(v => v.Id == id);

            if (video == null)
            {
                return NotFound();
            }

            _db.Tags.RemoveRange(video.Tags);
            _db.Genres.RemoveRange(video.Genres);
            _db.Videos.Remove(video);
            _db.SaveChanges();

            return NoContent();
        }

    }
}
