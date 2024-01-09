using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Search;

namespace DownloadYoutube.Interface
{
    public interface IUserChooses
    {
        
        public  Task<VideoSearchResult> LoadChooses(List<VideoSearchResult> searchResults, int selectedVideoIndex);
        
    }
}