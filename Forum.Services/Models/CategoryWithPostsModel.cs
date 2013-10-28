using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Services.Models
{
    public class CategoryWithPostsModel
    {
        public IEnumerable<PostModel> Posts { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}