using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Services.Models
{
    public class CommentEditModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string MyProperty { get; set; }
    }
}