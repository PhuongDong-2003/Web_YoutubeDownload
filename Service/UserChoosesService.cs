using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DownloadYoutube.Interface;
using YoutubeExplode.Search;

namespace DownloadYoutube.Service
{
    public class UserChoosesService : IUserChooses
    {
        public async Task<VideoSearchResult> LoadChooses(List<VideoSearchResult> searchResults, int selectedVideoIndex )
        {
           
                if ( selectedVideoIndex >= 0 && selectedVideoIndex <= searchResults.Count)
                {
                     return searchResults[selectedVideoIndex];
                }
                return null;
     
        }
    }
}