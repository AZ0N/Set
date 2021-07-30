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

    [Header("Animation Options")]
    public float defaultAnimationDuration = 2f;

    //TODO Change lists of GameObjects to RectTransforms and CardUI (or whatever the UI card class is called)
    private List<GameObject> backgroundGrid = new List<GameObject>();
    private CardUI[] foregroundCards;

    private void Start() 
    {
        PopulateGrid();
    }

    public void PopulateGrid(int gridSize = 12)
    {
        foreach(Transform child in backgroundGridUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gridSize; i++)
        {
            GameObject gridObject = Instantiate(gridPrefab, backgroundGridUI.transform);
            backgroundGrid.Add(gridObject);
        }

        //TODO Instantiate cards and animate their position to the background grid
    }
 }
