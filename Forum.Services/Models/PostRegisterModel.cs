using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Services.Models
{
    public class PostRegisterModel
    {
        public int CurrentCategoryId { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}