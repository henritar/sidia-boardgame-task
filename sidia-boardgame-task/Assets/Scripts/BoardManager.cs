using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    //Default size of the gameboard
    private const float BOARD_DEFAULT_SIZE = 16.0f;
    private const float TITLE_SIZE = 1.0f;
    private const float TITLE_OFFSET = 0.5f;

    //Selectin coordenates
    private int _tileSelectedX = -1;
    private int _tileSelectedZ = -1;

    private Player playerAbleToAct;

    public List<GameObject> playersPrefabs;
    public List<GameObject> powerUpsPrefabs;

    public Pieces[,] PiecesMap { set; get; }
     

    public bool playerOneTurn = true;


    void Start()
    {
        PiecesMap = new Pieces[(int)BOARD_DEFAULT_SIZE, (int)BOARD_DEFAULT_SIZE];
        updatePlayers();
        SpawnPowerUps();
    }

    void Update()
    {
        //Method responsible for get the tile which the player is pointing to
        UpdateSelectionTile();
        //Method responsible for drawing the board boarders
        DrawBoardSkeleton();
        movePlayer();

        if(playerAbleToAct.GetComponent<Player>().GetMovesAvailable() <= 0)
        {
            playerAbleToAct.GetComponent<Player>().Reset();
            playerOneTurn = !playerOneTurn;
        }
    }

    private void UpdateSelectionTile()
    {
        //if there is no camera, nothing happens
        if (!Camera.main)
        {
            return;
        }

        //Get which title the mouse is poiting to
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("BoardPlane")))
        {
            _tileSelectedX = (int)hit.point.x;
            _tileSelectedZ = (int)hit.point.z;
        }
        else
        {
            _tileSelectedX = -1;
            _tileSelectedZ = -1;
        }
    }

    private void DrawBoardSkeleton()
    {
        Vector3 width = Vector3.right * BOARD_DEFAULT_SIZE;
        Vector3 height = Vector3.forward * BOARD_DEFAULT_SIZE;

        for(int i = 0; i <= BOARD_DEFAULT_SIZE; i++)
        {
            Vector3 initialPoint = Vector3.forward * i;
            Debug.DrawLine(initialPoint, initialPoint + width);
            
            for(int j = 0; j <= BOARD_DEFAULT_SIZE; j++)
            {
                initialPoint = Vector3.right * j;
                Debug.DrawLine(initialPoint, initialPoint + height);
            }
        }

        //Draw the lines that indicates which tile the player is pointing to
        if(_tileSelectedX >= 0 && _tileSelectedZ >= 0)
        {
            //Draw firts diagonal line
            Debug.DrawLine(Vector3.forward * _tileSelectedZ + Vector3.right * _tileSelectedX, Vector3.forward * (_tileSelectedZ + 1) + Vector3.right * (_tileSelectedX + 1));
            //Draw second diagonal line
            Debug.DrawLine(Vector3.forward * (_tileSelectedZ + 1) + Vector3.right * _tileSelectedX, Vector3.forward * _tileSelectedZ + Vector3.right * (_tileSelectedX + 1));
        }
    }

    private void updatePlayers()
    {
        int lastTile = (int)BOARD_DEFAULT_SIZE - 1;

        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

        player1.transform.SetParent(transform);
        player2.transform.SetParent(transform);

        //Set them into the map so we are able to know which tite is occupied
        PiecesMap[0, 0] = player1.GetComponent<Pieces>();
        PiecesMap[0, 0].SetPosition(0, 0);
        PiecesMap[lastTile, lastTile] = player2.GetComponent<Pieces>();
        PiecesMap[lastTile, lastTile].SetPosition(lastTile, lastTile);
    }

    //private void SpawnPlayers()
    //{
    //    int lastTile = (int)BOARD_DEFAULT_SIZE - 1;


    //    //Spawn players on first and last tile of the board
    //    GameObject player1 = Instantiate(playersPrefabs[0], GetTitleCenterPosition(0, 0, 0.5f), Quaternion.identity) as GameObject;
    //    GameObject player2 = Instantiate(playersPrefabs[1], GetTitleCenterPosition(lastTile, lastTile, 0.5f), Quaternion.identity) as GameObject;
       
    //    //Set players as child of the board so they can move altogether
    //    player1.transform.SetParent(transform);
    //    player2.transform.SetParent(transform);

    //    //Set them into the map so we are able to know which tite is occupied
    //    PiecesMap[0, 0] = player1.GetComponent<Pieces>();
    //    PiecesMap[0, 0].SetPosition(0, 0); 
    //    PiecesMap[lastTile, lastTile] = player2.GetComponent<Pieces>();
    //    PiecesMap[lastTile, lastTile].SetPosition(lastTile, lastTile);
    //}

    private void SpawnPowerUps()
    {
        //Spawn a powerup for each title except that ones where the players start
        for(int i = 0; i < BOARD_DEFAULT_SIZE; i++)
        {
            for (int j = 0; j < BOARD_DEFAULT_SIZE; j++)
            {
                
                //PowerUps are not able to spawn where the players start
                if((i == 0 && j == 0) || (i == BOARD_DEFAULT_SIZE - 1 && j == BOARD_DEFAULT_SIZE - 1))
                {
                    continue;
                }
                int randomPowerUp = Random.Range(0, 3);
                GameObject powerUp = Instantiate(powerUpsPrefabs[randomPowerUp], GetTitleCenterPosition(i, j, 0.15f), Quaternion.identity) as GameObject;
                powerUp.transform.SetParent(transform);
                PiecesMap[i, j] = powerUp.GetComponent<Pieces>();
                PiecesMap[i, j].SetPosition(i, j);
            }
        }
    }

    private Vector3 GetTitleCenterPosition(int x, int z, float height)
    {
        Vector3 origin = Vector3.zero;
        //get the center o the tile and the height of the object so it won't fly
        origin.x += (TITLE_SIZE * x) + TITLE_OFFSET;
        origin.y = height;
        origin.z += (TITLE_SIZE * z) + TITLE_OFFSET;
        return origin;

    }

    private void movePlayer()
    {
        //Select which player can move this turn
        SelectPlayerTurn();

        if (Input.GetMouseButtonDown(0))
        {
            
            
            if (playerAbleToAct.LegalMoves(_tileSelectedX, _tileSelectedZ))
            {
                PiecesMap[playerAbleToAct.CurrentX, playerAbleToAct.CurrentZ] = null;
                playerAbleToAct.GetComponent<Player>().MovePlayer(GetTitleCenterPosition(_tileSelectedX, _tileSelectedZ, 0.5f));
                PiecesMap[_tileSelectedX, _tileSelectedZ] = playerAbleToAct;
            }
            else
            {
                return;
            }
        }
    }


    private void SelectPlayerTurn()
    {
        if (playerOneTurn)
        {
            playerAbleToAct = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        }
        else
        {
            playerAbleToAct = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();
        }
    }
}
