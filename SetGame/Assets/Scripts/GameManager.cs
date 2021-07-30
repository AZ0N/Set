using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameType 
    {
        SinglePlayer,
        Versus,
        Multiplayer
    }

    [Header("Gameobject References")]
    public BoardManager boardManager;
    public GameObject boardGameobject;
    public GameObject gameTypeSelection;
    private GameType gameType;

    private void Start() 
    {
        boardGameobject.SetActive(false);
        gameTypeSelection.SetActive(true);    
    }
    private void StartGame() 
    {
        gameTypeSelection.SetActive(false);
        boardGameobject.SetActive(true);

        boardManager.SetupGame();
        BoardUI.instance.PopulateGrid(boardManager.board);

        if(gameType == GameType.SinglePlayer)
            BoardUI.instance.SetButtonInteractable(true);
        else
            BoardUI.instance.SetButtonInteractable(false);
        
        boardManager.PrintAvailableSets();
    }
    public void SetGameType(int gameTypeIndex)
    {
        switch (gameTypeIndex)
        {
            case 0:
                gameType = GameType.SinglePlayer;
                break;
            case 1:
                gameType = GameType.Versus;
                break;
            case 2:
                gameType = GameType.Multiplayer;
                break;
            default:
                break;
        }

        StartGame();
    }
}