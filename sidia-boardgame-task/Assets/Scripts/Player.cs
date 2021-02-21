using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Pieces
{
    private const int MAX_HEALTH = 20;
    private const int RECOVER_POWER_UP_VALUE = 2;

    [SerializeField] private int _health = 10;
    [SerializeField] private int _power = 2;

    [SerializeField] private int _movesAvailable = 3;

    public virtual bool LegalMoves(int x, int z)
    {
        if(CurrentZ != z || CurrentX != x){
            if(x >= 0 && z >= 0 && Mathf.Abs(CurrentX - x) < 2 && Mathf.Abs(CurrentZ - z) < 2 && Mathf.Abs(CurrentX - x) != Mathf.Abs(CurrentZ -z) )
            {
                return true;

            }
        }
              
        return false;
        
    }

    public void ExtraAtkPowerUp()
    {
        _power++;
    }

    public void ExtraMovePowerUp()
    {
        _movesAvailable++;
    }

    public void RecoverPowerUp()
    {
        if(_health + RECOVER_POWER_UP_VALUE < MAX_HEALTH)
        {
            _health += 2;
        }
        else
        {
            _health = MAX_HEALTH;
        }
    }

    public int GetMovesAvailable()
    {
        return _movesAvailable;
    }

    public void MovePlayer(Vector3 position)
    {
        transform.position = position;
        StartCoroutine(VerifyAvailablesMoves());
    }

    public void Reset()
    {
        _movesAvailable = 3;
        _power = 2;
    }


    IEnumerator VerifyAvailablesMoves()
    {
        
        yield return new WaitForSeconds(0.012f);
        _movesAvailable--;
    }

}
