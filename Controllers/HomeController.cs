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

    [HttpPost]
    public async Task<JsonResult> DownloadJs(string link)
    {
        if (link is not null)
        {
            var fileResult = await _downLoadYoutubeService.Download(link);
            return Json(new { success = fileResult != null, fileResult });
        }
        return Json(new { success = false });
    }

    public async Task<IActionResult> Download2(string youtubeLink)
    {
        string link = "https://www.youtube.com/watch?v=e2Xx7WcvEns&list=RDGMEMQ1dJ7wXfLlqCjwV0xfSNbAVMR_zMKRuQbM0&index=5";
        try
        {
            var youtube = new YoutubeClient();
            var videoInfo = await youtube.Videos.GetAsync(link);
            var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoInfo.Id);
            var streamInfo = streamInfoSet.GetMuxedStreams().GetWithHighestVideoQuality();

            if (streamInfo != null)
            {
                var ms = new MemoryStream();

                await youtube.Videos.Streams.CopyToAsync(streamInfo, ms);
                ms.Position = 0;

                var fileType = streamInfo.Container;

                var cd = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = $"{videoInfo.Title}.{fileType}"
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());

                return File(ms, $"video/{fileType}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading video: {ex.Message}");
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
