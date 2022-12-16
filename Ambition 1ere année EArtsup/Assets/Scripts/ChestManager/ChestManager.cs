using UnityEngine;

public class ChestManager : MonoBehaviour
{
    float delayDestroy = 3f;
    public int chestScore = 1;
    public ScoreManager scoreManager;
    ScoreHUD scoreHUD;
    public bool isOpen = false;
    public AudioSource audioChest;
    public OpeningDoor[] openDoor;

    public void Start()
    {
        scoreHUD = FindObjectOfType<ScoreHUD>();
        scoreManager = FindObjectOfType<ScoreManager>();
        openDoor = FindObjectsOfType<OpeningDoor>();
    }
    public void OpenChest()
    {
        isOpen = true;
        audioChest.Play();
        Destroy(gameObject, delayDestroy);
        scoreManager.score += chestScore;
        for (int i = 0; i < openDoor.Length; i++)
        {
            openDoor[i].OpenDoor();
        }
        scoreHUD.AddPoint();
    }
}
