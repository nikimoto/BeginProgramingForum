using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class PostCreatedModel
    {
        [DataMember(Name="title")]
        public string Title { get; set; }

        [DataMember(Name="content")]
        public string Content { get; set; }

        [DataMember(Name="author")]
        public string Author { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name="creationDate")]
        public DateTime CreationDate { get; set; }

        [DataMember(Name="tags")]
        public ICollection<string> Tags { get; set; }

        public static PostCreatedModel Parse (Post post)
        {
            PostCreatedModel resultPost = new PostCreatedModel()
            {
                Author = post.Author.Username,
                Content = post.Content,
                Title  = post.Title,
                CreationDate = post.CreationDate,
                Category = post.Category.Title,
                Tags = new List<string>()
            };

            foreach(Tag currentTag in post.Tags)
            {
                resultPost.Tags.Add(currentTag.Name);
            }

            return resultPost;
        }
    }
}