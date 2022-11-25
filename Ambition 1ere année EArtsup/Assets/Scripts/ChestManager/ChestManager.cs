using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    float delayDestroy = 3f;
    public int chestScore = 1;
    public ScoreManager _scoreManager;
    public bool isOpen = false;
    public AudioSource audioChest;

    public void OpenChest()
    {
        isOpen = true;
        audioChest.Play();
        Destroy(gameObject, delayDestroy);
        _scoreManager._score += chestScore;
    }
}
