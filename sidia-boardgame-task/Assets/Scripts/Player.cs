using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Pieces
{
    [SerializeField] private int health = 10;
    [SerializeField] private int power = 2;


    public virtual bool LegalMoves(int x, int z)
    {
        if(x >= 0 && z >= 0)
        {
            return true;

        }
              
        return false;
        
    }

   
}
