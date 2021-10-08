using Borealis.Data;
using Borealis.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Borealis.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return Redirect("/TopTime/List");
        }


        [HttpGet("/denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Secured()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UnproccessedTimes()
        {
            return Redirect("/TopTime/List");
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            foreach(UserModel User in _db.Users)
            {
                if(User.EMailAddress == user.EMailAddress)
                {
                    TempData["Error"] = @"Greška! E-mail adresa se koristi!";
                    return View(user);
                }
            }
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", user.EMailAddress));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.EMailAddress));
                claims.Add(new Claim(ClaimTypes.Name, "User"));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

                _db.Users.Add(user);
                _db.SaveChanges();

                return Redirect("/TopTime/List");
            }

            TempData["Error"] = @"Greška! Pogrešan format e-mail adrese!";
            return View(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username, string password, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (username == "admin" && password == "0000")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Name, "Admin"));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(returnUrl);
            }

            foreach(UserModel user in _db.Users)
            {
                if(user.EMailAddress == username && user.Password == password)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", username));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                    claims.Add(new Claim(ClaimTypes.Name, "User"));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return Redirect("/TopTime/List");
                }
            }

            TempData["Error"] = "Greška! Pogrešna e-mail adresa ili lozinka!";
            return View("login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
