using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject gridPrefab;
    public GameObject cardPrefab;

    [Header("UI Gameobjects")]
    public GameObject backgroundGridUI;
    public GameObject foregroundGridUI; 

    //TODO Change lists of GameObjects to RectTransforms and CardUI (or whatever the UI card class is called)
    private GameObject[] backgroundGrid;
    private GameObject[] foregroundCards;

    private void Start() 
    {
        PopulateGrid();
    }

    public void PopulateGrid(int gridSize = 12)
    {
        foreach(Transform child in backgroundGridUI.transform)
        {
            Destroy(child);
        }

        for (int i = 0; i < gridSize; i++)
        {
            var gridObject = Instantiate(gridPrefab, backgroundGridUI.transform);
        }

        //TODO Instantiate cards and animate their position to the background grid
    }
 }
