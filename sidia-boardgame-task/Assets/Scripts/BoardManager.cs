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

    void Start()
    {
        
    }

    void Update()
    {
        //Method responsible for get the tile which the player is pointing to
        UpdateSelectionTile();
        //Method responsible for drawing the board boarders
        DrawBoardSkeleton();
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
}
