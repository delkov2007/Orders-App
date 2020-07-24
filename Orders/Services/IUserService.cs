using Microsoft.Extensions.Hosting;
using Orders.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Services
{
    public interface IUserService
    {
        public void PushUserToXML(UserViewModel model, IHostEnvironment hostingEnvironment);
        public string GetUserID(LoginViewModel login);
        public UserViewModel ForgeUser(UserViewModel model);
        public UserViewModel PullUserFromXML(string userId);

    }
}
