using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LittleMob_AnimationController : MonoBehaviour
{
    Animator anim;
    NavMeshAgent pawnAgent;

    void Start()
    {
        anim = this.GetComponent<Animator>();

        pawnAgent = this.GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, pawnAgent.destination) >= 1.3f) //si la distance entre le pawn et la destination et supérieur à 1.3f alors l'animation de movement se joue
        {
            anim.SetBool("IsMoving?", true);
            anim.SetBool("IsAttacking?", false);
            if (Vector3.Distance(transform.position, pawnAgent.destination) <= 2.7f && pawnAgent.stoppingDistance == 2)
            {
                anim.SetBool("IsMoving?", false);
                anim.SetBool("IsAttacking?", true);
            }
            else
            {
                anim.SetBool("IsAttacking?", false);
                anim.SetBool("IsMoving?", true);
            }
        }
        else
        {
            anim.SetBool("IsMoving?", false);
            anim.SetBool("IsAttacking?", false);
        }
    }
}
