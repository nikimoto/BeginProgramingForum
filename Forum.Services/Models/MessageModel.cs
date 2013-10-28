using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class MessageModel
    {
        [DataMember(Name="content")]
        public string Content { get; set; }

        [DataMember(Name="senderId")]
        public int SenderId { get; set; }

        [DataMember(Name="receiverId")]
        public int ReceiverId { get; set; }

        [DataMember(Name="creationDate")]
        public DateTime CreationDate { get; set; }
    }
}