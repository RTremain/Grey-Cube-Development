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
            context.Database.EnsureDeleted();
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
                    new Game { Title = "Starcraft 2", ReleaseDate = new DateTime(2010, 7, 10) },
                    new Game { Title = "Call of Duty: Modern Warfare", ReleaseDate = new DateTime(2007, 11, 7) },
                    new Game { Title = "The Elder Scrolls V: Skyrim", ReleaseDate = new DateTime(2011, 11, 11) },
                    new Game { Title = "Minecraft", ReleaseDate = new DateTime(2009, 5, 17) },
                    new Game { Title = "Battlefield 3", ReleaseDate = new DateTime(2011, 10, 25) },
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
                    new Member { Username = "Jalapegnome", PwSalt = "0d3nuW96aA65mhWOwi7RMQ==", PwHash = "qV3GGAd6GWI/nR1D6MwV6S60qVWE6mEAfYCyhO02J20=", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "IdleCyborg", PwSalt = "LxydHI+ySV29m7mnQbvtTQ==", PwHash = "BoW2Aeeb1tSpG82SRdZscpFOdzHr/BNy1dzdIWWPjKw=", Email = "b@b.com", Phone = "111 222-3333" },
                    new Member { Username = "Pyrogue", PwSalt = "U2GEjIO/M2a1VVrtAUxsZQ==", PwHash = "woH6fq/v9ivvnU5KZoY8H53XaCpkMGAuUZNFunLo5Cc=", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "AdviceBoulder", PwSalt = "", PwHash = "password", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "AuthorityLord", PwSalt = "", PwHash = "password", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "Sellamander", PwSalt = "", PwHash = "password", Email = "a@a.com", Phone = "123 456-7890" }
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
                            new Library { MemberId=  memberId.MemberId, GameId = gameList[0].GameId },
                            new Library { MemberId = memberId.MemberId, GameId = gameList[1].GameId }
                        };

                        foreach (Library l in libraryGames)
                        {
                            context.Library.Add(l);
                        }
                        context.SaveChanges();
                    }
                    
                }
                
            }

            if (!context.Wishlist.Any())
            {
                var memberId = context.Member.FirstOrDefault(m => m.Username == "Jalapegnome");
                if (memberId != null)
                {
                    int[] gameIds = new int[2];
                    var gameList = context.Game.ToList();

                    if (gameList.Count >= 4)
                    {
                        var wishlistGames = new Wishlist[]
                        {
                            new Wishlist { MemberId = memberId.MemberId, GameId = gameList[2].GameId },
                            new Wishlist { MemberId = memberId.MemberId, GameId = gameList[3].GameId }
                        };

                        foreach (Wishlist w in wishlistGames)
                        {
                            context.Wishlist.Add(w);
                        }
                        context.SaveChanges();
                    }

                }

            }

            if (!context.CreditCard.Any())
            {
                var member = context.Member.FirstOrDefault(m => m.Username == "Jalapegnome");
                if (member != null)
                {
                    var creditCards = new CreditCard[]
                    {
                        new CreditCard
                        {
                            CcNum = "5150690040978244",
                            ExpMonth = 5,
                            ExpYear = 2022,
                            Name = "Mike Smith",
                            StreetAddress = "123 city street",
                            City = "Waterloo",
                            Province = "Ontario",
                            PostalCode = "A1B 2C3",
                            MemberId = member.MemberId
                        },
                        new CreditCard
                        {
                            CcNum = "5177244191651488",
                            ExpMonth = 4,
                            ExpYear = 2020,
                            Name = "Mike Smith",
                            StreetAddress = "123 city street",
                            City = "Waterloo",
                            Province = "Ontario",
                            PostalCode = "A1B 2C3",
                            MemberId = member.MemberId
                        }
                    };

                    foreach (CreditCard c in creditCards)
                    {
                        context.CreditCard.Add(c);
                    }
                    context.SaveChanges();
                }
            } // End if (!context.CreditCard.Any())

            if (!context.Friend.Any())
            {
                var member = context.Member.FirstOrDefault(m => m.Username == "Jalapegnome");
                var friends = new Member[]
                {
                    context.Member.FirstOrDefault(m => m.Username == "Pyrogue"),
                    context.Member.FirstOrDefault(m => m.Username == "Sellamander"),
                    context.Member.FirstOrDefault(m => m.Username == "AdviceBoulder")
                };

                if (member != null)
                {

                    
                    var friendlist = new Friend[]
                    {
                        new Friend { MemberId = member.MemberId, FriendMemberId = friends[0].MemberId },
                        new Friend { MemberId = member.MemberId, FriendMemberId = friends[1].MemberId },
                        new Friend { MemberId = member.MemberId, FriendMemberId = friends[2].MemberId },
                        new Friend { MemberId = friends[0].MemberId, FriendMemberId = member.MemberId },
                        new Friend { MemberId = friends[1].MemberId, FriendMemberId = member.MemberId },
                        new Friend { MemberId = friends[2].MemberId, FriendMemberId = member.MemberId }

                    };

                    foreach (Friend f in friendlist)
                    {
                        context.Friend.Add(f);
                    }
                    context.SaveChanges();
                    

                }

            }

        } // End Initialize()
    }
}
