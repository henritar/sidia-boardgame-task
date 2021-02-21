using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    //List of players whom the camera is looking at
    [SerializeField] private List<Transform> lookAt = default;

    //offset so the camera can be positioned correctly
    private Vector3 offset_player1 = default;
    private Vector3 offset_player2 = default;

    public float speed = 0.125f;
    public bool playerOneTurn = true;

    private BoardManager _board = default;

    private void Start()
    {
        offset_player1 = new Vector3(0, 6, -5);
        offset_player2 = new Vector3(5, 6, 0);
        _board = GameObject.Find("GameBoard").GetComponent<BoardManager>();
    }

    private void Update()
    {
        playerOneTurn = _board.playerOneTurn;
    }

    // Update is called once per frame
    private void LateUpdate()
    {

        //change camera to the player who is able to move in that turn
        if (playerOneTurn)
        {

        Vector3 position = lookAt[0].position + offset_player1;
        Vector3 finalPosition = Vector3.Lerp(transform.position, position, speed);
        transform.position = lookAt[0].position + offset_player1;

        transform.LookAt(lookAt[0]);
        }
        else
        {
            Vector3 position = lookAt[1].position + offset_player2;
            Vector3 finalPosition = Vector3.Lerp(transform.position, position, speed);
            transform.position = lookAt[1].position + offset_player2;

            transform.LookAt(lookAt[1]);
        }
    }
}
