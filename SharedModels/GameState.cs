using System.ComponentModel.DataAnnotations.Schema;
namespace SharedModels
{
    [NotMapped]
    public class GameState
    {
        public string DungeonId { get; set; } = "";
        public string CurrentRoomId { get; set; } = "";

        public Player Player { get; set; } = new Player(0, "Hero", 100, 20, 0);
        public int Score { get; set; } = 0;
        public bool IsGameOver { get; set; } = false;

       
        public int RoomsVisited { get; set; } = 0;
        public int RoomsLimit { get; set; } = 5;  

      
        public int PathIndex { get; set; } = 0;
    }
}
