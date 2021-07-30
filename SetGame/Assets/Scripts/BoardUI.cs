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
    public float defaultAnimationDuration = 2f;

    //TODO Change lists of GameObjects to RectTransforms and CardUI (or whatever the UI card class is called)
    private List<RectTransform> backgroundGrid = new List<RectTransform>();
    private List<CardUI> foregroundCards = new List<CardUI>();

    public void PopulateGrid(List<CardData> board)
    {
        foreach(Transform child in backgroundGridUI)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < board.Count; i++)
        {
            GameObject gridObject = Instantiate(gridPrefab, backgroundGridUI);
            backgroundGrid.Add(gridObject.GetComponent<RectTransform>());
        }

        //Force FlexibleLayoutGroup update after adding cards. Making sure animations can start the same frame
        backgroundGridLayout.CalculateLayoutInputHorizontal();

        for (int i = 0; i < board.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, foregroundGridUI);

            cardObject.GetComponent<RectTransform>().position = pile.position;
            cardObject.GetComponent<RectTransform>().sizeDelta = backgroundGridLayout.cellSize;

            CardUI cardUI = cardObject.GetComponent<CardUI>();
            foregroundCards.Add(cardUI);
            DrawCard(i, board[i]);
            cardUI.StartMove(backgroundGrid[i], defaultAnimationDuration);
        }
    }
   
    private void DrawCard(int cardIndex, CardData card)
    {
        int spriteIndex = card.GetColorIndex() * 9 + card.GetFillIndex() * 3 + card.GetShapeIndex();
        foregroundCards[cardIndex].DrawIcons(shapeSprites[spriteIndex], card.GetAmountIndex() + 1);
    }
 }
