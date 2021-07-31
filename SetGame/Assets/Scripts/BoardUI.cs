using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
    public static BoardUI instance;

    [Header("Card Sprites")]
    public Sprite[] iconSprites;

    [Header("Prefabs")]
    public GameObject gridPrefab;
    public GameObject cardPrefab;

    [Header("UI References")]
    public Transform backgroundGridUI;
    public Transform foregroundGridUI;
    public RectTransform pileLocation;
    public FlexibleGridLayout backgroundGridLayout;

    [Header("Animation Options")]
    public float defaultAnimationDuration = 1f;

    //Board Objects
    private List<RectTransform> backgroundGrid = new List<RectTransform>();
    private List<CardUI> foregroundCards = new List<CardUI>();

    [Header("Card Button Options")]
    public Color selectedColor;

    [Header("Component References")]
    public Image boardBackground;
    
    //Animation variables
    [Header("Color Animation Options")]
    public float colorAnimDuration = 0.5f;
    public Color topColor;
    public Color botColor;
    private Color startColor;
    private Color endColor;

    private ColorBlock baseButtonColors = ColorBlock.defaultColorBlock, selectedButtonColors = ColorBlock.defaultColorBlock;
    private int[] selectedCards = new int[]{-1, -1, -1};

    //General
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
            cardObject.GetComponent<RectTransform>().position = pileLocation.position;
            cardObject.GetComponent<RectTransform>().sizeDelta = backgroundGridLayout.cellSize;

            //Add the CardUI component to the list of cards
            CardUI cardUI = cardObject.GetComponent<CardUI>();
            foregroundCards.Add(cardUI);

            //Draw icons on the created card and start he animation
            DrawCard(i, board[i]);
            cardUI.StartMove(backgroundGrid[i], defaultAnimationDuration);
        }
    }
    private void SetupColorBlocks()
    {
        baseButtonColors.normalColor = Color.white;
        baseButtonColors.pressedColor = selectedColor;
        baseButtonColors.highlightedColor = Color.white;
        baseButtonColors.selectedColor = Color.white;
        baseButtonColors.disabledColor = Color.white;

        selectedButtonColors.normalColor = selectedColor;
        selectedButtonColors.pressedColor = selectedColor;
        selectedButtonColors.selectedColor = selectedColor;
        selectedButtonColors.highlightedColor = selectedColor;
        selectedButtonColors.disabledColor = selectedColor;
    }

    //Card Icon Drawing
    private void DrawCard(int cardIndex, CardData card)
    {
        //Get the index of the correct sprite, and calling the cards DrawIcons method
        int spriteIndex = card.GetColorIndex() * 9 + card.GetFillIndex() * 3 + card.GetShapeIndex();
        foregroundCards[cardIndex].DrawIcons(iconSprites[spriteIndex], card.GetAmountIndex() + 1);
    }
    public void DrawNewCards(int[] newCardsIndexes)
    {
        foreach (int newCardIndex in newCardsIndexes)
        {
            DrawCard(newCardIndex, BoardManager.instance.board[newCardIndex]);
        }
    }
    
    //Card Methods
    public void SetCardsInteractable(bool shouldInteract) 
    {
        foreach (CardUI cardUI in foregroundCards) 
        {
            cardUI.cardButton.interactable = shouldInteract;
        }
    }
    public void AddNewCards()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject gridObject = Instantiate(gridPrefab, backgroundGridUI);
            backgroundGrid.Add(gridObject.GetComponent<RectTransform>());
        }

        backgroundGridLayout.CalculateLayoutInputHorizontal();

        for (int i = 0; i < foregroundCards.Count; i++)
        {
            foregroundCards[i].StartMove(backgroundGrid[i], defaultAnimationDuration);
        }

        for (int i = BoardManager.instance.board.Count - 3; i < BoardManager.instance.board.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, foregroundGridUI);

            cardObject.GetComponent<RectTransform>().position = pileLocation.position;
            cardObject.GetComponent<RectTransform>().sizeDelta = backgroundGridLayout.cellSize;

            CardUI cardUI = cardObject.GetComponent<CardUI>();
            foregroundCards.Add(cardUI);

            DrawCard(i, BoardManager.instance.board[i]);
            cardUI.StartMove(backgroundGrid[i], defaultAnimationDuration);
        }
    }

    //Card Selection
    public void SelectCard(int cardIndex)
    {
        // Get the index of cardIndex in selectedCards array. Returns -1 if cardIndex is not in selectedCards
        int indexOfCardIndex = Array.IndexOf(selectedCards, cardIndex);

        if (indexOfCardIndex != -1)
        {
            //If it isn't -1, cardIndex is already present in selectedCards. Deselect the card.
            selectedCards[indexOfCardIndex] = -1;
            foregroundCards[cardIndex].SetButtonColors(baseButtonColors);
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
                    foregroundCards[cardIndex].SetButtonColors(selectedButtonColors);
                    break;
                }
            }

            //After adding the cardIndex to selectedCards check if the array is full (doesn't contain -1)
            if (Array.IndexOf(selectedCards, -1) == -1) 
            {
                GameManager.instance.CardsSelected(new int[] {selectedCards[0], selectedCards[1], selectedCards[2]});
                //Clearing array
                for (int i = 0; i < selectedCards.Length; i++)
                {
                    foregroundCards[selectedCards[i]].SetButtonColors(baseButtonColors);
                    selectedCards[i] = -1;
                }
            }
        }
    }
    
    //Animation Methods
    public void AnimateBackgroundColor(int colorIndex)
    {
        StopCoroutine(ExecuteColorAnim());

        startColor = boardBackground.color;
        switch (colorIndex)
        {
            case 0:
                endColor = topColor;
                break;
            case 1:
                endColor = botColor;
                break;
            case -1:
                endColor = Color.white;
                break;
            default:
                break;
        }

        StartCoroutine(ExecuteColorAnim());
    }
    IEnumerator ExecuteColorAnim()
    {
        float timer = 0f;

        while (timer <= colorAnimDuration)
        {
            float percentage = Mathf.Clamp((timer / colorAnimDuration), 0, 1);
            float smoothPercentage = Mathf.SmoothStep(0, 1, percentage);

            Color currentColor = Color.Lerp(startColor, endColor, smoothPercentage);
            boardBackground.color = currentColor;

            timer += Time.deltaTime;

            yield return null;
        }
    }
 }