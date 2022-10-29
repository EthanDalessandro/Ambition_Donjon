using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{

    public float delay = 2f;
    void Start()
    {

    }

    void Update()
    {
        Destroy(gameObject, delay);
    }
}
