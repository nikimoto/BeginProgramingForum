using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class UserFlatModel
    {
        [DataMember(Name="username")]
        public string Username { get; set; }
        
        [DataMember(Name="authCode")]
        public string AuthCode { get; set; }
    }
}