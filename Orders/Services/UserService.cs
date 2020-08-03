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
        private string ForgeUniqueUserImageFileName (string filename, UserViewModel user)
        {
            
            filename = Path.GetFullPath(filename);
            return user.ID
                + "_"
                + user.FirstName
                + "_"
                + user.LastName
                + Path.GetExtension(filename);
            
        }
        public void PushUserToXML(UserViewModel model, IHostEnvironment hostingEnvironment)
        {
            List<UserViewModel> listOfUsers = Helpers.XmlHelper.ReadFromXml<UserViewModel>(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\userdatabase.xml")
                ?? new List<UserViewModel>();

            string filePath = "";
            UserViewModel newUser = ForgeUser(model);

            if (model.UserPicture != null)
            {
                var uniqueFileName = ForgeUniqueUserImageFileName(model.UserPicture.FileName, newUser);
                var uploads = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot/uploads");
                filePath = Path.Combine(uploads, uniqueFileName);
                var fs = new FileStream(filePath, FileMode.CreateNew);
                model.UserPicture.CopyTo(fs);
                fs.Close();
                newUser.UserPictureFilePath = "/uploads/" + uniqueFileName;
            }
            

            listOfUsers.Add(newUser);
            XmlHelper.WriteToXml(listOfUsers, @"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\userdatabase.xml");

        }
        public UserViewModel ForgeUser(UserViewModel model)
        {
            return new UserViewModel()
            {
                ID = Guid.NewGuid().ToString().Substring(0, 6),
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Email = model.Email,
                IsAdmin = false,
                IsLogged = false,
                OrdersList = new List<OrderModel>(),
                ItemsList = Helpers.XmlHelper.ReadFromXml<ItemModel>(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\autogentestbase.xml")
            };
        }
        public string GetUserID(LoginViewModel login)
        {
            List<string> resultUserID = null; 
            List<UserViewModel> users = 
                Helpers.XmlHelper.ReadFromXml<UserViewModel>(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\userdatabase.xml")
                ?? new List<UserViewModel>();

            resultUserID = users.Where(x => x.Username.Equals(login.Username) && x.Password.Equals(login.Password))
                .Select(x => x.ID).ToList();

            return resultUserID.FirstOrDefault();
        }
        public UserViewModel PullUserFromXML(string userId)
        {
            List<UserViewModel> users =
                Helpers.XmlHelper.ReadFromXml<UserViewModel>(@"C:\Users\HP\source\repos\Orders\Orders\wwwroot\xml\userdatabase.xml")
                ?? new List<UserViewModel>();

            users = users.Where(x => x.ID.Equals(userId))
                .Select(x => x).ToList();

            return users.FirstOrDefault();

        }
    }
}
