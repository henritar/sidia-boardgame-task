using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pieces
{

    [SerializeField] int id = default;
    [SerializeField] private AudioClip _clip = default;

    private BoardManager _board = default;

    private void Start()
    {
        _board = GameObject.Find("GameBoard").GetComponent<BoardManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        //If collision is detected with some player, the power up call the correct method and them destroy itself
        if (collision.gameObject.tag.Contains("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);

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
            _board.SubPowerUps();
            Destroy(this.gameObject);
        }
    }
}
