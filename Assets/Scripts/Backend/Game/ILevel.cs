
public interface ILevel
{
    public enum Area
    {
        Ancient_Path,
        Verdant_Grottos,
        Fogvale
    }
    public Area CurrentArea { get; set; }
    public bool[] Flags { get; set; }

}

