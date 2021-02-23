using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int gameState = 1; //-1 = Initial state; 0 = game over; 1 = game running; 2 = game paused; 3 =  battle; 4 = end Battle;
    private UIManager _uiManager = default;
    private GameObject _gameBoard = default;
    private GameObject _diceBox = default;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameBoard = GameObject.Find("GameBoard");
        _diceBox = GameObject.Find("DiceBox");

        gameState = -1;

        _diceBox.SetActive(false);
        _gameBoard.SetActive(false);

    }

    private void Update()
    {
        //if game over, show game over screen
        if(gameState == 0)
        {
            GameOver();
        }
        //if the game is running and the return key is pressed, pause the game
        else if (gameState == 1){

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _uiManager.PauseGame();
                gameState = 2;
            }
        }
        //if the game is paused and the return key is pressed, resume the game
        else if (gameState == 2)
        {
            if (Input.GetKeyDown(KeyCode.Return)){
                _uiManager.ResumeGame();
                gameState = 1;
            }
        }
        //If battle state, trigger battle event
        else if(gameState == 3)
        {
            EnterBattle();
        }
        //If battle end state, wait the players to press return key to return to the board (gamestate 1). If Game Over state, go to first if statement
        else if(gameState == 4)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _uiManager.ResetBattle();
                if(gameState != 0)
                {
                    StartGame();
                }
            }
        }
    }

    public void StartGame()
    {
        
        gameState = 1;
        _uiManager.HideTitleScreen();
        _uiManager.HideBattleScreen();
       
    }

    private void EnterBattle()
    {
        
        gameState = 3;
        _uiManager.ShowBattleScreen();
    }

    public void GameOver()
    {
        _uiManager.ShowGameOverScreen();
    }

    public void RestartGame()
    {
        _gameBoard.GetComponent<BoardManager>().RestartBoard();
        _uiManager.HideGameOverScreen();
        gameState = 1;
    }

    public int GetGameState()
    {
        return gameState;
    }

    public void SetGameState(int state)
    {
        //Set game state to game over if any impossible value is given
        if(state < 0 || state > 4)
        {
            state = 0;
        }
        gameState = state;
    }
}
