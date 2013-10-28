using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class PostModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "categoryId")]
        public int CategoryId { get; set; }


        [DataMember(Name = "categoryName")]
        public string CategoryName { get; set; }

        [DataMember(Name="title")]
        public string Title { get; set; }

        [DataMember(Name="content")]
        public string Content { get; set; }

        [DataMember(Name="author")]
        public string Author { get; set; }
        
        [DataMember(Name="tags")]
        public IEnumerable<string> Tags { get; set; }

        [DataMember(Name="creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name = "comments")]
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}