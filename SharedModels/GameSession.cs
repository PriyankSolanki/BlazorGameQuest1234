namespace SharedModels
{
    public class GameSession
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Score { get; set; }
        public DateTime DatePlayed { get; set; } = DateTime.UtcNow;
    }
}
