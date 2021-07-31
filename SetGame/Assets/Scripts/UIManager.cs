using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI GameObjects")]
    public GameObject board;
    public GameObject playerButtons;
    public GameObject gameTypeSelection;
    public GameObject setCheck;

    [Header("Set! Buttons")]
    public GameObject topButton;
    public RectTransform topAnimLocation;
    public GameObject botButton;
    public RectTransform botAnimLocation;

    //Button animation variable
    [Header("Animation Options")]
    public float buttonAnimDuration = 0.5f;
    private Vector3 topInPos;
    private Vector3 botInPos;
    private Vector3 topStartPos;
    private Vector3 botStartPos;
    private Vector3 topEndPos;
    private Vector3 botEndPos;
    private bool enableButtonOnEnd;
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance of UIManager already exists. Destroying object.");
            Destroy(this);
        } 
    }
    private void Start() 
    {
        topInPos = topButton.GetComponent<RectTransform>().position;
        botInPos = botButton.GetComponent<RectTransform>().position;
    }
    public void SelectGameType()
    {
        board.SetActive(false);
        playerButtons.SetActive(false);
        setCheck.SetActive(false);

        gameTypeSelection.SetActive(true);
    }
    public void EnableBoard()
    {
        gameTypeSelection.SetActive(false);
        setCheck.SetActive(false);

        board.SetActive(true);
        playerButtons.SetActive(true);
        AnimateButtons(true);
    }
    public void EnableSetCheck(CardData[] cardsToCheck, bool wasSet)
    {
        setCheck.SetActive(true);
        setCheck.GetComponent<SetCheckUI>().StartAnimation(cardsToCheck, wasSet);
    }
    public void ButtonPress(int buttonIndex)
    {
        GameManager.instance.SetPressed(buttonIndex);
    }
    public void SetButtonsInteractable(bool shouldInteract)
    {
        topButton.GetComponent<Button>().interactable = shouldInteract;
        botButton.GetComponent<Button>().interactable = shouldInteract;
    }
    public void AnimateButtons(bool animateIn)
    {
        StopCoroutine(ExecuteButtonMove());

        //If the buttons are animating out, then disable so they can't be pressed. If they are animating in, just make sure they are disabled during the animation
        SetButtonsInteractable(false);

        if (animateIn)
        {
            topStartPos = topAnimLocation.position;
            topEndPos = topInPos;

            botStartPos = botAnimLocation.position;
            botEndPos = botInPos;
        }
        else
        {
            topStartPos = topInPos;
            topEndPos = topAnimLocation.position;

            botStartPos = botInPos;
            botEndPos = botAnimLocation.position;
        }

        enableButtonOnEnd = animateIn;

        StartCoroutine(ExecuteButtonMove());
    }
    IEnumerator ExecuteButtonMove()
    {
        float timer = 0f;

        RectTransform topRect = topButton.GetComponent<RectTransform>();
        RectTransform botRect = botButton.GetComponent<RectTransform>();

        while (timer <= buttonAnimDuration)
        {
            float percentage = Mathf.Clamp((timer / buttonAnimDuration), 0, 1);
            float smoothPercentage = Mathf.SmoothStep(0, 1, percentage);

            Vector3 topPos = Vector3.Lerp(topStartPos, topEndPos, smoothPercentage);
            Vector3 botPos = Vector3.Lerp(botStartPos, botEndPos, smoothPercentage);

            topRect.position = topPos;
            botRect.position = botPos;

            timer += Time.deltaTime;

            yield return null;
        }

        //If the buttons are animating in, set them to interactable after the animation
        if (enableButtonOnEnd)
        {
            SetButtonsInteractable(true);
        }
    }
}