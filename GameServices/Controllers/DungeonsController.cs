using Microsoft.AspNetCore.Mvc;
using SharedModels;
using GameServices.Services;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DungeonsController : ControllerBase
    {
        private readonly IDungeonStore _store;
        private readonly Random _rng = new();

        public DungeonsController(IDungeonStore store) => _store = store;

        [HttpGet("generate")]
        public ActionResult<Dungeon> Generate([FromQuery] int rooms = 4)
        {
          
            rooms = 6;

            var dungeon = new Dungeon();
            var rng = new Random();

            var pool = new List<RoomType> { RoomType.Enemy, RoomType.Treasure, RoomType.Trap, RoomType.Heal, RoomType.Empty };
            //melange
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (pool[i], pool[j]) = (pool[j], pool[i]);
            }
            var chosen = pool.Take(rooms).ToList();

            string firstId = Guid.NewGuid().ToString();
            var first = CreateRoom(chosen[0]);
            first.Id = firstId;
            dungeon.Rooms[firstId] = first;
            dungeon.Path.Add(firstId);
            dungeon.StartRoomId = firstId;

            string prevId = firstId;

            for (int i = 1; i < chosen.Count; i++)
            {
                string id = Guid.NewGuid().ToString();
                var room = CreateRoom(chosen[i]);
                room.Id = id;
                room.Neighbors.Add(prevId);
                dungeon.Rooms[prevId].Neighbors.Add(id);

                dungeon.Rooms[id] = room;
                dungeon.Path.Add(id);
                prevId = id;
            }

            //derniere = sortie
            dungeon.ExitRoomId = prevId;

            _store.Save(dungeon);
            return Ok(dungeon);
        }

        private ProceduralRoom CreateRoom(RoomType type)
        {
            return type switch
            {
                RoomType.Enemy => new ProceduralRoom
                {
                    Type = type,
                    Title = "Un gobelin apparaît !",
                    Description = "Il grince des dents et fonce sur toi.",
                    AvailableActions = new() { "Combattre", "Fouiller", "Fuir" },
                    EnemyHP = 30,
                    Level = 1
                },
                RoomType.Treasure => new ProceduralRoom
                {
                    Type = type,
                    Title = "Un coffre mystérieux",
                    Description = "Vieil acier, serrure rouillée. Trésor… ou piège ?",
                    AvailableActions = new() { "Ouvrir", "Ignorer" }
                },
                RoomType.Trap => new ProceduralRoom
                {
                    Type = type,
                    Title = "Un couloir piégé",
                    Description = "Des dalles instables, des mécanismes dissimulés.",
                    AvailableActions = new() { "Désamorcer", "Fuir" }
                },
                RoomType.Heal => new ProceduralRoom
                {
                    Type = type,
                    Title = "Une fontaine lumineuse",
                    Description = "L’eau semble infuser une douce chaleur.",
                    AvailableActions = new() { "Boire", "Continuer" }
                },
                _ => new ProceduralRoom
                {
                    Type = RoomType.Empty,
                    Title = "Une salle vide",
                    Description = "Le silence. Peut-être rien… ou quelque chose t’échappe.",
                    AvailableActions = new() { "Explorer", "Continuer" }
                },
            };
        }

    }
}
