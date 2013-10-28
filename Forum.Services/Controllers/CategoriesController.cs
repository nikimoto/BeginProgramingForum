using BlogSystemApp.Api.Controllers;
using Forum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Forum.Models;
using Forum.Services.Models;

namespace Forum.Services.Controllers
{
    public class CategoriesController : BaseApiController
    {
        [HttpGet]
        [ActionName("all")]
        public IQueryable<CategoryModel> GetAll()
        {
            ForumContext context = new ForumContext();

            var allCategories =
                from categoryEntity in context.Categories
                select new CategoryModel()
                {
                    Id = categoryEntity.Id,
                    Title = categoryEntity.Title,
                    Description = categoryEntity.Description
                };
            return allCategories;
            
        }


        [HttpPut]
        [ActionName("edit-by-id")]
        public HttpResponseMessage EditCategoryById(string sessionKey, CategoryEditModel newComment)
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

                 Comment currentComment = context.Comments.FirstOrDefault(comment => comment.Id == newComment.Id);

                 if (currentComment == null)
                 {
                     throw new ArgumentNullException("Comment you want to edit, doesn't exist.");
                 }

                 currentComment.Content = newComment.Content;

                 context.SaveChanges();

                 //TODO yoan change model from commentCreateModel to commentCreatedModel
                 var response = this.Request.CreateResponse(HttpStatusCode.OK, newComment);

                 return response;

             });

            return responseMessage;
        }

        [HttpDelete]
        [ActionName("delete-by-id")]
        public HttpResponseMessage DeleteCategoryById(string sessionKey, int id)
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

               Category currentCategory = context.Categories.FirstOrDefault(cat => cat.Id == id);

               if (currentCategory == null)
               {
                   throw new ArgumentNullException("Category you want to remove, doesn't exist.");
               }

               context.Categories.Remove(currentCategory);

               context.SaveChanges();

               var response = this.Request.CreateResponse(HttpStatusCode.OK);

               return response;

           });

            return responseMessage;
        }
    }
}
