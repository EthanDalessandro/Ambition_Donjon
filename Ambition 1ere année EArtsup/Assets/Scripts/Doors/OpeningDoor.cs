using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    public int requiredScore = 4;
    public ScoreManager _scoreManager;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Ces paramètres permette à l'objet de ne pas bouger et de bloquer le player
        GetComponent<BoxCollider>().isTrigger = false;
        //rb.useGravity = false;
        rb.isKinematic = true;
    }
    void Update()
    {
        if(_scoreManager._score >= requiredScore) //Si le score requis est atteint alors...
        {
            //Ces paramètres vont permettre à l'objet de perdre sa collision et d'être affecté par la gravité et de détruire l'objet au bout de 5s
            GetComponent<BoxCollider>().isTrigger = true;
            rb.useGravity = true;
            rb.isKinematic = false;
            Destroy(gameObject, 5f);
        }
    }
}
