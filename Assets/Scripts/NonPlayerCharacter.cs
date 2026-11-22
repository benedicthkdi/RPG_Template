using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject dialogueBuddle;
    public string message;

    void Start()
    {
        dialogueBuddle.SetActive(false);
    }
}
