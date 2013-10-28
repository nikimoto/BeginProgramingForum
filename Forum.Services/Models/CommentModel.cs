using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class CommentModel
    {
        [DataMember(Name="author")]
        public string Author { get; set; }

        [DataMember(Name="creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name="content")]
        public string Content { get; set; }
    }
}