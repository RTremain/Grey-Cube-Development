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
                    new Employee{Name="Mike", PwHash="Un7t26TLhFryf/dnhQQmacI66QjmgQd7OG/lh8RtRfM=", PwSalt="ArOuHUFAUilUiKF8IY2/oQ==" },
                    new Employee{Name="Steve", PwHash="FwqbbFWubNGjxSt37Mltb87e6/l56svraxaNfh1TecI=", PwSalt="iYgoFRuOJGl49tfcwUvtVA==" }
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

            

            if (!context.Member.Any()) // we need to add members
            {

                var members = new Member[]
                {
                    new Member { Username = "Jalapegnome", PwSalt = "gr+xTxUQDCD1yPQg1pLbrQ==", PwHash = "6cMFqUXDd6f2lJ5UErvMBPavN4kfcwEEDSGMoN2iFmQ=", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "IdleCyborg", PwSalt = "URX7oMVni75HnzUFpkvi1A==", PwHash = "Lp2mQj+0Bu8lfMRlku3RqFIx4N+zbOgI3tD+WU7g9po=", Email = "b@b.com", Phone = "111 222-3333" },
                    new Member { Username = "Pyrogue", PwSalt = "XCAClqhvLKZUCpE2JPj29A==", PwHash = "2rUpErSbLwoFJv1KcotK1IMAxuN0+tIGdCFw0QA6nHc=", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "AdviceBoulder", PwSalt = "ygcEqm61GxL9CZgxu9GAJQ==", PwHash = "Qi8f4LuCVGg6DnhE+iXk+3WoRGNXO0S6QbEAdTyXCqA=", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "AuthorityLord", PwSalt = "gIA4jYvJRfNBMdksBWLKaA==", PwHash = "KVQ6l+a+zxJSEjfkOuMKFiTwYWckoc7Z1WaOvMZdoJQ=", Email = "a@a.com", Phone = "123 456-7890" },
                    new Member { Username = "Sellamander", PwSalt = "SOnZ/eHVfTF9uS0k77tJCw==", PwHash = "VWCcu8NN+tPadIwgG9M6HLNuNfreSOefVPvxBkym9og=", Email = "a@a.com", Phone = "123 456-7890" }
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
                            new Library { MemberId = memberId.MemberId, GameId = gameList[0].GameId },
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

            if (!context.Event.Any()) // we need to add events
            {

                var events = new Event[]
                {
                    new Event { Title = "Tabletop night", EventDate = new DateTime(2016, 5, 13, 19, 30, 0), Description = "Boardgames all night" },
                    new Event { Title = "LAN Night", EventDate = new DateTime(2016, 5, 13, 19, 30, 0), Description = "LAN gaming night" },
                    new Event { Title = "Blackjack", EventDate = new DateTime(2016, 5, 13, 19, 30, 0), Description = "Playing black jack" },
                    new Event { Title = "Illegal gambling night", EventDate = new DateTime(2016, 5, 13, 19, 30, 0), Description = "Don't tell the feds" },
                    new Event { Title = "Indie Feature Night",EventDate = new DateTime(2018, 8, 9, 18, 0, 0), Description = "All indie titles 50% off" }
                };

                foreach (Event e in events)
                {
                    context.Event.Add(e);
                }

                context.SaveChanges();
            }

            if (!context.Attendance.Any())
            {
                var memberId = context.Member.FirstOrDefault(m => m.Username == "Jalapegnome");
                if (memberId != null)
                {
                    var eventList = context.Event.ToList();

                    if (eventList.Count >= 2)
                    {
                        var attendanceList = new Attendance[]
                        {
                            new Attendance { MemberId = memberId.MemberId, EventId = eventList[0].EventId },
                            new Attendance { MemberId = memberId.MemberId, EventId = eventList[1].EventId }
                        };

                        foreach (Attendance a in attendanceList)
                        {
                            context.Attendance.Add(a);
                        }
                        context.SaveChanges();
                    }

                }

            }

        } // End Initialize()
    }
}
