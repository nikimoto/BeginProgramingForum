using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Services.Models
{
    public class UserEditModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public bool IsBanned { get; set; }
    }
}