using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCheckUI : MonoBehaviour
{
    [Header("Cards UI")]
    public Transform[] cardTransforms;
    public RectTransform backgroundRect;
    public Image backgroundImage;

    [Header("Sprites and IconPrefab")]
    public Sprite[] iconSprites;
    public GameObject iconPrefab;

    [Header("Component References")]
    public Image panelImage;

    [Header("Animation Settings")]
    public float animateInOut = 0.5f;
    public float animateIsSet = 0.5f;
    public float animateWait = 1f;
    public AnimationCurve shakeCurve;
    public float shakeMaxAngle = 30f;
    [Header("Background Colors")]
    public Color backgroundBaseColor;
    public Color isSetColor;
    public Color isNotSetColor;
    [Header("Panel Colors")]
    public Color enabledColor;
    public Color disabledColor;
    [Header("Animation References")]
    public RectTransform inLocation;
    public RectTransform middleLocation;
    public RectTransform outLocation;
    private bool wasSet = false;
    public void StartAnimation(CardData[] cards, bool _wasSet)
    {
        StopAllCoroutines();
        wasSet = _wasSet;
        DrawCards(cards);
        StartCoroutine(ExecuteAnimation());
    }
    IEnumerator ExecuteAnimation()
    {
        float timer = 0f;

        backgroundRect.position = inLocation.position;
        backgroundImage.color = backgroundBaseColor;

        while (backgroundRect.position != middleLocation.position)
        {
            float percentage = Mathf.Clamp((timer / animateInOut), 0, 1);
            float smoothPercentage = Mathf.SmoothStep(0, 1, percentage);

            Color panelColor = Color.Lerp(disabledColor, enabledColor, smoothPercentage);
            Vector3 position = Vector3.Lerp(inLocation.position, middleLocation.position, smoothPercentage);

            panelImage.color = panelColor;
            backgroundRect.position = position;

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while(timer <= animateIsSet)
        {
            float percentage = Mathf.Clamp((timer / animateIsSet), 0, 1);
            float smoothPercentage = Mathf.SmoothStep(0, 1, percentage);

            Color backgroundColor;

            if (wasSet)
            {
                backgroundColor = Color.Lerp(backgroundBaseColor, isSetColor, smoothPercentage);
            }
            else
            {
                backgroundColor = Color.Lerp(backgroundBaseColor, isNotSetColor, smoothPercentage);
                float shakeRotation = shakeCurve.Evaluate(smoothPercentage) * shakeMaxAngle;
                backgroundRect.eulerAngles = Vector3.forward * shakeRotation;
            }

            backgroundImage.color = backgroundColor;

            timer += Time.deltaTime;
            yield return null;
        }

        backgroundRect.eulerAngles = Vector3.zero;
        timer = 0f;

        while (timer <= animateWait)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while (backgroundRect.position != outLocation.position)
        {
            float percentage = Mathf.Clamp((timer / animateIsSet), 0, 1);
            float smoothPercentage = Mathf.SmoothStep(0, 1, percentage);

            Color panelColor = Color.Lerp(enabledColor, disabledColor, smoothPercentage);
            Vector3 position = Vector3.Lerp(middleLocation.position, outLocation.position, smoothPercentage);

            panelImage.color = panelColor;
            backgroundRect.position = position;

            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
    public void DrawCards(CardData[] cards)
    {
        for (int i = 0; i < cardTransforms.Length; i++)
        {
            foreach (Transform child in cardTransforms[i])
            {
                Destroy(child.gameObject);
            }
            
            int spriteIndex = cards[i].GetColorIndex() * 9 + cards[i].GetFillIndex() * 3 + cards[i].GetShapeIndex();

            for (int j = 0; j < cards[i].GetAmountIndex() + 1; j++)
            {
                GameObject iconObject = Instantiate(iconPrefab, cardTransforms[i].transform);
                iconObject.GetComponent<Image>().sprite = iconSprites[spriteIndex];
            }
        }
    }
}
