namespace TrainTickets.MigrationsTickets
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class ConfigurationTickets : DbMigrationsConfiguration<TrainTickets.Models.TicketsDbContext>
    {
        public ConfigurationTickets()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TrainTickets.Models.TicketsDbContext";
        }

        protected override void Seed(TrainTickets.Models.TicketsDbContext context)
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
        }
    }
}
