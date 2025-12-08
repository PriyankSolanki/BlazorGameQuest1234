namespace SharedModels
{
    public class Player : Charactere
    {
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Legacy score (not used for leaderboard but kept)
        public int Score { get; set; }

        public Player() { }

        public Player(int id, string name, int pv, int atq, int score, string username = "")
            : base(id, name, pv, atq)
        {
            Score = score;
            Username = username;
            IsActive = true;
        }
    }
}
