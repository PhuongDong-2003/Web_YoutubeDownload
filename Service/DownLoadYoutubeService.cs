using System;
using System.Collections.Generic;
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
        public async Task<FileResult> Download(string youtubeLink)
        {
            try
            {
                var youtube = new YoutubeClient();
                var videoInfo = await youtube.Videos.GetAsync(youtubeLink);
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