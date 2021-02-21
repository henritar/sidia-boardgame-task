using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int gameState = 0; // 0 = game over; 1 = game running; 2 = game paused
    private UIManager _uiManager = default;
    private GameObject _gameBoard = default;
    [SerializeField] List<GameObject> _player = default;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameBoard = GameObject.Find("GameBoard");
        
    }

    private void Update()
    {
        //If game is not running
        if(gameState != 1)
        {
            //hide Gameboard
            _gameBoard.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startGame();
            }
        }
        //While game ins running, press enter to pause
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameState = 2;
            _uiManager.showTitleScreen();
        }
    }

    public void startGame()
    {
        
        gameState = 1;
        _uiManager.hideTitleScreen();
       
    }

    public void gameOver()
    {
        gameState = 0;
        _uiManager.showTitleScreen();
    }

    public int getGameState()
    {
        return gameState;
    }

    public void setGameState(int state)
    {
        //Set game state to game over if any impossible value is given
        if(state < 0 || state > 2)
        {
            state = 0;
        }
        gameState = state;
    }
}
