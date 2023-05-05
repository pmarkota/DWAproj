using System.Data;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class VideoListResponse
    {
        public int Count { get; set; }
        public int TotalPages{ get; set; }
        public List<Video> Videos { get; set; }
    }

    public class VideosController : Controller
    {

        
        private readonly HttpClient _client;

        public VideosController(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://localhost:7062/api/");
        }

        // GET: VideoController
        public async Task<IActionResult> Index(string videoNameFilter = "a", string genreNameFilter = "a", int? page = 1, int pageSize = 10, string orderBy = "id")
        {
            HttpResponseMessage response = await _client.GetAsync($"videos?videoNameFilter={videoNameFilter}&genreNameFilter={genreNameFilter}&page={page}&pageSize={pageSize}&orderBy={orderBy}");



            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("api call successful");

                // Deserialize the JSON response into a Video object
                var videos = JsonConvert.DeserializeObject<VideoListResponse>(responseBody);
                Console.WriteLine(videos.Count);
                videos.TotalPages = (int)Math.Ceiling((double)videos.Count / pageSize);
                Console.WriteLine(videos.TotalPages);
                // Return the Video object
                return View(videos);
            }
            else
            {
                Console.WriteLine("Couldn't call the API");
                return NotFound();
            }
        }




        // GET: VideoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VideoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VideoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VideoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VideoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
