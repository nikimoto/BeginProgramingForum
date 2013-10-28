namespace Forum.Data.Migrations
{
    using Forum.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Forum.Data.ForumContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Forum.Data.ForumContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Categories.AddOrUpdate(
                c => c.Title,
                new Category
                {
                    Title = "Javascript",
                    Description = "...... we all love it"
                },
                new Category
                {
                    Title = "C#",
                    Description = "powerful language developed by S.Nakov. "
                });

            context.Users.AddOrUpdate(
                u => u.Username,
                new User
                {
                    IsAdmin = true,
                    AuthCode = "d033e22ae348aeb5660fc2140aec35850c4da997",
                    Username = "Administrator"
                });
        }
    }
}
