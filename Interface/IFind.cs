using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace DownloadYoutube.Interface
{
    public interface IFind
    {
           public Task<List<VideoSearchResult>> Find(string keyword);
    }
}