using UnityEngine;

internal class AP1 : MonoBehaviour, ILevel
{
    public ILevel.Area CurrentArea { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool[] Flags { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        CurrentArea = ILevel.Area.Ancient_Path;
    }

}

