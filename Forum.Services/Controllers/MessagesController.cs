using BlogSystemApp.Api.Controllers;
using Forum.Data;
using Forum.Models;
using Forum.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Forum.Services.Controllers
{
    public class MessagesController : BaseApiController
    {
        [HttpGet]
        [ActionName("getUsers")]
        public HttpResponseMessage getUsers(string sessionKey)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();
                  var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);

                  if (user == null)
                  {
                      throw new ArgumentException("Invalid user data!");
                  }

                  var chatUsers = context.Users.Where(u => u.RecievedMessages.Any(x => x.Sender.Id == user.Id) ||
                      u.SentMessages.Any(x => x.Reciever.Id == user.Id));

                  var users = from u in chatUsers
                              select new UserChatModel
                              {
                                  Id = u.Id,
                                  Username = u.Username
                              };

                  //var messages = user.RecievedMessages.Union(user.SentMessages);
                  //IEnumerable<User> users = messages.Select(return x => new User { 
                  //    Id = x.Receiver.Id == user.Id ? x.Receiver.Id : x.Sender.Id, 
                  //    AuthCode = x.Receiver.Id == user.Id ? x.Receiver.AuthCode : x.Sender.AuthCode };);

                  var response = this.Request.CreateResponse(HttpStatusCode.OK);

                  return response;
              });

            return responseMessage;
        }

        [HttpGet]
        [ActionName("getMessagesWithUser")]
        public HttpResponseMessage GetMessagesWithUser(string sessionKey, int userId)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();
                  var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);

                  if (user == null)
                  {
                      throw new ArgumentException("Invalid user data!");
                  }

                  var allMessages = user.RecievedMessages.Where(m => m.Sender.Id == userId)
                      .Union(user.SentMessages.Where(m => m.Reciever.Id == userId).OrderBy(m => m.CreationDate));

                  var messages = from m in allMessages
                                 select new MessageModel
                                 {
                                     Content = m.Content,
                                     CreationDate = m.CreationDate,
                                     ReceiverId = m.Reciever.Id,
                                     SenderId = m.Sender.Id
                                 };

                  var response = this.Request.CreateResponse(HttpStatusCode.OK, messages);

                  return response;
              });

            return responseMessage;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage PostCreate(SendMessageModel messageModel, string sessionKey)
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
                            throw new ArgumentException("Invalid user data!");
                        }

                        User reciever = context.Users.FirstOrDefault(usr => usr.Username == messageModel.ToUser);
                        if (reciever == null)
                        {
                            throw new ArgumentException("Cannot send message to not registred users!");
                        }

                        var message = new Message
                        {
                            Sender = currentUser,
                            Reciever = reciever,
                            Content = messageModel.Content,
                            CreationDate = DateTime.Now
                        };

                        context.Messages.Add(message);
                        context.SaveChanges();
                       
                        HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, message);

                        return response;
                    }
                });

            return responseMessage;
        }
    }
}
