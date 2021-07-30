using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject iconPrefab;

    [Header("GameObject Components")]
    public RectTransform cardRect;
    public Image cardImage;

    [Header("Flip Options")]
    public Sprite frontSprite;
    public Sprite backSprite;
    public AnimationCurve flipCurve;
    private float flipDuration;
    private bool frontFacing = true;

    //Animation Variables
    private Vector3 initalPos;
    private Vector2 initialSizeDelta;
    private RectTransform targetRect;
    private float moveDuration;

    public void DrawIcons(Sprite icon, int amount) 
    {
        foreach (Transform child in transform) 
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject iconObject = Instantiate(iconPrefab, transform);
            iconObject.GetComponent<Image>().sprite = icon;
        }
    }
    public void StartMove(RectTransform _targetRect, float _duration) 
    {
        StopAllCoroutines();
        initalPos = cardRect.position;
        initialSizeDelta = cardRect.sizeDelta;
        targetRect = _targetRect;
        moveDuration = _duration;
        StartCoroutine(ExecuteMove());
    }
    private IEnumerator ExecuteMove()
    {
        float time = 0f;

        while(cardRect.position != targetRect.position)
        {
            float percentage = Mathf.Clamp((time / moveDuration), 0, 1);
            float smoothPercentage = Mathf.SmoothStep(0, 1, percentage);

            Vector3 calculatedPos = Vector3.Lerp(initalPos, targetRect.position, smoothPercentage);
            Vector2 calculatedSizeDelta = Vector2.Lerp(initialSizeDelta, targetRect.sizeDelta, smoothPercentage);

            cardRect.position = calculatedPos;
            cardRect.sizeDelta = calculatedSizeDelta;

            time += Time.deltaTime;

            yield return null;
        }
    }
    public void StartFlip(float _flipDuration)
    {
        StopCoroutine(ExecuteFlip());
        flipDuration = _flipDuration;
        StartCoroutine(ExecuteFlip());
    }
    private IEnumerator ExecuteFlip() 
    {
        float time = 0f;

        while (time <= flipDuration)
        {
            float currentScaleX = flipCurve.Evaluate(time / flipDuration);
            time += Time.deltaTime;

            Vector3 currentScale = cardRect.localScale;
            currentScale.x = currentScaleX;

            cardRect.localScale = currentScale;

            if ((time / flipDuration) >= 0.5f)
            {
                if(frontFacing)
                    cardImage.sprite = backSprite;
                else
                    cardImage.sprite = frontSprite;
            }

            yield return null;
        }

        frontFacing = !frontFacing;
    }
    public void SetFace(bool shouldFaceFront) 
    {
        if (shouldFaceFront)
            cardImage.sprite = frontSprite;
        else
            cardImage.sprite = backSprite;

        frontFacing = shouldFaceFront;
    }
}
