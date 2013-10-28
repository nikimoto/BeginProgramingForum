using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AuthCode { get; set; }
        public string SessionKey { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAdmin { get; set; }


        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> RecievedMessages { get; set; }
        public bool IsBanned { get; set; }

        public User()
        {
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.SentMessages = new HashSet<Message>();
            this.RecievedMessages = new HashSet<Message>();
        }

        
    }
}
