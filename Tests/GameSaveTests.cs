using Xunit;
using Microsoft.AspNetCore.Mvc;
using GameServices.Controllers;
using GameServices;
using SharedModels;
using Microsoft.EntityFrameworkCore;

namespace GameTests
{
    public class GameSaveTests
    {
        private AppDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void SaveGame_ShouldStore()
        {
            var db = CreateDb();
            var ctrl = new SavesController(db);

            var result = ctrl.Save(new GameSave { PlayerName = "Test", FinalScore = 50 });

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAll_ShouldReturnItems()
        {
            var db = CreateDb();
            db.GameSaves.Add(new GameSave { PlayerName = "A", FinalScore = 10 });
            db.SaveChanges();

            var ctrl = new SavesController(db);
            var result = ctrl.GetAll();

            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
