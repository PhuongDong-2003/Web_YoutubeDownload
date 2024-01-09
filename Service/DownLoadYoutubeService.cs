using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Io;
using DownloadYoutube.Interface;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos.Streams;

namespace DownloadYoutube.Service
{
    public class DownLoadYoutubeService : IDownLoadYotube
    {
        public async Task DownloadVideoAsync(YoutubeClient youtube, VideoSearchResult video)
        {
            var videoId = video.Id;
            var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoId);

            var muxedStream = streamInfoSet.GetMuxedStreams().GetWithHighestVideoQuality();

            await youtube.Videos.Streams.DownloadAsync(muxedStream, $"{video.Title}.{muxedStream.Container}");
            var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{video.Title}.{muxedStream.Container}");
            
            Console.WriteLine($"Video đã được tải về: {outputFilePath}");
        }
    }
}