using System;
using System.Collections.Generic;
using System.Linq;
using GCDGameStore.Models;

namespace GCDGameStore.Data
{
    public class DbInitializer
    {
        public static void Initialize(GcdGameStoreContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Employee.Any()) // we need to add employees
            {
                var employees = new Employee[]
                {
                new Employee{Name="Mike", PwHash="Password1" },
                new Employee{Name="Steve", PwHash="Password2" }
                };

                foreach (Employee e in employees)
                {
                    context.Employee.Add(e);
                }

                context.SaveChanges();
            }


            if (!context.Game.Any()) // we need to add games
            {

                var games = new Game[]
                {
                new Game{Title="Doom",ReleaseDate = new DateTime(2016, 5, 13) },
                new Game{Title="Monster Hunter: World",ReleaseDate = new DateTime(2018, 8, 9) }
                };

                foreach (Game g in games)
                {
                    context.Game.Add(g);
                }

                context.SaveChanges();
            }


        }
    }
}
