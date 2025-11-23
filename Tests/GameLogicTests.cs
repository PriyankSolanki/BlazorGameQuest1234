using Xunit;
using SharedModels;
using GameServices.Services;
using GameServices.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class GameLogicTests
    {
        private readonly IDungeonStore store;
        private readonly GameController controller;

        public GameLogicTests()
        {
            store = new InMemoryDungeonStore();
            controller = new GameController(store);
        }

        private (GameState, ProceduralRoom) CreateRoom(RoomType type)
        {
            var d = new Dungeon();
            string id = Guid.NewGuid().ToString();

            d.StartRoomId = id;
            d.ExitRoomId = id;
            d.Path.Add(id);

            var room = new ProceduralRoom
            {
                Id = id,
                Type = type,
                EnemyHP = 20,
                Level = 1,
                AvailableActions = new() { "test" }
            };

            d.Rooms[id] = room;
            store.Save(d);

            var state = new GameState
            {
                DungeonId = d.Id,
                CurrentRoomId = id,
                Player = new Player(0, "Test", 100, 5, 0),
                Score = 10
            };

            return (state, room);
        }

        private GameState Run(GameState s, string action)
        {
            var result = controller.DoAction(
                new GameActionRequest { State = s, Action = action });

            Assert.IsType<OkObjectResult>(result.Result);
            return (GameState)((OkObjectResult)result.Result!).Value!;
        }

        [Fact]
        public void Enemy_Combat_ShouldModify()
        {
            var (s, _) = CreateRoom(RoomType.Enemy);
            var updated = Run(s, "combattre");

            Assert.NotNull(updated);
        }

        [Fact]
        public void Heal_ShouldIncreaseHP()
        {
            var (s, _) = CreateRoom(RoomType.Heal);
            s.Player.PV = 50;

            var updated = Run(s, "boire");

            Assert.True(updated.Player.PV >= 50);
        }

        [Fact]
        public void Treasure_ShouldChangeScore()
        {
            var (s, _) = CreateRoom(RoomType.Treasure);
            var updated = Run(s, "ouvrir");

            Assert.NotNull(updated);
        }

        [Fact]
        public void Empty_ShouldIncreaseScore()
        {
            var (s, _) = CreateRoom(RoomType.Empty);
            var updated = Run(s, "explorer");

            Assert.True(updated.Score >= 10);
        }
    }
}
