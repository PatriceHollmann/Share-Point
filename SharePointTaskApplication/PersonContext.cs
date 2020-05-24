using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SharePointTaskApplication
{
    //public class PersonContext : DbContext
    //{
    //    public DbSet<PersonData> Persons { get; set; }
    //}
    public class UserContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
    public class ContextInitializer: DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            string password = "123456";
            context.Roles.Add(new Role { Id = 1, Name = "Admin" });
            context.Roles.Add(new Role { Id = 2, Name = "User" });

            context.Users.Add(new UserData { Id = 1, RoleId = 1, Email = "somemail@mail.ru", PasswordHash = password/*.GetHashCode()*/ });
            base.Seed(context);
        }
    }
}