using UnityEngine;
using DG.Tweening;


public class OpeningDoor : MonoBehaviour
{
    public int requiredScore = 4;
    public float transitionDelay = 2f;
    public ScoreManager scoreManager;
    public Ease easeMode;
    public Vector3 transitionDirection;

    public void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void OpenDoor()
    {
        if (scoreManager.score >= requiredScore) //Si le score requis est atteint alors...
        {
            transform.DOMove(transform.position + transitionDirection, transitionDelay) //Pour donner une direction on doit prendre la position actuelle et ajouter à celle-ci un vecteur pour pas qu'il cherche à reach la position dans le monde
                .SetEase(easeMode);
        }
    }
}
