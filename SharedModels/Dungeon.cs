namespace SharedModels
{
    public class Dungeon
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StartRoomId { get; set; } = "";
        public string ExitRoomId { get; set; } = "";

        
        public Dictionary<string, ProceduralRoom> Rooms { get; set; } = new();

        public List<string> Path { get; set; } = new();
    }
}
