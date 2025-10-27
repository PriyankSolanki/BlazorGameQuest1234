namespace SharedModels;

public class Charactere
{
    public int Id { get; set; }
    public string name { get; set; }
    public int PV { get; set; }
    public int ATQ { get; set; }
    
    public Charactere() { }

    public Charactere(int id, string name, int pv, int atq)
    {
        Id = id;
        this.name = name;
        PV = pv;
        ATQ = atq;
    }
}