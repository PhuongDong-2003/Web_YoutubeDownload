using System.Diagnostics;
using System.Text.Json;
using DownloadYoutube.Service;
using Microsoft.AspNetCore.Mvc;
using YoutubeDownload.Models;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace YoutubeDownload.Controllers;

public class HomeController : Controller
{
    private readonly FindService _findService;
    private readonly DownLoadYoutubeService _downLoadYoutubeService;
    private readonly UserChoosesService _userChoosesService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, FindService findService, DownLoadYoutubeService downLoadYoutubeService, UserChoosesService userChoosesService)
    {
        _findService = findService;
        _downLoadYoutubeService = downLoadYoutubeService;
        _userChoosesService = userChoosesService;
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

    public async Task<IActionResult> Download(int selectedVideoIndex)
    {

        var youtube = new YoutubeClient();

        if (selectedVideoIndex >= 0)
        {
            var video = await _userChoosesService.LoadChooses(searchResults, selectedVideoIndex);

            if (video is not null)
            {
                await _downLoadYoutubeService.DownloadVideoAsync(youtube, video);
                return View("Index");
            }

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
