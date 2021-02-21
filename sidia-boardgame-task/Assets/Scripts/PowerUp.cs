using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pieces
{

    [SerializeField] int id;


    private void OnCollisionEnter(Collision collision)
    {
        
        
        if (collision.gameObject.tag.Contains("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (id == 0)
            {
                player.ExtraAtkPowerUp();
            }
            else if (id == 1)
            {
                player.ExtraMovePowerUp();
            }
            else if (id == 2)
            {
                player.RecoverPowerUp();
            }

            Destroy(this.gameObject);
        }
    }
}
