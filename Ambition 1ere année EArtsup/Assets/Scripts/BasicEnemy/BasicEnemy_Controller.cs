using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class BasicEnemy_Controller : MonoBehaviour
{
    public HealthBarPlayer healthBarPlayer;
    public Slider mainSlider;
    public int minLevel = 0;
    public int maxLevel = 10;
    Vector3 originPosition;
    NavMeshAgent agentPawn;
    public GameObject _player;
    public TextMesh textName_LV;
    public TextMesh textHealth;
    public float level;
    public float health = 100;
    public float maxHealth;
    public float damage = 10f;
    public float attackSpeed = 1f;
    float _attackSpeed = 1f;
    public bool canAttack = true;
    public List<GameObject> PlayerList;

    void Start()
    {
        originPosition = new Vector3(transform.position.x, 0, transform.position.z); // je donne la position de l'instantiation comme point de départ de ce gameobject
        level = Random.Range(minLevel, maxLevel); // je random le lv du gameobject à l'instantiation
        health = Random.Range(health, (level * health / 5)); // les pv du personnages sont définies par ce calcul (qui est un calcul au pif)

        maxHealth = health;
        mainSlider.value = health / maxHealth;

        agentPawn = this.GetComponent<NavMeshAgent>();

        textName_LV.text = transform.name + " LEVEL : " + level.ToString(); // Le nom du gameobject et son lv sont définie ici
    }

    void Update()
    {
        if (canAttack == false) //Si il ne peut plus attaquer 
        {
            _attackSpeed += 1 * Time.deltaTime; //j'incrémente un nombre qui vas servir de cooldown
            if (_attackSpeed > attackSpeed) //si le nombre est supérieure à mon attackspeed
            {
                canAttack = true; //il peut de nouveau attaquer
                _attackSpeed = 0; //et on reset le nombre incrémenter
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject); // Mort du gameobject
        }

        textHealth.text = health.ToString(); //? les Pv sont vérifié constamment et mis à jour même si aucun changement, pas opti, on pourrait faire la MaJ à chaque fois qu'il reçois un coup un ou un autre événement, A REVOIR

        //!Autre manière de faire une direction qui bloque sur l'axe Y pour la barre de vie en 3D par exemple
        // Vector3 direction = _player.transform.position - textName_LV.transform.position; // la direction est de la healthbar --> joueur donc p - h
        // direction.y = 0;
        // Quaternion rotation = Quaternion.LookRotation(-direction);
        // textName_LV.transform.rotation = rotation;
        textName_LV.transform.rotation = Quaternion.LookRotation(textName_LV.transform.position - _player.transform.position); //on fait en sort que les text regarde constamment la camera
        textHealth.transform.rotation = Quaternion.LookRotation(textHealth.transform.position - _player.transform.position); //on fait en sort que les text regarde constamment la camera
        mainSlider.transform.rotation = Quaternion.LookRotation(mainSlider.transform.position - _player.transform.position);

        if (PlayerList.Count != 0) //Si il y a un joueur qui rentre dans la liste et donc que la liste n'est pas égale à 0
        {
            if (Vector3.Distance(transform.position, PlayerList[0].transform.position) <= agentPawn.stoppingDistance + 0.5f && canAttack == true) // si la distance entre le player, et que le gameobject peut attaquer alors on fait l'action
            {
                Debug.Log(transform.name + "Do Damage");
                PlayerList[0].gameObject.GetComponent<PlayerController>().health -= damage; // on retire des pv au premier player dans la liste
                healthBarPlayer.UpdateHealth();//Et on Update la health bar de ce joueur
                canAttack = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerList.Add(other.GetComponent<PlayerController>().gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            PlayerList.Remove(other.GetComponent<PlayerController>().gameObject);
        }
    }

    public void UpdateHealth()
    {
        mainSlider.value = health / maxHealth;
    }
}