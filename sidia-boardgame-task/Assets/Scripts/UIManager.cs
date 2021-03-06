﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static int[] diceResults = new int[6];
   
    //Elements used in UI
    public int movementPlayer1;
    public int movementPlayer2;
    public bool playerOneWin;
    public Text playerOneDiceResults;
    public Text playerTwoDiceResults;
    public Text BattleResult;
    public Text movementText;
    public Text p1Health;
    public Text p2Health;
    public Text p1Power;
    public Text p2Power;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject battleScreen;
    public GameObject HUD;
    public GameObject pause;
    public Button start;
    public Button quit;
    public Button replay;
    public Button mainMenu;
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
        replay.onClick.AddListener(_gameManager.RestartGame);
        mainMenu.onClick.AddListener(ReturnToMainMenu);
    }

    private void Update()
    {
        //Update Health, Power and Movement counters in Screen
        UpdateHUD();
        
    }

    public void ReturnToMainMenu()
    {
        _gameManager.RestartGame();
        ShowTitleScreen();
    }


    public void UpdateHUD()
    {
        p1Health.text = "P1 HEALTH: " + _players[0].GetCurrentHealth();
        p2Health.text = "P2 HEALTH: " + _players[1].GetCurrentHealth();
        p1Power.text = "P1 Power: " + _players[0].GetCurrentPower();
        p2Power.text = "P2 Power: " + _players[1].GetCurrentPower();


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
        //Show the result get from the dices rolls
        playerOneDiceResults.text = "Player 1 Results: " + diceResults[0] + " " + diceResults[1] + " " + diceResults[2];
        playerTwoDiceResults.text = "Player 2 Results: " + diceResults[3] + " " + diceResults[4] + " " + diceResults[5];

    }

    public void ShowBattleResult()
    {
        //Final score from dices comparison
        int p1Score = 0;
        int p2Score = 0;


        UpdateDicesResults();

        //Array to List so it is able to sort and compare each die result 
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
                //If dices results are equals, the turn player wins the comparison
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
            playerOneWin = true;
        }
        else
        {
            BattleResult.text = "PLAYER 2 WINS!";
            playerOneWin = false;
        }

        //Set game state to battle end
        _gameManager.SetGameState(4);

       

    }

 
    //Reset the text values and do damage to the loser player
    public void ResetBattle()
    {
        bool isGameOver = false;
        if (playerOneWin)
        {
            _players[1].TakeDamage(_players[0].GetCurrentPower());
            if (_players[1].GetCurrentHealth() < 1)
            {
                isGameOver = true;
            }
        }
        else
        {
            _players[0].TakeDamage(_players[1].GetCurrentPower());
            if (_players[0].GetCurrentHealth() < 1)
            {
                isGameOver = true;
            }
        }

        //If game is over, show GameOver screen
        if (isGameOver)
        {
            _gameManager.SetGameState(0);
            playerOneDiceResults.text = "";
            playerTwoDiceResults.text = "";
        }
        else
        {
            BattleResult.text = "";
            playerOneDiceResults.text = "Player 1 Results: ";
            playerTwoDiceResults.text = "Player 2 Results: ";
        }
    }

    //THE FOLLOWING METHODS ACTIVATE AND DEACTIVATE ALL THE MENUS SCREEN AND CONTROLS WHICH GAMEOBJECT MUST BE ACTIVATED OR NOT

    //Show game over screen by deativating gameboard and dicebox.
    public void ShowGameOverScreen()
    {
        _gameBoard.SetActive(false);
        _diceBox.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    //Hide Game Over screen
    public void HideGameOverScreen()
    {
        BattleResult.text = "";
        HideBattleScreen();
        gameOverScreen.SetActive(false);
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

    //Pause the game and turn off music
    public void PauseGame() 
    { 
        Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;
        pause.SetActive(true);
        _gameBoard.SetActive(false);
        HUD.SetActive(false);
    }


    //Resume the game and turn on the music
    public void ResumeGame()
    {
        Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;
        pause.SetActive(false);
        _gameBoard.SetActive(true);
        HUD.SetActive(true);
    }
}
