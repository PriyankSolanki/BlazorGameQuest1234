namespace SharedModels
{
    public class Charactere
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PV { get; set; }
        public int ATQ { get; set; }
        
        public Charactere() { }

        public Charactere(int id, string name, int pv, int atq)
        {
            Id = id;
            Name = name;
            PV = pv;
            ATQ = atq;
        }
    }
}
