using Microsoft.Extensions.Hosting;
using Orders.Helpers;
using Orders.Models;
using Orders.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Orders.Services
{
    public class UserService : IUserService
    {
        private string ForgeUniqueUserImageFileName (string filename, UserViewModel model)
        {
            filename = Path.GetFileName(filename);
            string uniqueFileName = Path.GetFileNameWithoutExtension(filename)
                + "_"
                + model.ID
                + "_"
                + model.FirstName
                + "_"
                + model.LastName
                + Path.GetExtension(filename);
            return uniqueFileName;
        }
        public void ForgeUserModel(UserViewModel model, IHostEnvironment hostingEnvironment)
        {
            List<UserViewModel> listOfUsers = new List<UserViewModel>();

            string filePath = "";

            if (model.UserPicture != null)
            {
                var uniqueFileName = ForgeUniqueUserImageFileName(model.UserPicture.FileName, model);
                var uploads = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot/uploads");
                filePath = Path.Combine(uploads, uniqueFileName);
                var fs = new FileStream(filePath, FileMode.Open);
                model.UserPicture.CopyTo(fs);
                fs.Close();
            }
            UserViewModel newUser = new UserViewModel()
            {
                ID = Guid.NewGuid().ToString(),
                Credentials = { Username = model.Credentials.Username, Password = model.Credentials.Password },
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Email = model.Email,
                UserPictureFilePath = filePath,
                IsAdmin = false,
                IsLogged = false,
                OrdersList = new List<OrderModel>(),
                ItemsList = Helpers.XmlHelper.ReadFromXml<ItemModel>(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\autogentestbase.xml")
            };

            listOfUsers.Append(newUser);
            XmlHelper.WriteToXml(listOfUsers, @"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\userdatabase.xml");

        }
    }
}
