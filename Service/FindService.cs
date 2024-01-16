using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DownloadYoutube.Interface;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Common;
using System.ComponentModel;
using YoutubeExplode.Videos.Streams;

namespace DownloadYoutube.Service
{
    public class FindService : IFind
    {
        public async Task<List<VideoSearchResult>> Find(string keyword)
        {
            if (keyword is not null)
            {
                var youtube = new YoutubeClient();
                var searchResults = new List<VideoSearchResult>();


                await foreach (var result in youtube.Search.GetVideosAsync(keyword))
                {
                    searchResults.Add(result);

                    if (searchResults.Count >= 6)
                        break;
                }

                return searchResults;
            }
            return null;


        }

    }


}
