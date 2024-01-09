using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DownloadYoutube.Interface;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Common;

namespace DownloadYoutube.Service
{
    public class FindService : IFind
    {
        public async Task<List<VideoSearchResult>> Find(string keyword)
        {
            if (keyword is not null)
            {
                var youtube = new YoutubeClient();
                return (await youtube.Search.GetVideosAsync(keyword)).ToList();
            }
            return null;


        }

    }


}
