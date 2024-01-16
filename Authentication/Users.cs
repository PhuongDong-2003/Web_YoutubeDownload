using System.ComponentModel.DataAnnotations;
namespace YoutubeDownload.Authentication
{
    public class Users
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

      

    }
}
