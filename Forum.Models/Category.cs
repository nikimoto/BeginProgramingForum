using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public Category()
        {
            this.Posts = new HashSet<Post>();
        }
    }
}
