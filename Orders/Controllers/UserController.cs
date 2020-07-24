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
            _userService.ForgeUserModel(model, hostingEnvironment);
            

            return RedirectToAction("LogIn", "User");
        }

       




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            LoginViewModel login = new LoginViewModel
            {
                UserID = -1,
                
            };

            return View(login);
        }
    }
}
