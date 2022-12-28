using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy_Movement : MonoBehaviour
{
    // Variable de stockage de la position d'origine de l'ennemi
    Vector3 originPosition;

    // Composant NavMeshAgent de l'ennemi
    public NavMeshAgent agentPawn;

    // Variable de contrôle du délai de suivi du joueur
    float followSmooth = 0f;

    // Durée du délai de suivi du joueur
    public float followSmoothCD = 0.5f;

    public void Start()
    {
        agentPawn = this.GetComponent<NavMeshAgent>();
        // Enregistre la position d'origine de l'ennemi
        originPosition = transform.position;

        // Définit la destination de l'agent de navmesh sur sa position d'origine
        agentPawn.SetDestination(originPosition);
    }

    public void OnTriggerStay(Collider other)
    {
        // Récupère le script PlayerController attaché à l'objet en collision
        var player = other.GetComponent<PlayerController>();

        // Si l'objet en collision est le joueur
        if (player)
        {
            // Modifie la distance d'arrêt de l'agent de navmesh
            agentPawn.stoppingDistance = 2f;

            // Incrémente la variable de délai de suivi du joueur
            followSmooth += 1f * Time.deltaTime;

            // Si le délai de suivi du joueur est écoulé
            if (followSmooth >= followSmoothCD)
            {
                // Remet le délai de suivi du joueur à 0
                followSmooth = 0f;

                // Met à jour la destination de l'agent de navmesh sur la position du joueur
                agentPawn.SetDestination(player.transform.position);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerController>();

        // Si l'objet en collision était le joueur
        if (player)
        {
            // Remet la distance d'arrêt de l'agent de navmesh à 0
            agentPawn.stoppingDistance = 0f;

            // Remet le délai de suivi du joueur à 0
            followSmooth = 0f;

            // Met à jour la destination de l'agent de navmesh sur sa position d'origine
            agentPawn.SetDestination(originPosition);
        }

    }
}
