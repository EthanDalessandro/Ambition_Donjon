using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Ce tableau de GameObjects contiendra les audio clips à jouer lors de différentes actions du joueur (attaque, déplacement, etc.)
    public GameObject[] audioArray;

    // Ces vecteurs stockeront les entrées de mouvement et de regard du joueur
    private Vector2 moveAxis;
    private Vector2 lookAxis;

    // Ces variables seront utilisées pour gérer la rotation de la caméra et la vitesse de saut du joueur
    private float xRotation;
    private float ySpeed;
    private bool hasJumped;

    // Ce Character Controller sera utilisé pour gérer la gravité et les collisions du joueur
    public CharacterController characterController;

    // Ces variables définissent la vitesse de déplacement et de rotation du joueur
    public float moveSpeed;
    public float lookSpeed;

    // Ce Transform représente la caméra du joueur
    public Transform camTransform;

    // Ces variables définissent les limites de rotation de la caméra en X (verticalement)
    public float minXRotation;
    public float maxXRotation;

    // Cette variable définit la vitesse de saut du joueur
    public float jumpSpeed;

    // Ces variables définissent la portée de l'attaque du joueur et la force de recul appliquée aux ennemis touchés
    public float attackRange = 1f;
    public float knockBackForce = 12f;

    // Ces variables définissent les dégâts infligés par l'attaque du joueur et les chances de coup critique
    public float damage = 10;
    float damageCalculatedCrit;
    public float critChance = 10;
    public float critDamage = 100f;

    // Ces variables définissent la vie et la vie maximale du joueur
    public float health = 100f;
    public float maxHealth;

    // Ce Transform représente la direction vers laquelle le joueur regarde (utilisé pour l'attaque)
    public Transform direction;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        moveAxis = callback.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext callback)
    {
        lookAxis = callback.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callback)
    {
        if (callback.started)
            hasJumped = true;
    }



    void Update()
    {
        if (health <= 0)
        {
            // Debug.Log("YOU DIED LOOSER");
        }
        Move();
        Look();
    }


    void Move()
    {
        // Crée un vecteur de direction à partir des entrées de mouvement du joueur (moveAxis)
        Vector3 direction = new Vector3(moveAxis.x, 0f, moveAxis.y);

        // Applique la rotation du joueur au vecteur de direction
        direction = transform.TransformDirection(direction);

        // Multiplie la vitesse de déplacement par le vecteur de direction
        direction *= moveSpeed;

        // Ajoute la gravité à la vitesse verticale du joueur
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // Si le joueur est au sol
        if (characterController.isGrounded)
        {
            // Réinitialise la vitesse verticale du joueur (afin d'éviter qu'il ne "flotte" au sol)
            ySpeed = -0.5f;

            // Si le joueur a sauté
            if (hasJumped)
            {
                // Applique la vitesse de saut au joueur
                ySpeed = jumpSpeed;
            }
        }

        // Ajoute la vitesse verticale au vecteur de direction
        direction.y = ySpeed;
        // direction.y += Physics.gravity.y * Time.deltaTime;

        // Utilise le Character Controller pour déplacer le joueur en fonction du vecteur de direction et du temps écoulé depuis la dernière frame
        characterController.Move(direction * Time.deltaTime);

        // Réinitialise la variable hasJumped
        hasJumped = false;
    }

    public void RightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }
    public void LeftAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }

    void Look()
    {
        // Look
        transform.Rotate(0f, lookAxis.x * lookSpeed, 0f);
        //xRotation = xRotation + lookAxis.y * lookSpeed;
        xRotation += lookAxis.y * lookSpeed;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);
        camTransform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
        //camTransform.Rotate(lookAxis.y * lookSpeed, 0f, 0f);
    }
    void CritialDamage()
    {
        damageCalculatedCrit = damage + (damage * critDamage / 100f);
    }
}
