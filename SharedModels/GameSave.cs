namespace SharedModels
{
    public class GameSave
    {
        public int Id { get; set; }  
        public string PlayerName { get; set; } = "";
        public int FinalScore { get; set; }
        public int FinalPV { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool Victory { get; set; } = false;
        public int RoomsExplored { get; set; }
        public int RoomsTotal { get; set; }

       
        public GameState? LastState { get; set; }
    }
}
