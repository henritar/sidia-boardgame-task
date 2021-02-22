using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    //Default size of the gameboard
    private const float BOARD_DEFAULT_SIZE = 16.0f;
    private const float TITLE_SIZE = 1.0f;
    private const float TITLE_OFFSET = 0.5f;
    private const float POWER_UP_MAX = BOARD_DEFAULT_SIZE * BOARD_DEFAULT_SIZE - 2;

    //Selected coordenates
    private int _tileSelectedX = -1;
    private int _tileSelectedZ = -1;

    //Active player turn
    private Player playerAbleToAct = default;
    [SerializeField] private int powerupsCounter;

    //List of powerup prefebs to initialize them on the field
    public List<GameObject> powerUpsPrefabs = default;

    private GameManager _gameManager = default;

    //Map to know which prefab is occupying which tile
    public Pieces[,] PiecesMap { set; get; }
     
    //Boolean which indicates whose turn is
    public bool playerOneTurn = true;


    void Start()
    {
        PiecesMap = new Pieces[(int)BOARD_DEFAULT_SIZE, (int)BOARD_DEFAULT_SIZE];

        _gameManager = GameObject.Find("GameManager").GetComponent <GameManager> ();

        //Set players to start position
        updatePlayers();
        //Instatiate each powerup randomly
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

        if(powerupsCounter < POWER_UP_MAX * 0.1)
        {
            int locationX = Random.Range(0, (int)BOARD_DEFAULT_SIZE);
            int locationZ = Random.Range(0, (int)BOARD_DEFAULT_SIZE);

            if(PiecesMap[locationX, locationZ] == null)
            {
                powerupsCounter++;
                int randomPowerUp = Random.Range(0, 3);
                GameObject powerUp = Instantiate(powerUpsPrefabs[randomPowerUp], GetTitleCenterPosition(locationX, locationZ, 0.15f), Quaternion.identity) as GameObject;
                powerUp.transform.SetParent(transform);
                PiecesMap[locationX, locationZ] = powerUp.GetComponent<Pieces>();
                PiecesMap[locationX, locationZ].SetPosition(locationX, locationZ);
            }
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

        player1.GetComponent<Player>().RestartGame();
        player2.GetComponent<Player>().RestartGame();

        player1.transform.position = GetTitleCenterPosition(0, 0, 0.5f);
        player2.transform.position = GetTitleCenterPosition(lastTile, lastTile, 0.5f);

        //Set them into the map so we are able to know which tite is occupied
        PiecesMap[0, 0] = player1.GetComponent<Pieces>();
        PiecesMap[0, 0].SetPosition(0, 0);
        PiecesMap[lastTile, lastTile] = player2.GetComponent<Pieces>();
        PiecesMap[lastTile, lastTile].SetPosition(lastTile, lastTile);
    }

 
    private void SpawnPowerUps()
    {
        powerupsCounter = 0;
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
                powerupsCounter++;
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
            Player player;
            //Verify if the player are able to move to target tile
            if (playerAbleToAct.LegalMoves(_tileSelectedX, _tileSelectedZ) && (PiecesMap[_tileSelectedX, _tileSelectedZ] == null || !PiecesMap[_tileSelectedX, _tileSelectedZ].tag.Contains("Player")))
            {
                player = playerAbleToAct.GetComponent<Player>();
                //Update location map
                PiecesMap[playerAbleToAct.CurrentX, playerAbleToAct.CurrentZ] = null;
                
                player.MovePlayer(GetTitleCenterPosition(_tileSelectedX, _tileSelectedZ, 0.5f));
                PiecesMap[_tileSelectedX, _tileSelectedZ] = playerAbleToAct;
                PiecesMap[_tileSelectedX, _tileSelectedZ].SetPosition(_tileSelectedX, _tileSelectedZ);
            }
            else
            {
                return;
            }
            if (SearchForNearbyPlayer(player.transform.position))
            {
                
                _gameManager.SetGameState(3);
                Debug.Log("FOUND A PLAYER NEARBY!");
            }
        }
    }


    private void SelectPlayerTurn()
    {
        //Change player turn
        if (playerOneTurn)
        {
            playerAbleToAct = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        }
        else
        {
            playerAbleToAct = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();
        }
    }

    private bool SearchForNearbyPlayer(Vector3 playerPostion)
    {
        
        //Player one verifies if there is another player nearby
        if (playerOneTurn)
        {
            int right = (int)playerPostion.x + 1;
            int left = (int)playerPostion.x - 1;
            int up = (int)playerPostion.z + 1;
            int down = (int)playerPostion.z - 1;

            if ((right <= BOARD_DEFAULT_SIZE - 1 && PiecesMap[right, (int)playerPostion.z] != null && PiecesMap[right, (int)playerPostion.z].tag.Contains("Player")) || (left >= 0 && PiecesMap[left, (int)playerPostion.z] != null && PiecesMap[left, (int)playerPostion.z].tag.Contains("Player"))
            || (up <= BOARD_DEFAULT_SIZE - 1 && PiecesMap[(int)playerPostion.x, up] != null && PiecesMap[(int)playerPostion.x, up].tag.Contains("Player")) || (down >= 0 && PiecesMap[(int)playerPostion.x, down] != null && PiecesMap[(int)playerPostion.x, down].tag.Contains("Player")))
            {
                return true;
            }

        }
        //Player one verifies if there is another player nearby (Two different codes because player 2 starts at the opposite edge of the field (15,15))
        else
        {
            int down = (int)playerPostion.x + 1;
            int up = (int)playerPostion.x - 1;
            int right = (int)playerPostion.z + 1;
            int left = (int)playerPostion.z - 1;

            if ((right <= BOARD_DEFAULT_SIZE - 1 && PiecesMap[(int)playerPostion.x, right] != null && PiecesMap[(int)playerPostion.x, right].tag.Contains("Player")) || (left >= 0 && PiecesMap[(int)playerPostion.x, left] != null && PiecesMap[(int)playerPostion.x, left].tag.Contains("Player"))
                 || (up >= 0 && PiecesMap[up, (int)playerPostion.z] != null && PiecesMap[up, (int)playerPostion.z].tag.Contains("Player")) || (down <= BOARD_DEFAULT_SIZE - 1 && PiecesMap[down, (int)playerPostion.z] != null && PiecesMap[down, (int)playerPostion.z].tag.Contains("Player")))
            {
                return true;
            }
        }

        return false;

    }

    public void SubPowerUps()
    {
        powerupsCounter--;
    }

    public void RestartBoard()
    {
        this.gameObject.SetActive(true);
        DestroyPowerUps();
        updatePlayers();
        SpawnPowerUps();
        
    }

    public void DestroyPowerUps()
    {
        for (int i = 0; i < BOARD_DEFAULT_SIZE; i++)
        {
            for (int j = 0; j < BOARD_DEFAULT_SIZE; j++)
            {
                if(PiecesMap[i, j] != null && PiecesMap[i,j].gameObject.tag == "PowerUp")
                {
                    SubPowerUps();
                    Destroy(PiecesMap[i, j].gameObject);
                    PiecesMap[i, j] = null;
                }
            }
        }
    }
}
