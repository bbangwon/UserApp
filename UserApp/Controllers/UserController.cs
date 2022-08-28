using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserApp.Common;
using UserApp.Models;

namespace UserApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ILoginFailedManager loginFailedManager;

        public UserController(IUserRepository userRepository, ILoginFailedManager loginFailedManager)
        {
            this.userRepository = userRepository;
            this.loginFailedManager = loginFailedManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if(ModelState.IsValid)
            {
                this.userRepository.AddUser(
                    model.UserId!, 
                    CryptoEngine.EncryptPassword(model.Password!));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]    //인증되지 않은 사용자도 접근 가능
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserViewModel model, string? returnUrl = null)
        {
            if(ModelState.IsValid)
            {
                if(this.loginFailedManager.IsLoginFailed(model.UserId))
                {
                    ViewBag.LoginFailed = true;
                    ViewBag.LoginLock = true;
                    return View(model);
                }
                
                if(userRepository.IsCorrectUser(
                    model.UserId!,
                    CryptoEngine.EncryptPassword(model.Password!)))
                {
                    //인증 부여
                    var claims = new List<Claim>()
                    {
                        new Claim("UserId", model.UserId!)
                    };

                    var ci = new ClaimsIdentity(claims, CryptoEngine.EncryptPassword(model.Password));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
                    return LocalRedirect("/User/Index");
                }
                else
                {
                    ViewBag.LoginFailed = true;
                    this.loginFailedManager.UpdateLoginCount(model.UserId);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if(returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult UserInfo()
        {
            return View();
        }


    }
}
