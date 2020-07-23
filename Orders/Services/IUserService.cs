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
        public void ForgeUserModel(UserViewModel model, IHostEnvironment hostingEnvironment);
    }
}
