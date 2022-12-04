using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] audioArray;
    public float speed = 4f;
    public float sensitivity = 2f;
    public float jumpForce = 1f;
    public float attackRange = 1f;
    public float knockBackForce = 12f;
    public float damage = 10;
    float damageCalculatedCrit;
    public float critChance = 10;
    public float critDamage = 100f;
    public float health = 100f;
    public float maxHealth;
    public Transform direction;
    public Transform raycastJumpPosition;
    public Rigidbody body;
    bool CanJump = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (health <= 0)
        {
            // Debug.Log("YOU DIED LOOSER");
        }
        Movement();


        if (Input.GetKeyDown(KeyCode.Mouse1)) //Si Clique droit alors...
        {
            RightAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Si clique gauche alors...
        {
            LeftAttack();
        }
    }


    void Movement()
    {
        direction.eulerAngles += new Vector3(Input.GetAxis("Mouse Y") * sensitivity * -1, 0, 0); //Je Bouge la camera Verticalement
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sensitivity); //Je pivote le player sur l'axe Y pour changer son forward en fonction de la souris

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.S))
        {
            transform.Translate((Vector3.forward * Input.GetAxis("Vertical") * speed) * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.D))
        {
            transform.Translate((Vector3.right * Input.GetAxis("Horizontal") * speed) * Time.deltaTime, Space.Self);
        }
        if (CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            CanJump = false;
        }

        Debug.DrawRay(raycastJumpPosition.position, raycastJumpPosition.up * -0.3f, Color.blue); //Dessine un Rayon Bleu

        RaycastHit hitJump; //gameObject que le rayon a toucher

        Ray jumpRay = new Ray(raycastJumpPosition.position, raycastJumpPosition.up * -1); //Construire un Rayon de la position donnée vers une direction donnée

        if (Physics.Raycast(jumpRay, out hitJump, 0.3f))// si le rayon de 0.3f de distance touche un objet alors canJump = true et si il ne touche rien on set le canJump à false
        {
            CanJump = true;
        }
        else
        {
            CanJump = false;
        }
    }

    void RightAttack()
    {
        Debug.DrawRay(direction.position, direction.forward * attackRange, Color.red); //Dessine un Rayon Rouge qui suit les mouvements de la camera
        RaycastHit mouse1_hitObject; //L'objet qui a été toucher par le raycast
        Ray mouse1_Ray = new Ray(direction.position, direction.forward); //j'initie le rayon qui vas apparaitre lors du clique droit

        if (Physics.Raycast(mouse1_Ray, out mouse1_hitObject, attackRange)) //si le rayon du clique droit touche un object dans la range définie
        {

            // Debug.Log(mouse1_hitObject.transform.name + " a été toucher");
            //Si on touche avec le rayon un basic enemy controller alors...
            if (mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>()) //si l'objet toucher comporte le script basicEnemyController alors...
            {

                if (Random.Range(0, 100) <= critChance) //Si je Crit alors...
                {
                    CritialDamage();
                    mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>().health -= damageCalculatedCrit; //on vas retirer 10 de pv à l'objet qui comporte ce script
                    Instantiate(audioArray[1], Vector3.forward, Quaternion.identity);
                    // Debug.Log("Vous avez fait " + damageCalculatedCrit + " de dégâts, il reste " + mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>().health + " PV");
                    mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>().UpdateHealth();
                }
                else // Sinon
                {
                    mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>().health -= damage; //on vas retirer 10 de pv à l'objet qui comporte ce script
                    Instantiate(audioArray[0], Vector3.forward, Quaternion.identity);
                    // Debug.Log("Vous avez fait " + damage + " de dégâts, il reste " + mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>().health + " PV");
                    mouse1_hitObject.transform.GetComponent<BasicEnemy_Controller>().UpdateHealth();
                }

                mouse1_hitObject.transform.GetComponent<Rigidbody>().AddForceAtPosition(mouse1_Ray.direction * knockBackForce, mouse1_hitObject.transform.position, ForceMode.Impulse); //On pousse le gameObject toucher
            }
        }
    }
    void LeftAttack()
    {
        Debug.DrawRay(direction.position, direction.forward * attackRange, Color.blue);
        RaycastHit leftClickHitObject;
        Ray ray = new Ray(direction.position, direction.forward);

        if (Physics.Raycast(ray, out leftClickHitObject, attackRange))
        {
            ChestManager chestManager = leftClickHitObject.transform.GetComponent<ChestManager>();
            if (chestManager)
            {
                if (chestManager.isOpen == false)
                {
                    chestManager.OpenChest();
                    print("Vous avez trouvé un coffre !!");
                }
            }
        }
    }

    void CritialDamage()
    {
        damageCalculatedCrit = damage + (damage * critDamage / 100f);
    }
}
