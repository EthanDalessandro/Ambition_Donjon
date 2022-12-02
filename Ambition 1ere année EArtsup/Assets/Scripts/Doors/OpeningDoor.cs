using UnityEngine;
using DG.Tweening;


public class OpeningDoor : MonoBehaviour
{
    public int requiredScore = 4;
    public ScoreManager _scoreManager;
    Rigidbody rb;

    public void OpenDoor()
    {
        if(_scoreManager._score >= requiredScore) //Si le score requis est atteint alors...
        {
            transform.DOMoveY(transform.position.y - 10f, 2)
                .SetEase(Ease.InBounce);
                
            Destroy(gameObject, 5f);
        }
    }
}
