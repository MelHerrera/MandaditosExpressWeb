namespace MandaditosExpress.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MandaditosExpress.Models.MandaditosDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MandaditosExpress.Models.MandaditosDB context)
        {

        }
    }
}
