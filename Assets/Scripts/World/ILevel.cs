
using System;

public interface ILevel
{
    public enum Area
    {
        Ancient_Path,
        Verdant_Grottos,
        Fogvale
    }
    public Area CurrentArea { get; set; }
    [Flags]
    public enum LevelFlags { }; 

}

