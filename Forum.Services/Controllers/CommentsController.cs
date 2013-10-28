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
    public class CommentsController : BaseApiController
    {

        [HttpGet]
        public IQueryable<CommentModel> GetAll()
        {
            ForumContext context = new ForumContext();

            var allComments =
                from commentEntity in context.Comments
                select new CommentModel()
                {
                    Author = commentEntity.Author.Username,
                    Content = commentEntity.Content,
                    CreationDate = commentEntity.CreationDate
                };

            return allComments;
        }

        [HttpGet]
        [ActionName("from-post")]
        public HttpResponseMessage GetByPostId(string sessionKey, int postId)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();

                  using (context)
                  {
                      var allComments =
                          from commentEntity in context.Comments
                          where commentEntity.Post.Id == postId
                          select new CommentModel()
                          {
                              Author = commentEntity.Author.Username,
                              Content = commentEntity.Content,
                              CreationDate = commentEntity.CreationDate
                          };


                      var response = this.Request.CreateResponse(HttpStatusCode.OK, allComments);

                      return response;

                  }
              });

            return responseMessage;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage PostCreateComment(CommentCreateModel inputComment, string sessionKey)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  ForumContext context = new ForumContext();

                  User currentUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

                  Post currentPost = context.Posts.FirstOrDefault(pst => pst.Id == inputComment.PostId);

                  if (currentUser == null)
                  {
                      throw new ArgumentNullException("If you want to comment posts, you should be logged or registered.");
                  }

                  if (currentPost == null) 
                  {
                      throw new ArgumentNullException("The post you are trying to comment, doesn't exist.");
                  }

                  Comment newComment = new Comment()
                  {
                      Author = currentUser,
                      Post = currentPost,
                      Content = inputComment.Content,
                      CreationDate = DateTime.Now
                  };


                  context.Comments.Add(newComment);
                  context.SaveChanges();

                  CommentModel createdComment = new CommentModel()
                  {
                      Author = newComment.Author.Username,
                      Content = newComment.Content,
                      CreationDate = newComment.CreationDate
                  };

                  return this.Request.CreateResponse(HttpStatusCode.Created, createdComment);
              });

            return responseMessage;
        }

        //[HttpPut]
        //[ActionName("edit-by-id")]
        //public HttpResponseMessage EditUserById(string sessionKey, CommentEditModel newUser)
        //{
        //    HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
        //     () =>
        //     {
        //         ForumContext context = new ForumContext();

        //         User adminUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

        //         if (adminUser == null)
        //         {
        //             throw new ArgumentNullException("If you want to edit users, you have to login or register first.");
        //         }

        //         if (adminUser.IsAdmin == false)
        //         {
        //             throw new ArgumentException("You have to be admin, to edit users.");
        //         }

        //         User currentUser = context.Users.FirstOrDefault(usr => usr.Id == newUser.Id);

        //         if (currentUser == null)
        //         {
        //             throw new ArgumentNullException("User you want to edit, doesn't exist.");
        //         }

        //         currentUser.Username = newUser.Username;

        //         context.SaveChanges();

        //         UserModel result = UserModel.Parse(currentUser);

        //         var response = this.Request.CreateResponse(HttpStatusCode.OK, result);

        //         return response;

        //     });

        //    return responseMessage;
        //}

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

               context.Users.Remove(currentUser);

               context.SaveChanges();

               UserModel result = UserModel.Parse(currentUser);

               var response = this.Request.CreateResponse(HttpStatusCode.OK, result);

               return response;

           });

            return responseMessage;
        }
    }
}