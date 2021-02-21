using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int movementPlayer1;
    public int movementPlayer2;
    public Text movementText;
    public GameObject titleScreen;
    public Button start;
    public Button quit;
    private GameManager _gameManager = default;
    private BoardManager _board = default;
    private GameObject _gameBoard = default;
    [SerializeField] private List<Player> _players = default;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameBoard = GameObject.Find("GameBoard");
        _board = _gameBoard.GetComponent<BoardManager>();
        
        //Menu buttons
        start.onClick.AddListener(_gameManager.startGame);
        quit.onClick.AddListener(Application.Quit);
    }

    private void Update()
    {
        UpdateMovement();
    }


    public void UpdateMovement()
    {
        //update on screen how many moves the current player has
        if (_board.playerOneTurn)
        {
            movementText.text = "Movements: " + _players[0].GetMovesAvailable();

        }
        else{
            movementText.text = "Movements: " + _players[1].GetMovesAvailable();
        }

    }


    public void hideTitleScreen()
    {
        _gameBoard.SetActive(true);
        titleScreen.SetActive(false);
    }

    public void showTitleScreen()
    {
        _gameBoard.SetActive(false);
        titleScreen.SetActive(true);

    }
}
