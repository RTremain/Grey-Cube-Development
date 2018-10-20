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
                    new Employee{Name="Mike", PwHash="gn16/Lx7NN0YVhdmYy83AAdKs8kEL760FJv69wd3C4c=", PwSalt="yfQLySCG0lSsaN3hXZ1JWQ==" },
                    new Employee{Name="Steve", PwHash="zmzrqTW3ITuGFK37hW9PSCMauTL85r8stxt9dFOiVdA=", PwSalt="VI9QMNvV3sSdtcxCcxzXAQ==" }
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
                    new Game { Title = "Doom", ReleaseDate = new DateTime(2016, 5, 13) },
                    new Game { Title = "Monster Hunter: World", ReleaseDate = new DateTime(2018, 8, 9) },
                    new Game { Title = "Fallout 4", ReleaseDate = new DateTime(2015, 11, 10) }
                };

                foreach (Game g in games)
                {
                    context.Game.Add(g);
                }

                context.SaveChanges();
            }

            if (!context.Event.Any()) // we need to add events
            {

                var events = new Event[]
                {
                    new Event { Title = "Tabletop night", EventDate = new DateTime(2016, 5, 13, 19, 30, 0), Description = "Boardgames all night" },
                    new Event { Title = "Indie Feature Night",EventDate = new DateTime(2018, 8, 9, 18, 0, 0), Description = "All indie titles 50% off" }
                };

                foreach (Event e in events)
                {
                    context.Event.Add(e);
                }

                context.SaveChanges();
            }

            if (!context.Member.Any()) // we need to add members
            {

                var members = new Member[]
                {
                    new Member { Username = "Jalapegnome", PwHash = "Password", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "IdleCyborg", PwHash = "Password2", Email = "b@b.com", Phone = "111 222-3333" }
                };

                foreach (Member m in members)
                {
                    context.Member.Add(m);
                }

                context.SaveChanges();
            }

            if (!context.Library.Any())
            {
                var memberId = context.Member.FirstOrDefault(m => m.Username == "Jalapegnome");
                if (memberId != null)
                {
                    int[] gameIds = new int[2];
                    var gameList = context.Game.ToList();

                    if (gameList.Count >= 2)
                    {
                        var libraryGames = new Library[]
                    {
                        new Library { MemberId=  memberId.MemberId, GameId = gameList[0].GameID },
                        new Library { MemberId = memberId.MemberId, GameId = gameList[1].GameID }
                    };

                        foreach (Library l in libraryGames)
                        {
                            context.Library.Add(l);
                        }
                        context.SaveChanges();
                    }
                    
                }
                
            }

        }
    }
}
