﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Services.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        //public virtual ICollection<PostModel> Posts { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
    }
}