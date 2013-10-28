using System;

namespace Forum.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Reciever { get; set; }
    }
}
