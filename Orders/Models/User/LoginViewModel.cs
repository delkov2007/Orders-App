﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Models.User
{
    public class LoginViewModel
    {
        public int UserID = -1;
        public CredentialsModel Credentials { get; set; }

    }
}
