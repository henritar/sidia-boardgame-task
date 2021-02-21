using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pieces : MonoBehaviour
{

    public int CurrentX { set; get; }
    public int CurrentZ { get; set; }

    public void SetPosition(int x, int z)
    {
        CurrentX = x;
        CurrentZ = z;
    }

}
