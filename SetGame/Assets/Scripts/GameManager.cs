using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameType 
    {
        SinglePlayer,
        Versus,
        Multiplayer
    }
    private GameType gameType;
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance of GameManager already exists. Destroying object.");
            Destroy(this);
        } 
    }
    private void Start() 
    {
        UIManager.instance.SelectGameType();    
    }
    private void StartGame() 
    {
        UIManager.instance.EnableBoard();

        BoardManager.instance.SetupGame();
        BoardUI.instance.PopulateGrid(BoardManager.instance.board);

        if(gameType == GameType.SinglePlayer)
            BoardUI.instance.SetCardsInteractable(true);
        else
            BoardUI.instance.SetCardsInteractable(false);
        
        //Debugging
        BoardManager.instance.PrintAvailableSets();
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
    public void CardsSelected(int[] selectedCards)
    {
        Debug.Log($"Cards selected: {selectedCards[0]}, {selectedCards[1]}, {selectedCards[2]}");
        BoardUI.instance.SetCardsInteractable(false);
        UIManager.instance.AnimateButtons(true);
        BoardUI.instance.AnimateBackgroundColor(-1);
    }
    public void SetPressed(int buttonIndex)
    {
        BoardUI.instance.SetCardsInteractable(true);
        UIManager.instance.AnimateButtons(false);
        BoardUI.instance.AnimateBackgroundColor(buttonIndex);
        
        //TODO Start timer
    }
}