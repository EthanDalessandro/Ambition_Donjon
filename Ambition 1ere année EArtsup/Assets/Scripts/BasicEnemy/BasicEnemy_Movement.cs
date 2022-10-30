using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy_Movement : MonoBehaviour
{
    Vector3 originPosition;
    public NavMeshAgent agentPawn;
    float followSmooth = 0f;
    public float followSmoothCD = 1f;

    public void Start()
    {
        agentPawn = this.GetComponent<NavMeshAgent>();
        originPosition = transform.position;

        agentPawn.SetDestination(originPosition);
    }

    public void OnTriggerStay(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player)
        {
            agentPawn.stoppingDistance = 2f;

            followSmooth += 1f * Time.deltaTime;

            if (followSmooth >= followSmoothCD)
            {
                followSmooth = 0f;
                agentPawn.SetDestination(player.transform.position);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        if (player)
        {
            agentPawn.stoppingDistance = 0f;
            followSmooth = 0f;
            agentPawn.SetDestination(originPosition);
        }
    }

}
