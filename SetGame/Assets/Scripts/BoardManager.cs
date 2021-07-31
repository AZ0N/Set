using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public int baseBoardSize = 12;

    //CardData lists
    public List<CardData> board = new List<CardData>();
    public List<CardData> deck = new List<CardData>();

    private System.Random rng = new System.Random();

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance of BoardManager already exists. Destroying object.");
            Destroy(this);
        } 
    }
    
    //Set operations
    public bool IsSet(CardData a, CardData b, CardData c)
    {
        short or_result = (short)((int)a.attributes | (int)b.attributes | (int)c.attributes);

        for (int i = 0; i < 4; i++)
        {
            switch (or_result & 0b111)
            {
                case 0b001:
                case 0b010:
                case 0b100:
                case 0b111:
                    break;
                default:
                    return false;
            }
            or_result = (short)(or_result >> 3);
        }
        return true;
    }
    public bool IsSet(CardData[] cards)
    {
        return IsSet(cards[0], cards[1], cards[2]);
    }

    //General board operations
    public void SetupGame() 
    {
        GenerateDeck();
        ShuffleDeck();
        PopulateBoard();
    }
    public void PopulateBoard()
    {
        for (int i = 0; i < baseBoardSize; i++)
        {
            board.Add(PopCardFromDeck());
        }
    }
    public bool BoardHasSets()
    {
        for (int i = 0; i < board.Count - 2; i++)
        {
            for (int j = i + 1; j < board.Count - 1; j++)
            {
                for (int k = j + 1; k < board.Count; k++)
                {
                    if (IsSet(board[i], board[j], board[k]))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    
    //Deck operations
    public void GenerateDeck() 
    {
        deck.Clear();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        deck.Add(new CardData(i, j, k, l));
                    }
                }
            }
        }
    }
    public void ShuffleDeck()
        {
            //Shuffles the deck using Fisher-Yates shuffle. https://stackoverflow.com/questions/273313/randomize-a-listt
            int n = deck.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CardData c = deck[k];
                deck[k] = deck[n];
                deck[n] = c;
            }
        }
    public CardData PopCardFromDeck() 
    {
        CardData card = deck[0];
        deck.Remove(card);
        return card;
    }

    //Debugging methods to print contents of deck and board lists
    public void PrintCardList(List<CardData> cardList) 
    {
        foreach (CardData card in cardList) 
        {
            card.PrintCardAttributes();
        } 
    }
    public void PrintDeck() 
    {
        PrintCardList(deck);
    }
    public void PrintBoard() 
    {
        PrintCardList(board);
    }
    public void PrintAvailableSets()
    {
        int amountSets = 0;
        string set_locations = "";

        for (int i = 0; i < board.Count - 2; i++)
        {
            for (int j = i + 1; j < board.Count - 1; j++)
            {
                for (int k = j + 1; k < board.Count; k++)
                {
                    if (IsSet(board[i], board[j], board[k]))
                    {
                        set_locations += $" ({i + 1},{j + 1},{k + 1})";
                        amountSets++;
                    }
                }
            }
        }

        Debug.Log($"{amountSets} available sets:{set_locations}");
    }
}