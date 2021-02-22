using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static int[] diceResults = new int[6];
   
    public int movementPlayer1;
    public int movementPlayer2;
    public Text playerOneDiceResults;
    public Text playerTwoDiceResults;
    public Text BattleResult;
    public Text movementText;
    public GameObject titleScreen;
    public GameObject battleScreen;
    public GameObject HUD;
    public Button start;
    public Button quit;
    private GameManager _gameManager = default;
    private BoardManager _board = default;
    private GameObject _gameBoard = default;
    private GameObject _diceBox = default;
    [SerializeField] private List<Player> _players = default;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameBoard = GameObject.Find("GameBoard");
        _board = _gameBoard.GetComponent<BoardManager>();
        _diceBox = GameObject.Find("DiceBox");

        //Menu buttons
        start.onClick.AddListener(_gameManager.StartGame);
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

    public void UpdateDicesResults()
    {
        //update on screen how many moves the current player has
        playerOneDiceResults.text = "Player 1 Results: " + diceResults[0] + " " + diceResults[1] + " " + diceResults[2];
        playerTwoDiceResults.text = "Player 2 Results: " + diceResults[3] + " " + diceResults[4] + " " + diceResults[5];

    }

    public void ShowBattleResult()
    {
        int p1Score = 0;
        int p2Score = 0;
        UpdateDicesResults();
        List<int> p1DicesResultList = new List<int>();
        p1DicesResultList.Add(diceResults[0]);
        p1DicesResultList.Add(diceResults[1]);
        p1DicesResultList.Add(diceResults[2]);

        List<int> p2DicesResultList = new List<int>();
        p2DicesResultList.Add(diceResults[3]);
        p2DicesResultList.Add(diceResults[4]);
        p2DicesResultList.Add(diceResults[5]);

        p1DicesResultList.Sort();
        p2DicesResultList.Sort();

        for(int i = 0; i < 3; i++)
        {
            if(p1DicesResultList[i] > p2DicesResultList[i])
            {
                p1Score++;
            }
            else if(p1DicesResultList[i] < p2DicesResultList[i])
            {
                p2Score++;
            }
            else
            {
                if (_board.playerOneTurn)
                {
                    p1Score++;
                }
                else
                {
                    p2Score++;
                }
            }
        }

        if(p1Score > p2Score)
        {
            BattleResult.text = "PLAYER 1 WINS!";
        }
        else if(p1Score < p2Score)
        {
            BattleResult.text = "PLAYER 2 WINS!";
        }
        else
        {
            if (_board.playerOneTurn)
            {
                BattleResult.text = "PLAYER 1 WINS!";
            }
            else
            {
                BattleResult.text = "PLAYER 2 WINS!";
            }
        }

        _gameManager.SetGameState(4);

    }

    public void ResetBattle()
    {
        BattleResult.text = "";
        playerOneDiceResults.text = "Player 1 Results: ";
        playerTwoDiceResults.text = "Player 2 Results: ";
    }

    public void HideTitleScreen()
    {
        _gameBoard.SetActive(true);
        titleScreen.SetActive(false);
        HUD.SetActive(true);
    }

    public void ShowTitleScreen()
    {
        _gameBoard.SetActive(false);
        titleScreen.SetActive(true);
        HUD.SetActive(false);

    }

    public void ShowBattleScreen()
    {
        _gameBoard.SetActive(false);
        battleScreen.SetActive(true);
        _diceBox.SetActive(true);
        HUD.SetActive(false);

    }

    public void HideBattleScreen()
    {
        _gameBoard.SetActive(true);
        _diceBox.SetActive(false);
        battleScreen.SetActive(false);
        HUD.SetActive(true);
    }
}
