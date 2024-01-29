using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AngleSharp.Io;
using DownloadYoutube.Interface;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos.Streams;

namespace DownloadYoutube.Service
{
    public class DownLoadYoutubeService : IDownLoadYotube
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DownLoadYoutubeService> _logger;

        public DownLoadYoutubeService(ILogger<DownLoadYoutubeService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<FileResult> Download(string youtubeLink)
        {

            try
            {
                var youtube = new YoutubeClient();
                var videoInfo = await youtube.Videos.GetAsync(youtubeLink);
                var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoInfo.Id);
                var streamInfo = streamInfoSet.GetMuxedStreams().GetWithHighestVideoQuality();
                var size = streamInfo.Size;

                if (streamInfo != null)
                {

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var ms = new MemoryStream();
                    await youtube.Videos.Streams.CopyToAsync(streamInfo, ms);
                    ms.Position = 0;
                    var fileType = streamInfo.Container;

                    stopwatch.Stop();
                    var time = stopwatch.Elapsed;
                    _logger.LogInformation("Video downloaded - VideoName: {VideoName}, ElapsedTime: {ElapsedTime}s, File size: {Size}",
                   videoInfo.Title, time, size);


                    var cd = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"{videoInfo.Title}.{fileType}"
                    };

                    return new FileContentResult(ms.ToArray(), $"video/{fileType}")
                    {
                        FileDownloadName = $"{videoInfo.Title}.{fileType}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading video: {ex.Message}");
            }

            return null;
        }

        public async Task<FileResult> DownloadAudio(string youtubeLink)
        {
            try
            {
                var youtube = new YoutubeClient();
                var videoInfo = await youtube.Videos.GetAsync(youtubeLink);
                var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoInfo.Id);
                var streamInfo = streamInfoSet.GetAudioOnlyStreams().GetWithHighestBitrate();
                var size = streamInfo.Size;
                if (streamInfo != null)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var ms = new MemoryStream();
                    await youtube.Videos.Streams.CopyToAsync(streamInfo, ms);
                    ms.Position = 0;

                    var fileType = streamInfo.Container;
                    stopwatch.Stop();
                    var time = stopwatch.Elapsed;
                    _logger.LogInformation("Video downloaded - VideoName: {VideoName}, ElapsedTime: {ElapsedTime}s, File size: {Size}",
                   videoInfo.Title, time, size);
                    var cd = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"{videoInfo.Title}.{fileType}"
                    };

                    return new FileContentResult(ms.ToArray(), $"video/{fileType}")
                    {
                        FileDownloadName = $"{videoInfo.Title}.{fileType}"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading video: {ex.Message}");
            }

            return null;
        }
    }
}