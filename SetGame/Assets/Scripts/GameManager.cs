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
    public GameType gameType;
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
        if (gameType != GameType.SinglePlayer)
            BoardUI.instance.SetCardsInteractable(false);
        
        UIManager.instance.AnimateButtons(true);
        BoardUI.instance.AnimateBackgroundColor(-1);

        CardData[] cards = BoardManager.instance.GetSelectedCards(selectedCards);

        UIManager.instance.EnableSetCheck(cards, BoardManager.instance.IsSet(cards));

        if (BoardManager.instance.IsSet(cards))
        {
            if (BoardManager.instance.deck.Count < 3)
            {
                Debug.Log("No more cards left!");
                //TODO Make selected cards invisible
            }
            else
            {
                BoardManager.instance.ReplaceCards(selectedCards);
                //TODO Create animation so cards don't change immediatly
                BoardUI.instance.DrawNewCards(selectedCards);
            }

            if (!BoardManager.instance.BoardHasSets())
            {
                if (BoardManager.instance.deck.Count < 3)
                {
                    Debug.Log("End Game");
                }
                else
                {
                    BoardManager.instance.AddCards();
                    BoardUI.instance.AddNewCards();
                }
            }

            //Debuggin
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