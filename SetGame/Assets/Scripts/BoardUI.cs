using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
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

    private void Start() 
    {
        PopulateGrid();
    }

    public void PopulateGrid(int gridSize = 12)
    {
        foreach(Transform child in backgroundGridUI)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gridSize; i++)
        {
            GameObject gridObject = Instantiate(gridPrefab, backgroundGridUI);
            backgroundGrid.Add(gridObject.GetComponent<RectTransform>());
        }

        //Force FlexibleLayoutGroup update after adding cards. Making sure animations can start the same frame
        backgroundGridLayout.CalculateLayoutInputHorizontal();

        for (int i = 0; i < gridSize; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, foregroundGridUI);

            cardObject.GetComponent<RectTransform>().position = pile.position;
            cardObject.GetComponent<RectTransform>().sizeDelta = backgroundGridLayout.cellSize;
            //TODO Calculate size of cards

            CardUI cardUI = cardObject.GetComponent<CardUI>();
            foregroundCards.Add(cardUI);
            cardUI.StartMove(backgroundGrid[i], defaultAnimationDuration);
        }
    }
 }
