using Xunit;
using SharedModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GameServices;

namespace Tests
{
    public class DatabaseTests
    {
        // Fournit un DbContext InMemory isolé pour chaque test
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_" + System.Guid.NewGuid())
                .Options;

            return new AppDbContext(options);
        }

        //Test 1 : Ajout d'un Player
        [Fact]
        public void Test_AjoutPlayer_Fonctionne()
        {
            var context = GetDbContext();
            int countBefore = context.Players.Count();

            var player = new Player
            {
                Name = "Younes",
                PV = 100,
                ATQ = 20,
                Score = 10
            };

            context.Players.Add(player);
            context.SaveChanges();

            int countAfter = context.Players.Count();
            Assert.Equal(countBefore + 1, countAfter);
        }

        //test 2 : Ajout d'un Ennemie
        [Fact]
        public void Test_AjoutEnnemie_Fonctionne()
        {
            var context = GetDbContext();
            var ennemie = new Ennemie
            {
                Name = "Gobelin",
                PV = 60,
                ATQ = 12
            };

            context.Ennemies.Add(ennemie);
            context.SaveChanges();

            var saved = context.Ennemies.FirstOrDefault(e => e.Name == "Gobelin");
            Assert.NotNull(saved);
            Assert.Equal(12, saved.ATQ);
        }

        //test 3 : Ajout d’un User
        [Fact]
        public void Test_AjoutUser_Fonctionne()
        {
            var context = GetDbContext();
            var user = new User
            {
                Login = "userTest",
                Password = "1234",
                IsAdmin = false
            };

            context.Users.Add(user);
            context.SaveChanges();

            var saved = context.Users.FirstOrDefault(u => u.Login == "userTest");
            Assert.NotNull(saved);
            Assert.False(saved.IsAdmin);
        }

        //Test 4 : Ajout d’un Characteres de base
        [Fact]
        public void Test_AjoutCharactereBase_Fonctionne()
        {
            var context = GetDbContext();
            var chara = new Charactere
            {
                Name = "Aventurier",
                PV = 80,
                ATQ = 15
            };

            context.Characters.Add(chara);
            context.SaveChanges();

            var saved = context.Characters.FirstOrDefault(c => c.Name == "Aventurier");
            Assert.NotNull(saved);
            Assert.Equal(80, saved.PV);
        }

        //Test 5 : Création d'une Room avec un player et un Ennemie
        [Fact]
        public void Test_CreationRoom_Fonctionne()
        {
            var context = GetDbContext();

            var player = new Player { Name = "Hero", PV = 100, ATQ = 20, Score = 5 };
            var ennemie = new Ennemie { Name = "Orc", PV = 90, ATQ = 15 };
            var room = new Room { Level = 1, Player = player, Ennemie = ennemie };

            context.Rooms.Add(room);
            context.SaveChanges();

            var savedRoom = context.Rooms
                .Include(r => r.Player)
                .Include(r => r.Ennemie)
                .FirstOrDefault();

            Assert.NotNull(savedRoom);
            Assert.Equal("Hero", savedRoom.Player.Name);
            Assert.Equal("Orc", savedRoom.Ennemie.Name);
        }

        //test 6 : verifie que la suppression d’un joueur fonctionne
        [Fact]
        public void Test_SuppressionPlayer_Fonctionne()
        {
            var context = GetDbContext();
            var player = new Player { Name = "TempPlayer", PV = 100, ATQ = 18, Score = 0 };
            context.Players.Add(player);
            context.SaveChanges();

            context.Players.Remove(player);
            context.SaveChanges();

            Assert.Null(context.Players.FirstOrDefault(p => p.Name == "TempPlayer"));
        }
    }
}
