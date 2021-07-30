using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
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
 }
