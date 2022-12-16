using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    Text scoreText;
    ScoreManager scoreManager;

    public void Start()
    {
        scoreText = GetComponent<Text>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void AddPoint()
    {
        scoreText.text = "Score  : " + scoreManager.score;
    }
}
    
