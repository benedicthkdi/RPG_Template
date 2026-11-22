using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Image healthBar;

    public GameObject npcDialogue;
    public TextMeshProUGUI dialogueText;
    public float displayTime = 4f;
    private float timerDisplay;

    public GameObject loseScreen;
    public GameObject winScreen;


    void Start()
    {
        SetHealthValue(1);
        npcDialogue.SetActive(false);
        timerDisplay = -1f;
    }

    void Update()
    {
        if(timerDisplay > 0)
        {
            timerDisplay -= Time.deltaTime;
            if(timerDisplay < 0)
            {
                npcDialogue.SetActive(false);
            }
        }
    }

    public void SetHealthValue(float percentage)
    {
        healthBar.fillAmount = percentage; 
    }

    public void DisplayDialogue(string message)
    {
        dialogueText.text = message;
        npcDialogue.SetActive(true);
        timerDisplay = displayTime;
    }

    public void DisplayLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void DisplayWinScreen()
    {
        winScreen.SetActive(true);
    }
}
