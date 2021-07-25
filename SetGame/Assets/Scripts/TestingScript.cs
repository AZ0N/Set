using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    void Start()
    {
        CardData testCard = new CardData(0, 0, 1, 2);
        testCard.PrintCardBits();
    }
}
