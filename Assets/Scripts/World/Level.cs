
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Level : GameState
{
    public string LevelID { get; private set; }
    public HashSet<string> flags = new();

    public Level(string LevelID, GameStatemachine machine) : base(machine)
    {
        this.machine = machine;
        this.LevelID = LevelID;
    }

    public void SetFlag(string flag) 
    { 
        flags.Add(flag);
    }
    public bool HasFlag(string flag) 
    { 
        return flags.Contains(flag);
    }
    public void ResetFlags() 
    { 
        flags.Clear();
    }
}

