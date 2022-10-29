using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy_Controller : MonoBehaviour
{
    Vector3 originPosition;
    public GameObject _player;
    public TextMesh textName_LV;
    public TextMesh textHealth;
    public float lv;
    public float health = 100;
    public float mana = 100;
    public float damage = 10f;
    public float attackRange = 0.5f;
    public float attackSpeed = 1f;
    float _attackSpeed = 1f;
    public bool canAttack = true;
    public List<GameObject> PlayerList;

    void Start()
    {
        originPosition = new Vector3(transform.position.x, 0, transform.position.z); // je donne la position de l'instantiation comme point de départ de ce gameobject
        lv = Random.Range(0, 150); // je random le lv du gameobject à l'instantiation
        health = Random.Range(health, (lv * health / 5)); // les pv du personnages sont définies par ce calcul (qui est un calcul au pif)

        textName_LV.text = transform.name + " LEVEL : " + lv.ToString(); // Le nom du gameobject et son lv sont définie ici
    }

    void Update()
    {
        if(canAttack == false) //Si il ne peut plus attaquer 
        {
            _attackSpeed += 1 * Time.deltaTime; //j'incrémente un nombre qui vas servir de cooldown
            if(_attackSpeed > attackSpeed) //si le nombre est supérieure à mon attackspeed
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

        textName_LV.transform.rotation = Quaternion.LookRotation(textName_LV.transform.position - _player.transform.position); //on fait en sort que les text regarde constamment la camera
        textHealth.transform.rotation = Quaternion.LookRotation(textHealth.transform.position - _player.transform.position); //on fait en sort que les text regarde constamment la camera

        if(PlayerList.Count > 0 && Vector3.Distance(transform.position, PlayerList[0].transform.position) >= attackRange) //si un objet s'ajoute dans la liste et que l'objet n'est pas dans la range d'attaque du gameobject il avance vers le player qui est rentré.
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerList[0].transform.position, Time.deltaTime); //on fais avancer le gameobject vers le player
        }

        if(PlayerList.Count != 0) //Si il y a un joueur qui rentre dans la liste et donc que la liste n'est pas égale à 0
        {
            if(Vector3.Distance(transform.position, PlayerList[0].transform.position) <= attackRange && canAttack == true) // si la distance entre le player et que le gameobject peut attaquer alors on fait l'action
            {
                Debug.Log(transform.name + "Do Damage");
                PlayerList[0].gameObject.GetComponent<PlayerController>().health -= damage; // on retire des pv au premier player dans la liste
                canAttack = false;
            }
        }

        if(PlayerList.Count == 0 && transform.position != originPosition) //? si il n'y à plus de joueur dans la liste, le gameObject retourne à son point de spawn, système pas opti ni finis
        {
            transform.position = Vector3.MoveTowards(transform.position, originPosition, Time.deltaTime);
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            PlayerList.Add(other.GetComponent<PlayerController>().gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            PlayerList.Remove(other.GetComponent<PlayerController>().gameObject);
        }
    }

    
}
