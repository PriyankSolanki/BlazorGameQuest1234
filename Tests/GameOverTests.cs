using Xunit;
using SharedModels;
using GameServices.Services;
using GameServices.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class GameOverTests
    {
        private readonly IDungeonStore store;
        private readonly GameController controller;

        public GameOverTests()
        {
            store = new InMemoryDungeonStore();
            controller = new GameController(store);
        }

        private GameState Prepare(GameState s)
        {
            var d = new Dungeon();

            string id = Guid.NewGuid().ToString();
            d.StartRoomId = id;
            d.ExitRoomId = id;
            d.Path.Add(id);

            d.Rooms[id] = new ProceduralRoom { Id = id, Type = RoomType.Empty };
            store.Save(d);

            s.DungeonId = d.Id;
            s.CurrentRoomId = id;
            return s;
        }

        private GameState Run(GameState s)
        {
            var result = controller.DoAction(new GameActionRequest
            {
                State = s,
                Action = "explorer"
            });

            Assert.IsType<OkObjectResult>(result.Result);
            return (GameState)((OkObjectResult)result.Result!).Value!;
        }

        [Fact]
        public void Ends_WhenHPZero()
        {
            var s = Prepare(new GameState
            {
                Player = new Player(0, "Test", 0, 5, 0)
            });

            var updated = Run(s);
            Assert.True(updated.IsGameOver);
        }

        [Fact]
        public void Ends_WhenScoreNegative()
        {
            var s = Prepare(new GameState
            {
                Player = new Player(0, "Test", 100, 5, 0),
                Score = -1
            });

            var updated = Run(s);
            Assert.True(updated.IsGameOver);
        }
    }
}
