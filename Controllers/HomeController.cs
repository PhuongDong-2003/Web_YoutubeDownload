using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using DownloadYoutube.Service;
using Microsoft.AspNetCore.Mvc;
using YoutubeDownload.Models;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownload.Controllers;

public class HomeController : Controller
{
    private readonly FindService _findService;
    private readonly DownLoadYoutubeService _downLoadYoutubeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, FindService findService, DownLoadYoutubeService downLoadYoutubeService)
    {
        _findService = findService;
        _downLoadYoutubeService = downLoadYoutubeService;
        _logger = logger;
    }

    public List<VideoSearchResult> searchResults = new List<VideoSearchResult>();

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Find(string keyword)
    {
        searchResults = await _findService.Find(keyword);
        ViewBag.listresult = searchResults;
        return View("Index");
    }

    public async Task<FileResult> Download(string link)
    {

        if (link is not null)
        {

            return await _downLoadYoutubeService.Download(link);
        }
        return null;

    }

    [HttpPost]
    public async Task<PartialViewResult> FindJS(string keyword)
    {
        if (keyword is not null)
        {
            var results = await _findService.Find(keyword);
            ViewBag.listresult = results;
            return PartialView("_SearchResultsPartial");
        }

        return null;
    }

    [HttpGet]
    public async Task<ActionResult> DownloadJs([FromQuery] string link)
    {
        var role = User.Claims.Where(c => c.Type == "https://my-app.example.com/roles")
                            .Select(c => c.Value)
                            .ToList();

        if (link is not null && role is not null)
        {
            if (role.Contains("customer"))
            {
                try
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var results = await _downLoadYoutubeService.Download(link);


                    stopwatch.Stop();
                    var elapsedTime = stopwatch.Elapsed;

                


                    return results;
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Error downloading video - User: {UserName}, VideoName: {VideoName}", User.Identity.Name, link);
                    throw;
                }
            }
            else
            {
                return BadRequest("You don't have permission to download");
            }


        }

        return RedirectToAction("Error");
    }

    [HttpGet]
    public async Task<ActionResult> DownloadAudio([FromQuery] string link)
    {

        if (link is not null)
        {
            return await _downLoadYoutubeService.DownloadAudio(link);
        }

        return RedirectToAction("Error");
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}



