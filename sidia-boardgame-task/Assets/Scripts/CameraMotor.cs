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
    [SerializeField] private Vector3 offset_dice_box_p1 = default;
    [SerializeField] private Vector3 offset_dice_box_p2 = default;

    public float speed = 0.125f;
    public bool playerOneTurn = true;

    private BoardManager _board = default;
    private GameManager _gameManager = default;

    private void Start()
    {
        offset_player1 = new Vector3(0, 6, -5);
        offset_player2 = new Vector3(5, 6, 0);
        offset_dice_box_p1 = new Vector3(0, 7, -8);
        offset_dice_box_p2 = new Vector3(7, 7.5f, -0.8f);
        _board = GameObject.Find("GameBoard").GetComponent<BoardManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
            //Get which playeris the current turn player so the camera can follow the correct player
            playerOneTurn = _board.playerOneTurn;
    }

    // Update is called once per frame
    private void LateUpdate()
    {

        if (_gameManager.GetGameState() != 3 && _gameManager.GetGameState() != 4)
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
        else
        {
            //If game state is Battle State, show player's diceBox Perspective
            if (playerOneTurn)
            {
                transform.position = new Vector3(0.0f, -50.0f, 0.0f) + offset_dice_box_p1;

            }
            else
            {
                transform.position = new Vector3(0.0f, -50.0f, 0.0f) + offset_dice_box_p2;
            }
        }

    }
}
