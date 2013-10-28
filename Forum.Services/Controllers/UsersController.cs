using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Forum.Models;
using Forum.Data;
using Forum.Services;
using Forum.Services.Models;

namespace BlogSystemApp.Api.Controllers
{
    public class UsersController : BaseApiController
    {

        private const int MinUsernameLength = 6;
        private const int MaxUsernameLength = 30;
        private const int MinDisplayNameLength = 6;
        private const int MaxDisplayNameLength = 30;
        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";
        private const string ValidDisplayNameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";

        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private static readonly Random rand = new Random();

        private const int SessionKeyLength = 50;

        private const int Sha1Length = 40;

        private string GenerateSessionKey(int userId)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(userId);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }
            return skeyBuilder.ToString();
        }

        private void ValidateAuthCode(string inputAuthCode)
        {
            if (inputAuthCode == null)
            {
                throw new ArgumentNullException("Authcode cannot be null value. It should be SHA1 encripted password.");
            }
            else if (inputAuthCode == "")
            {
                throw new ArgumentException("Authcode cannot be empty value. It should be SHA1 encripted password.");
            }
            else if (inputAuthCode.Length != Sha1Length)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Authcode should be SHA1 encripted password with exactly {0} symbols.", Sha1Length));
            }
        }

        private void ValidateDisplayName(string inputDisplayName)
        {
            if (inputDisplayName == null)
            {
                throw new ArgumentNullException("Display name cannot be null value.");
            }
            else if (inputDisplayName.Length < MinDisplayNameLength || inputDisplayName.Length > MaxDisplayNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Display name should be between {0} and {1} characters long.",
                        MinDisplayNameLength, MaxDisplayNameLength));
            }
            else if (inputDisplayName.Any(ch => !ValidDisplayNameCharacters.Contains(ch)))
            {
                throw new ArgumentException("Displayname contains invalid characters. It should contain only Latin letters, digits and the characters '_' (underscore),'-' (dash), ' ' (space) and '.' (dot).");
            }
        }

        private void ValidateUsername(string inputUsername)
        {
            if (inputUsername == null)
            {
                throw new ArgumentNullException("Username cannot be null value.");
            }
            else if (inputUsername.Length < MinUsernameLength || inputUsername.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username should be between {0} and {1} characters long.",
                        MinUsernameLength, MaxUsernameLength));
            }
            else if (inputUsername.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentException("Username contains invalid characters. It should contain only Latin letters, digits and the characters '_' (underscore) and '.' (dot).");
            }
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage PostRegisterUser(UserFlatModel inputUser)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
                 () =>
                 {
                     ForumContext context = new ForumContext();

                     using (context)
                     {
                         this.ValidateUsername(inputUser.Username);
                         this.ValidateAuthCode(inputUser.AuthCode);

                         var usernameToLower = inputUser.Username.ToLower();

                         User user = context.Users.FirstOrDefault(
                             usr => usr.Username == usernameToLower);
                         if (user != null)
                         {
                             throw new InvalidOperationException("User already exists");
                         }

                         user = new User()
                         {
                             Username = usernameToLower,
                             AuthCode = inputUser.AuthCode,
                             CreationDate = DateTime.Now
                         };

                         context.Users.Add(user);
                         context.SaveChanges();

                         user.SessionKey = this.GenerateSessionKey(user.Id);
                         context.SaveChanges();

                         UserLoggedModel loggedModel = new UserLoggedModel()
                         {
                             Username = user.Username,
                             SessionKey = user.SessionKey
                         };

                         HttpResponseMessage response =
                             this.Request.CreateResponse(HttpStatusCode.Created,
                                             loggedModel);
                         return response;
                     }
                 });

            return responseMessage;
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage PostLoginUser(UserFlatModel inputUser)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();

                  using (context)
                  {
                      this.ValidateUsername(inputUser.Username);
                      this.ValidateAuthCode(inputUser.AuthCode);

                      var usernameToLower = inputUser.Username.ToLower();

                      User user = context.Users.FirstOrDefault(
                          usr => usr.Username == usernameToLower
                          && usr.AuthCode == inputUser.AuthCode);

                      if (user == null)
                      {
                          throw new InvalidOperationException("Invalid username or password");
                      }
                      if (user.SessionKey == null)
                      {
                          user.SessionKey = this.GenerateSessionKey(user.Id);
                          context.SaveChanges();
                      }
                      if (user.IsBanned == true)
                      {
                          throw new ArgumentException("User is banned.");
                      }

                      UserLoggedModel loggedModel = new UserLoggedModel()
                      {
                          Username = user.Username,
                          SessionKey = user.SessionKey
                      };

                      var response =
                          this.Request.CreateResponse(HttpStatusCode.Created,
                                          loggedModel);
                      return response;
                  }
              });

            return responseMessage;
        }

        [HttpPut]
        [ActionName("logout")]
        public HttpResponseMessage PutLogoutUser(string sessionKey)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();

                  using (context)
                  {
                      User currentUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

                      if (currentUser == null)
                      {
                          throw new ArgumentException("User is not registered or not logged in.");
                      }

                      currentUser.SessionKey = null;

                      context.SaveChanges();

                      string logoutText = "You successfully have logged out.";

                      var response = this.Request.CreateResponse(HttpStatusCode.Accepted, logoutText);

                      return response;
                  }
              });

            return responseMessage;
        }

        [HttpGet]
        [ActionName("current")]
        public HttpResponseMessage GetById(string sessionKey)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();

                  using (context)
                  {

                      User currentUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

                      if (currentUser == null)
                      {
                          throw new ArgumentNullException("User you are looking for is not logged or doesn't exist.");
                      }

                      var response = this.Request.CreateResponse(HttpStatusCode.OK, currentUser);

                      return response;
                  }
              });

            return responseMessage;
        }

        [HttpGet]
        [ActionName("all")]
        public IQueryable<UserModel> GetAll()
        {
            ForumContext context = new ForumContext();

            var allUsers =
                from UserEntity in context.Users
                select new UserModel()
                {
                    AuthCode = UserEntity.AuthCode,
                    IsAdmin = UserEntity.IsAdmin,
                    CreationDate = UserEntity.CreationDate,
                    SessionKey = UserEntity.SessionKey,
                    Username = UserEntity.Username,
                    Id = UserEntity.Id,
                    IsBanned = UserEntity.IsBanned
                };

            return allUsers;
        }

        [HttpPut]
        [ActionName("edit-by-id")]
        public HttpResponseMessage EditUserById(string sessionKey, UserEditModel newUser)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
             () =>
             {
                 ForumContext context =new ForumContext();

                 User adminUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

                 if (adminUser == null)
                 {
                     throw new ArgumentNullException("If you want to edit users, you have to login or register first.");
                 }

                 if(adminUser.IsAdmin == false)
                 {
                     throw new ArgumentException("You have to be admin, to edit users.");
                 }
                 
                 User currentUser = context.Users.FirstOrDefault(usr => usr.Id == newUser.Id);

                 if (currentUser == null)
                 {
                     throw new ArgumentNullException("User you want to edit, doesn't exist.");
                 }

                 //currentUser.Username = newUser.Username;
                 currentUser.IsBanned = newUser.IsBanned;

                 context.SaveChanges();

                 UserModel result = UserModel.Parse(currentUser);

                 var response = this.Request.CreateResponse(HttpStatusCode.OK, result);

                 return response;

             });

            return responseMessage;
        }

        [HttpDelete]
        [ActionName("delete-by-id")]
        public HttpResponseMessage DeleteUserById(string sessionKey, int id)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
           () =>
           {
               ForumContext context = new ForumContext();

               User adminUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

               if (adminUser == null)
               {
                   throw new ArgumentNullException("If you want to edit users, you have to login or register first.");
               }

               if (adminUser.IsAdmin == false)
               {
                   throw new ArgumentException("You have to be admin, to edit users.");
               }

               User currentUser = context.Users.FirstOrDefault(usr => usr.Id == id);

               if (currentUser == null)
               {
                   throw new ArgumentNullException("User you want to edit, doesn't exist.");
               }

               currentUser.IsBanned = true;

               context.SaveChanges();

               UserModel result = UserModel.Parse(currentUser);

               var response = this.Request.CreateResponse(HttpStatusCode.OK, result);

               return response;

           });

            return responseMessage;
        }
    }
}
