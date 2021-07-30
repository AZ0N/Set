using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gameobject References")]
    public BoardManager boardManager;
    public BoardUI boardUI;

    private void Start() 
    {
        //Setup game data and draw cards to screen
        boardManager.SetupGame();
        boardUI.PopulateGrid(boardManager.board);    
    }
}