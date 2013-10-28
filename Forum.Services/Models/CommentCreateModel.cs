using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    public class CommentCreateModel
    {
        public string Content { get; set; }

        public int PostId { get; set; }
    }
}