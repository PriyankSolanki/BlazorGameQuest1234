namespace SharedModels;

public class Player :  Charactere
{
    public int Score { get; set; }
    
    public Player() { }

    public Player(int id, string name, int pv, int atq, int score) : base(id, name, pv, atq)
    {
        Score = score;
    }
}
