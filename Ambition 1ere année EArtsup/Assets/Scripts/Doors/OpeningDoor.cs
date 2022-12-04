using UnityEngine;
using DG.Tweening;


public class OpeningDoor : MonoBehaviour
{
    public int requiredScore = 4;
    public float transitionDelay = 2f;
    public ScoreManager _scoreManager;

    public void OpenDoor()
    {
        if (_scoreManager._score >= requiredScore) //Si le score requis est atteint alors...
        {
            transform.DOMoveY(transform.position.y - 10f, transitionDelay)
                .SetEase(Ease.InOutQuint);

            Destroy(gameObject, 5f);
        }
    }
}
