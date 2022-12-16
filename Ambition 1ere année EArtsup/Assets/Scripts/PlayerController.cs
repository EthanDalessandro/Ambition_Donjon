using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject[] audioArray;
    private Vector2 moveAxis;
    private Vector2 lookAxis;
    private float xRotation;
    private float ySpeed;
    private bool hasJumped;
    public CharacterController characterController;
    public float moveSpeed;
    public float lookSpeed;
    public Transform camTransform;
    public float minXRotation;
    public float maxXRotation;
    public float jumpSpeed;
    public float attackRange = 1f;
    public float knockBackForce = 12f;
    public float damage = 10;
    float damageCalculatedCrit;
    public float critChance = 10;
    public float critDamage = 100f;
    public float health = 100f;
    public float maxHealth;
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
        Vector3 direction = new Vector3(moveAxis.x, 0f, moveAxis.y);
        direction = transform.TransformDirection(direction);
        direction *= moveSpeed;
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            ySpeed = -0.5f;
            if (hasJumped)
            {
                ySpeed = jumpSpeed;
            }
        }

        direction.y = ySpeed;
        //direction.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(direction * Time.deltaTime);

        hasJumped = false;
    }

    public void RightAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
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
        if(context.performed)
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
