using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace DownloadYoutube.Interface
{
    public interface IDownLoadYotube
    {
        public Task DownloadVideoAsync(YoutubeClient youtube, VideoSearchResult video);
    }
}