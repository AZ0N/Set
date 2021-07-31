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
        BoardUI.instance.SetCardsInteractable(false);
        UIManager.instance.AnimateButtons(true);
        BoardUI.instance.AnimateBackgroundColor(-1);

        CardData[] cards = new CardData[]
        {
            BoardManager.instance.board[selectedCards[0]],
            BoardManager.instance.board[selectedCards[1]],
            BoardManager.instance.board[selectedCards[2]]
        };

        UIManager.instance.EnableSetCheck(cards, BoardManager.instance.IsSet(cards));

        if (BoardManager.instance.IsSet(cards))
        {
            BoardManager.instance.ReplaceCards(selectedCards);
            //TODO Create animation so cards don't change immediatly
            BoardUI.instance.DrawNewCards(selectedCards);
            BoardManager.instance.PrintAvailableSets();
        }
        //TODO Handle points
    }
    public void SetPressed(int buttonIndex)
    {
        BoardUI.instance.SetCardsInteractable(true);
        UIManager.instance.AnimateButtons(false);
        BoardUI.instance.AnimateBackgroundColor(buttonIndex);
        
        //TODO Start timer
    }
}