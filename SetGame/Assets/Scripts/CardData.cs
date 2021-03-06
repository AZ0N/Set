using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardData
{
    public short attributes {get;}
    public CardData(int colorShift, int shapeShift, int amountShift, int fillShift)
    {
        //Check if the argument passed to the constructor are valid.
        if (colorShift > 2 || shapeShift > 2 || amountShift > 2 || fillShift > 2)
        {
            Debug.Log($"Wrong parameters for creating CardData instance: {colorShift}, {shapeShift}, {amountShift}, {fillShift}");
        }
        //Bitshift values into the following pattern **** AAA BBB CCC DDD, where AAA=Color, BBB=Shape, CCC=Amount, DDD=Fill and **** is unused bits
        attributes = (short)((1 << (colorShift + 9)) | (1 << (shapeShift + 6)) | (1 << (amountShift + 3)) | (1 << fillShift));
    }

    public int GetColorIndex() 
    {
        return GetAttributeIndex(attributes >> 9);
    }
    public int GetShapeIndex()
    {
        return GetAttributeIndex((attributes >> 6) & 0b111);
    }
    public int GetAmountIndex() 
    {
        return GetAttributeIndex((attributes >> 3) & 0b111);
    }
    public int GetFillIndex()
    {
        return GetAttributeIndex(attributes & 0b111);
    }
    public int GetAttributeIndex(int attr) 
    {
        switch (attr)
        {
            case 0b001:
                return 0;
            case 0b010:
                return 1;
            case 0b100:
                return 2;
        }
        //Error
        return -1;
    }
    public void PrintCardBits() //Debugging method to check card bits
    {
        string bitString = string.Empty;

        for (int i = (sizeof(short) * 8) - 1; i >= 0; i--)
        {
            if ((attributes & (1 << i)) == (1 << i))
                bitString += "1";
            else
                bitString += "0";
        }

        Debug.Log(bitString);
    }
    public void PrintCardAttributes() //Debugging method easily identifying attributes
    {
        string attributeString = string.Empty;
        
        for (int i = 3; i >= 0; i--)
        {
            switch ((attributes >> (i * 3)) & 0b111)
            {
                case 0b001:
                    attributeString += "A";
                    break;
                case 0b010:
                    attributeString += "B";
                    break;
                case 0b100:
                    attributeString += "C";
                    break;
                default:
                    break;
            }
        }
        Debug.Log(attributeString);
    }
}
