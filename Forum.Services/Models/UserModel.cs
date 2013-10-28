using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Forum.Services.Models
{
    [DataContract]
    public class UserModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        
        [DataMember(Name="username")]
        public string Username { get; set; }
        
        [DataMember(Name="authCode")]
        public string AuthCode { get; set; }
        
        [DataMember(Name="sessionKey")]
        public string SessionKey { get; set; }
        
        [DataMember(Name="creationDate")]
        public DateTime CreationDate { get; set; }
        
        [DataMember(Name="isAdmin")]
        public bool IsAdmin { get; set; }

        [DataMember(Name="isBanned")]
        public bool IsBanned { get; set; }

        public static UserModel Parse(User currentUser)
        {
            return new UserModel()
            {
                AuthCode = currentUser.AuthCode,
                CreationDate = currentUser.CreationDate,
                IsAdmin = currentUser.IsAdmin,
                Id = currentUser.Id,
                SessionKey = currentUser.SessionKey,
                Username = currentUser.Username,
                IsBanned = currentUser.IsBanned
            };
        }
    }
}