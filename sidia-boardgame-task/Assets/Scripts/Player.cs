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


    //Verify if the player selected a legal move (inside the board, adjacent tile and not diagonally)
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

    public int GetCurrentHealth()
    {
        return _health;
    }

    public int GetCurrentPower()
    {
        return _power;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
    }

    public void MovePlayer(Vector3 position)
    {
        transform.position = position;
        StartCoroutine(VerifyAvailablesMoves());
    }

    //Reset movement and power counters
    public void Reset()
    {
        _movesAvailable = 3;
        _power = 2;
    }

    //Restore player to default values
    public void RestartGame()
    {
        Reset();
        _health = 10;
    }

    //Coroutine to reduce player's movement counter
    IEnumerator VerifyAvailablesMoves()
    {
        
        yield return new WaitForSeconds(0.012f);
        _movesAvailable--;
    }

}
