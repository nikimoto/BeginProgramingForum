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
    public class PostsController : BaseApiController
    {
        [HttpGet]
        [ActionName("all")]
        public IEnumerable<PostModel> GetAll()
        {
            ForumContext context = new ForumContext();

            var allPosts =
                from postEntity in context.Posts
                select new PostModel()
                {
                    Author = postEntity.Author.Username,
                    Id = postEntity.Id,
                    CategoryId = postEntity.Category.Id,
                    CategoryName = postEntity.Category.Title,
                    Title = postEntity.Title,
                    CreationDate = postEntity.CreationDate,
                    Content = postEntity.Content,
                    Comments =
                        from commentEntity in postEntity.Comments
                        select new CommentModel()
                        {
                            Author = commentEntity.Author.Username,
                            Content = commentEntity.Content,
                            CreationDate = commentEntity.CreationDate
                        },
                    Tags =
                        from tagEntity in postEntity.Tags
                        select tagEntity.Name
                };

            return allPosts;
        }

        [HttpGet]
        [ActionName("by-id")]
        public HttpResponseMessage GetById(int id)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
              () =>
              {
                  PostModel currentPost = GetAll().FirstOrDefault(pst => pst.Id == id);

                  if (currentPost == null)
                  {
                      throw new ArgumentException("The post you are searching, doesn't exist");
                  }
                  else
                  {
                      HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, currentPost);

                      return response;
                  }
              });

            return responseMessage;
        }

        [HttpGet]
        [ActionName("by-category")]
        public HttpResponseMessage GetByCategoryId(int id)
        {
            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    ForumContext context = new ForumContext();

                    Category currentCategory = context.Categories.FirstOrDefault(cat => cat.Id == id);

                    if (currentCategory == null)
                    {
                        throw new ArgumentNullException(string.Format("Category with id: {0} doesn't exist.", id));
                    }

                    var filteredPosts = GetAll().Where(pst => pst.CategoryId == id);

                    CategoryWithPostsModel result = new CategoryWithPostsModel()
                    {
                        Title = currentCategory.Title,
                        Posts = filteredPosts,
                        Description = currentCategory.Description
                    };

                    HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, result);
                    
                    return response;
                });

            return responseMessage;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage PostCreate(PostRegisterModel inputPost, string sessionKey)
        {

            HttpResponseMessage responseMessage = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    ForumContext context = new ForumContext();

                    using(context)
                    {
                        User currentUser = context.Users.FirstOrDefault(usr => usr.SessionKey == sessionKey);

                        int currentCategoryId = inputPost.CurrentCategoryId;

                        Category currentCategory = context.Categories.FirstOrDefault(cat => cat.Id == currentCategoryId);

                        if(currentUser == null)
                        {
                            throw new ArgumentNullException("You should be logged or registered to create new posts.");
                        }

                        if(currentCategory == null)
                        {
                            throw new ArgumentNullException("You try to create post in non-existing category.");
                        }

                        Post newPost = new Post()
                        {
                            Author = currentUser,
                            Category = currentCategory,
                            Content = inputPost.Content,
                            CreationDate = DateTime.Now,
                            Title = inputPost.Title                            
                        };


                        foreach(string tagName in inputPost.Tags)
                        {
                            Tag currentTag = context.Tags.FirstOrDefault(t => t.Name == tagName);

                            if(currentTag == null)
                            {
                                currentTag = new Tag()
                                {
                                    Name = tagName
                                };

                                context.Tags.Add(currentTag);
                                context.SaveChanges();

                                newPost.Tags.Add(currentTag);
                            }
                            else
                            {
                                newPost.Tags.Add(currentTag);
                            }
                        }


                        context.Posts.Add(newPost);
                        context.SaveChanges();

                        var resultPost = new PostModel
                        {
                            Id = newPost.Id,
                            Content = newPost.Content,
                            CategoryName = newPost.Category.Title,
                            CategoryId = newPost.Category.Id,
                            CreationDate = newPost.CreationDate,
                            Tags = (from t in newPost.Tags
                                   select t.Name),
                            Title = newPost.Title,
                            Author = newPost.Author.Username
                        };

                        HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, resultPost);

                        return response;
                    }
                });

            return responseMessage;
        }

        [HttpPut]
        [ActionName("edit-by-id")]
        public HttpResponseMessage EditPostById(string sessionKey, PostEditModel newPost)
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

                 Post currentPost = context.Posts.FirstOrDefault(pst => pst.Id == newPost.Id);

                 if (currentPost == null)
                 {
                     throw new ArgumentNullException("User you want to edit, doesn't exist.");
                 }

                 currentPost.Content = newPost.Content;

                 context.SaveChanges();


                 var response = this.Request.CreateResponse(HttpStatusCode.OK, newPost);

                 return response;

             });

            return responseMessage;
        }

        [HttpDelete]
        [ActionName("delete-by-id")]
        public HttpResponseMessage DeletePostById(string sessionKey, int id)
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

               Post currentPost = context.Posts.FirstOrDefault(pst => pst.Id == id);

               if (currentPost == null)
               {
                   throw new ArgumentNullException("Post you want to edit, doesn't exist.");
               }

               context.Posts.Remove(currentPost);

               context.SaveChanges();

               //UserModel result = UserModel.Parse(currentUser);

               var response = this.Request.CreateResponse(HttpStatusCode.NoContent);

               return response;

           });

            return responseMessage;
        }
    }
}
