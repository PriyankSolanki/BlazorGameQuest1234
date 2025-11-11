using Microsoft.AspNetCore.Mvc;
using SharedModels;
using GameServices.Services;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IDungeonStore _store;
        private readonly Random _rng = new();

        public GameController(IDungeonStore store) => _store = store;

        private ProceduralRoom? GetCurrentRoom(GameState s, out Dungeon? d)
        {
            d = null;
            if (!_store.TryGet(s.DungeonId, out var dungeon)) return null;
            d = dungeon;
            return dungeon.Rooms.TryGetValue(s.CurrentRoomId, out var r) ? r : null;
        }

        private void GoNext(GameState s, Dungeon d)
        {
            
            s.PathIndex++;
            if (s.PathIndex >= d.Path.Count)
            {
                
                s.IsGameOver = true;
                return;
            }

            s.CurrentRoomId = d.Path[s.PathIndex];
            s.RoomsVisited++;
        }

        private void CheckEnd(GameState s, Dungeon d)
        {
            if (s.Player.PV <= 0 || s.Score < 0)
                s.IsGameOver = true;

            if (s.PathIndex >= d.Path.Count)
                s.IsGameOver = true;

            if (s.RoomsVisited >= Math.Min(s.RoomsLimit, d.Path.Count))
                s.IsGameOver = true;
        }

        [HttpPost("action")]
        public ActionResult<GameState> DoAction([FromBody] GameActionRequest req)
        {
            var state = req.State;
            var action = req.Action.ToLowerInvariant();
            var room = GetCurrentRoom(state, out var dungeon);
            if (room is null || dungeon is null) return Ok(state);
            if (state.IsGameOver) return Ok(state);

            switch (room.Type)
            {
                case RoomType.Enemy:
                    HandleEnemyRoom(state, dungeon, room, action);
                    break;
                case RoomType.Treasure:
                    HandleTreasureRoom(state, dungeon, room, action);
                    break;
                case RoomType.Trap:
                    HandleTrapRoom(state, dungeon, room, action);
                    break;
                case RoomType.Heal:
                    HandleHealRoom(state, dungeon, room, action);
                    break;
                case RoomType.Empty:
                    HandleEmptyRoom(state, dungeon, room, action);
                    break;
                case RoomType.Exit:
                    state.IsGameOver = true;
                    break;
            }

            CheckEnd(state, dungeon);
            return Ok(state);
        }

        private void HandleEnemyRoom(GameState s, Dungeon d, ProceduralRoom room, string action)
        {
            room.EnemyHP ??= 25 + 5 * room.Level;

            if (room.Cleared)
            {
                
                GoNext(s, d);
                return;
            }

            switch (action)
            {
                case "combattre":
                    int playerDmg = _rng.Next(10, 20);
                    int enemyDmg = _rng.Next(5, 15);

                    room.EnemyHP -= playerDmg;

                    if (room.EnemyHP <= 0)
                    {
                        s.Score += 15;
                        room.Cleared = true;
                        GoNext(s, d);
                        return;
                    }

                   
                    s.Player.PV -= enemyDmg;
                    s.Score -= 2;
                    break;

                case "fouiller":
                    if (_rng.NextDouble() < 0.5)
                        s.Score += 10;
                    else
                        s.Player.PV -= _rng.Next(5, 11);
                    room.Cleared = true;
                    GoNext(s, d);
                    break;

                case "fuir":
                    s.Score -= 5;
                    room.Cleared = true; 
                    GoNext(s, d);
                    break;
            }
        }


        private void HandleTreasureRoom(GameState s, Dungeon d, ProceduralRoom room, string action)
        {
            if (room.Cleared) { GoNext(s, d); return; }

            switch (action)
            {
                case "ouvrir":
                    if (_rng.NextDouble() < 0.7)
                        s.Score += _rng.Next(15, 31);
                    else
                        s.Player.PV -= _rng.Next(10, 21);
                    room.Cleared = true;
                    GoNext(s, d);
                    break;
                case "ignorer":
                    s.Score += 1;
                    room.Cleared = true;
                    GoNext(s, d);
                    break;
            }
        }

        private void HandleTrapRoom(GameState s, Dungeon d, ProceduralRoom room, string action)
        {
            if (room.Cleared) { GoNext(s, d); return; }

            if (action == "désamorcer")
            {
                if (_rng.NextDouble() < 0.5)
                {
                    s.Score += 10;
                }
                else
                {
                    s.Player.PV -= _rng.Next(10, 26);
                    s.Score -= 5;
                }
            }
            else if (action == "fuir")
            {
                s.Score -= 2;
            }

            room.Cleared = true;
            GoNext(s, d);
        }

        private void HandleHealRoom(GameState s, Dungeon d, ProceduralRoom room, string action)
        {
            if (room.Cleared) { GoNext(s, d); return; }

            if (action == "boire")
            {
                s.Player.PV = Math.Min(100, s.Player.PV + 20);
                s.Score += 3;
            }
            GoNext(s, d);
            room.Cleared = true;
        }

        private void HandleEmptyRoom(GameState s, Dungeon d, ProceduralRoom room, string action)
        {
            if (room.Cleared) { GoNext(s, d); return; }

            s.Score += 1;
            room.Cleared = true;
            GoNext(s, d);
        }
    }

    public class GameActionRequest
    {
        public GameState State { get; set; } = new();
        public string Action { get; set; } = "";
    }
}
