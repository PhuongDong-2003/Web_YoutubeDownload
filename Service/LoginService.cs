using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeDownload.Authentication;

namespace YoutubeDownload.Service
{
    public class LoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      public UserRole GetUser(Users users)
        {
            return _configuration.GetSection("Users")
                                 .Get<List<UserRole>>()
                                 .FirstOrDefault(u => u.Username == users.Username && u.Password == users.Password);

        }
    }
}