namespace SharedModels;

public class Room
{
    public int Id { get; set; }
    public int Level { get; set; }
    public Player Player { get; set; }
    public Ennemie Ennemie { get; set; }
    
    public Room() { }

    public Room(int id, int level, Player player, Ennemie ennemie)
    {
        Id = id;
        Level = level;
        Player = player;
        Ennemie = ennemie;
    }
}