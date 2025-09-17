using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AuthTest.ViewModels;
using AuthTest.Models;

namespace AuthTest.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(VmUser user)
        {
            if (!string.IsNullOrEmpty(user.userName) && string.IsNullOrEmpty(user.password))
            {
                return RedirectToAction("Login");
            }

            //Check the user name and password  
            //Here can be implemented checking logic from the database  
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            //if (user.userName == "Ashraf" && user.password == "password")
            //{
            //    //Create the identity for the user  
            //    identity = new ClaimsIdentity(new[] {
            //        new Claim(ClaimTypes.Name, user.userName),
            //        new Claim(ClaimTypes.Role, "Admin")
            //    }, CookieAuthenticationDefaults.AuthenticationScheme);

            //    isAuthenticated = true;
            //}

            //if (user.userName == "Murshid" && user.password == "password")
            //{
            //    //Create the identity for the user  
            //    identity = new ClaimsIdentity(new[] {
            //        new Claim(ClaimTypes.Name, user.userName),
            //        new Claim(ClaimTypes.Role, "User")
            //    }, CookieAuthenticationDefaults.AuthenticationScheme);

            //    isAuthenticated = true;
            //}

            AuthDbContext _ctx = new AuthDbContext();
            var oUser = _ctx.User.Where(u => u.Username == user.userName && u.Password == user.password).FirstOrDefault();
            if (oUser != null)
            {
                //Create the identity for the user  
                identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, user.userName),
                        new Claim(ClaimTypes.Role, oUser.Role)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}