namespace FitnessViewer.Infrastructure.Migrations
{
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FitnessViewer.Infrastructure.Data.ApplicationDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FitnessViewer.Infrastructure.Data.ApplicationDb context)
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

            Repository repo = new Repository();
            // if no data in calendar then populate it.  1980->2050 should be plenty
            if (repo.GetCalendar().ToList().Count == 0)
            {

                DateTime date = new DateTime(1980, 1, 1);

                List<Calendar> detailsToAdd = new List<Calendar>();

                while (date <= new DateTime(2050, 12, 31))
                {
                    detailsToAdd.Add(new Calendar(date));
                    date = date.AddDays(1);
                }
                repo.AddCalendarDates(detailsToAdd);
                repo.SaveChanges();
            }

            using (ApplicationDb db = new ApplicationDb())
            {
                List<Calendar> nullWeekLabel =db.Calendar.Where(c => c.WeekLabel == null).ToList();

                if (nullWeekLabel.Count > 0)
                {
                    foreach (Calendar cal in nullWeekLabel)
                    {
                        // populate WeekLabel property.
                        cal.UpdateValuesForDate();

                        // update 
                        db.Calendar.Attach(cal);
                        var entry = db.Entry(cal);
                        entry.Property(c => c.WeekLabel).IsModified = true;
                        db.SaveChanges();
                    }
                   
                }
            }
        }
    }
}

