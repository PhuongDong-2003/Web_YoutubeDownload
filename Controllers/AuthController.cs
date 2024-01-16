using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoutubeDownload.Authentication;
using YoutubeDownload.Service;


namespace YoutubeDownload.Controllers
{
    public class AuthController : Controller
    {
        private readonly LoginService _loginService;
        private readonly ILogger<AuthController> logger;
  

        public AuthController( ILogger<AuthController> logger, LoginService loginService)
        {
            _loginService = loginService;
            this.logger = logger;
           
        }


  

        [HttpPost]
        public async Task<IActionResult> Login(Users users)
        {

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                var validAccount = _loginService.GetUser(users);
                if (validAccount != null)
                {

                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, validAccount.Username),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim("TypeCustomer", "Vip")
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                    ViewData["ValidateMessage"] = "Đăng Nhập thành công.";


                    return View("Login");
                }
                else
                {
                    ViewData["ValidateMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng.";

                }

            }

            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }

        public IActionResult Index()
        {
            return View("Auth");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}