using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Orders.Models.User;
using Orders.Services;

namespace Orders.Controllers
{
    public class UserController : Controller
    {
        private readonly IHostEnvironment hostingEnvironment;
        private readonly IUserService _userService;
        public UserController(IHostEnvironment environment, IUserService userService)
        {
            hostingEnvironment = environment;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            UserViewModel newUser = new UserViewModel();
            return View(newUser);
        }
        [HttpPost]
        public IActionResult CreateUser(UserViewModel model)
        {
            _userService.PushUserToXML(model, hostingEnvironment);
            

            return RedirectToAction("LogIn", "User");
        }

        public IActionResult Index(string userId)
        {
            UserViewModel loggedUser = _userService.PullUserFromXML(userId);

            return View(loggedUser);
        }
        [HttpPost]
        public IActionResult Index(UserViewModel model)
        {
            return View(model);
        }
        [HttpGet]
        public IActionResult LogIn(int statusOfLogging=-1)
        {
            LoginViewModel login = new LoginViewModel
            {
                StatusOfLogging = statusOfLogging
            };

            return View(login);
        }
        [HttpPost]
        public IActionResult LogIn(LoginViewModel model)
        {
            string userId = _userService.GetUserID(model);
            
            if (userId == null)
            {
                model.StatusOfLogging = 0;
                return Redirect("/user/LogIn?statusoflogging="+model.StatusOfLogging);
            }

            return Redirect("/user/index?userId=" + userId);
        }
    }
}
