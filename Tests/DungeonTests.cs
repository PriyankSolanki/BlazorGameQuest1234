using Xunit;
using SharedModels;
using GameServices.Services;
using GameServices.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class DungeonTests
    {
        private readonly IDungeonStore store;
        private readonly DungeonsController controller;

        public DungeonTests()
        {
            store = new InMemoryDungeonStore();
            controller = new DungeonsController(store);
        }

        private Dungeon GetDungeon()
        {
            var result = controller.Generate();
            Assert.IsType<OkObjectResult>(result.Result);

            return (Dungeon)((OkObjectResult)result.Result!).Value!;
        }

        [Fact]
        public void Generate_ShouldReturnDungeon()
        {
            var d = GetDungeon();
            Assert.NotNull(d);
        }

        [Fact]
        public void Generate_ShouldSaveDungeon()
        {
            var d = GetDungeon();
            Assert.True(store.TryGet(d.Id, out var stored));
        }

        [Fact]
        public void Generate_ShouldHaveStart()
        {
            var d = GetDungeon();
            Assert.False(string.IsNullOrEmpty(d.StartRoomId));
        }

        [Fact]
        public void Generate_ShouldHavePath()
        {
            var d = GetDungeon();
            Assert.True(d.Path.Count > 0);
        }
    }
}
