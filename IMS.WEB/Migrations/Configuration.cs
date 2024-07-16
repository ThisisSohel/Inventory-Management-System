using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace IMS.WEB.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<IMS.WEB.Database.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; //Here I have  to modify only when I need to change something. 
            ContextKey = "IMS.WEB.Database.ApplicationDbContext";
        }

        protected override void Seed(IMS.WEB.Database.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}