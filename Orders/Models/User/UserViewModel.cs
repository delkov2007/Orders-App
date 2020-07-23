using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Orders.Models.User
{
    public class UserViewModel
    {
        public string ID { get; set; }

        public CredentialsModel Credentials = new CredentialsModel() { Username = "", Password = ""};
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }
        [XmlIgnore]
        public IFormFile UserPicture { set; get; }
        public string UserPictureFilePath { get; set; }
        public bool IsLogged { get; set; }
        public bool IsAdmin { get; set; }
        public List<OrderModel> OrdersList { get; set; }
        public List<ItemModel> ItemsList { get; set; }

    }
}
