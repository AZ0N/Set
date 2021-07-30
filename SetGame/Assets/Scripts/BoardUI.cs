using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
    public static BoardUI instance;

    [Header("Cards")]
    public Sprite[] shapeSprites;

    [Header("Prefabs")]
    public GameObject gridPrefab;
    public GameObject cardPrefab;

    [Header("UI References")]
    public Transform backgroundGridUI;
    public Transform foregroundGridUI;
    public RectTransform pile;
    public FlexibleGridLayout backgroundGridLayout;

    [Header("Animation Options")]
    public float defaultAnimationDuration = 1f;

    private List<RectTransform> backgroundGrid = new List<RectTransform>();
    private List<CardUI> foregroundCards = new List<CardUI>();

    [Header("Card Button Options")]
    public Color selectedColor;

    private ColorBlock baseButtonColors = ColorBlock.defaultColorBlock, selectedButtonColors = ColorBlock.defaultColorBlock;
    private int[] selectedCards = new int[]{-1, -1, -1};

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance of BoardUI already exists. Destroying object.");
            Destroy(this);
        }

        SetupColorBlocks();
    }
    public void PopulateGrid(List<CardData> board)
    {
        //Remove all children of backgroundUI transform. Remove all gridPrefabs
        foreach(Transform child in backgroundGridUI)
        {
            Destroy(child.gameObject);
        }

        //Instantiate the appropiate amount of gridPrefabs and add them to the backgroundGrid list
        for (int i = 0; i < board.Count; i++)
        {
            GameObject gridObject = Instantiate(gridPrefab, backgroundGridUI);
            backgroundGrid.Add(gridObject.GetComponent<RectTransform>());
        }

        //Force FlexibleLayoutGroup update after adding cards. Making sure animations can start the same frame
        backgroundGridLayout.CalculateLayoutInputHorizontal();

        for (int i = 0; i < board.Count; i++)
        {
            //Instantiate cardPrefab parented to the foregroundGrid
            GameObject cardObject = Instantiate(cardPrefab, foregroundGridUI);

            //Setting initial position and size of cards
            cardObject.GetComponent<RectTransform>().position = pile.position;
            cardObject.GetComponent<RectTransform>().sizeDelta = backgroundGridLayout.cellSize;

            //Add the CardUI component to the list of cards
            CardUI cardUI = cardObject.GetComponent<CardUI>();
            foregroundCards.Add(cardUI);

            //Draw icons on the created card and start he animation
            DrawCard(i, board[i]);
            cardUI.StartMove(backgroundGrid[i], defaultAnimationDuration);
        }
    }
    private void DrawCard(int cardIndex, CardData card)
    {
        //Get the index of the correct sprite, and calling the cards DrawIcons method
        int spriteIndex = card.GetColorIndex() * 9 + card.GetFillIndex() * 3 + card.GetShapeIndex();
        foregroundCards[cardIndex].DrawIcons(shapeSprites[spriteIndex], card.GetAmountIndex() + 1);
    }
    public void SetButtonInteractable(bool shouldInteract) 
    {
        foreach (CardUI cardUI in foregroundCards) 
        {
            cardUI.gameObject.GetComponent<Button>().interactable = shouldInteract;
        }
    }
    public void SelectCard(int cardIndex)
    {
        // Get the index of cardIndex in selectedCards array. Returns -1 if cardIndex is not in selectedCards
        int index = Array.IndexOf(selectedCards, cardIndex);

        if (index != -1)
        {
            //If it isn't -1, cardIndex is already present in selectedCards. Deselect the card.
            selectedCards[index] = -1;
            Debug.Log($"Deselected {cardIndex}");
        }
        else
        {
            //Else the numbers is not present in selectedCards. Loop until we find an empty spot
            for (int i = 0; i < selectedCards.Length; i++)
            {
                if (selectedCards[i] == -1)
                {
                    //An empty spot was found. Set the spot and break out of the loop
                    selectedCards[i] = cardIndex;
                    Debug.Log($"Selected {cardIndex}");
                    break;
                }
            }

            //After adding the cardIndex to selectedCards check if the array is full (doesn't contain -1)
            if (Array.IndexOf(selectedCards, -1) == -1) 
            {
                Debug.Log("3 cards selected! Clearing selection");
                //Clearing array
                for (int i = 0; i < selectedCards.Length; i++)
                {
                    selectedCards[i] = -1;
                }
            }
        }
    }
    private void SetupColorBlocks()
    {
        baseButtonColors.normalColor = Color.white;
        baseButtonColors.pressedColor = Color.white;
        baseButtonColors.highlightedColor = selectedColor;

        selectedButtonColors.normalColor = selectedColor;
        selectedButtonColors.selectedColor = selectedColor;
        selectedButtonColors.highlightedColor = selectedColor;
    }
 }
