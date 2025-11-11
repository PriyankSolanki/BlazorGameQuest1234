using System.Collections.Generic;

namespace SharedModels
{
    public class ProceduralRoom
    {
        public string Id { get; set; } = "";
        public RoomType Type { get; set; } = RoomType.Empty;
        public int Level { get; set; } = 1;
        public List<string> Neighbors { get; set; } = new();

        public int? EnemyHP { get; set; }
        public bool Cleared { get; set; }
        public bool LootTaken { get; set; }

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public List<string> AvailableActions { get; set; } = new();
    }
}
