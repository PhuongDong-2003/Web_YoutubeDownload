using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace DownloadYoutube.Interface
{
    public interface IDownLoadYotube
    {
         public Task<FileResult> Download(string youtubeLink);

         public Task<FileResult> DownloadAudio(string youtubeLink);
    }
}