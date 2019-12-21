namespace Contacts.Migrations
{
    using Contacts.DAL;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Contacts.DAL.ContactsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Contacts.DAL.ContactsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //This creates a few contacts with assigned phone numbers for easier testing. This can be commented out before the 'update-database' command if you wish for an empty database.
            HelperForSeed.SeedData(context);
        }
    }
}
