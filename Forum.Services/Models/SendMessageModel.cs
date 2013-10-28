using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class SendMessageModel
    {
        [DataMember(Name="content")]
        public string Content { get; set; }
        [DataMember(Name = "toUser")]
        public string ToUser { get; set; }
    }
}