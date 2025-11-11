using System.Collections.Concurrent;
using SharedModels;

namespace GameServices.Services
{
    public interface IDungeonStore
    {
        void Save(Dungeon dungeon);
        bool TryGet(string id, out Dungeon dungeon);
    }

    public class InMemoryDungeonStore : IDungeonStore
    {
        private static readonly ConcurrentDictionary<string, Dungeon> _store = new();

        public void Save(Dungeon dungeon) => _store[dungeon.Id] = dungeon;
        public bool TryGet(string id, out Dungeon dungeon) => _store.TryGetValue(id, out dungeon!);
    }
}
