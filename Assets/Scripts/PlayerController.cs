using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool enableAutoFireMode;
    public float autoFireInterval = 0.1f;
    public float autoFireTimer;
    private int currentBulletCount;
    private bool isCooldown;
    private float cooldownTimer;
    public int maxBulletCount = 10;
    public float bulletRegenerateCooldown = 2f;
    // [6] set the range of the player's movement in x-axis
    public float xRange = 10;

    // [8] declare Projectile prefab variable
    public GameObject projectilePrefab;

    private float horizontalInput;

    // [1] declare a private InputAction variable
    private InputAction moveAction;
    // [10] declare a private InputAction variable for shooting
    private InputAction shootAction;

    private void Awake()
    {
        
       currentBulletCount = maxBulletCount;
        
        moveAction = InputSystem.actions.FindAction("Move");

        // [11] find the action by name
        shootAction = InputSystem.actions.FindAction("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        void Update()
        {
            horizontalInput = moveAction.ReadValue<Vector2>().x;
            transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.right);

            if (!enableAutoFireMode && shootAction.triggered)
            {
                Instantiate(projectilePrefab, transform.position, transform.rotation);
            }

            if (enableAutoFireMode)
            {
                autoFireTimer += Time.deltaTime;
                if (autoFireTimer >= autoFireInterval)
                {
                    Instantiate(projectilePrefab, transform.position, transform.rotation);
                    autoFireTimer = 0f;
                }
                // [3] use input system to get horizontal input
                horizontalInput = moveAction.ReadValue<Vector2>().x;

                // [4] move the player
                transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.right);

                // [5] keep the player inbounds
                // if (transform.position.x < -10)
                // {
                //     transform.position = new Vector3(-10, transform.position.y, transform.position.z);
                // }

                // [7] keep the player inbounds using xRange variable
                if (transform.position.x < -xRange)
                {
                    transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
                }
                if (transform.position.x > xRange)
                {
                    transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
                }

                // [12] check if the player is shooting
                if (shootAction.triggered)
                {
                    // [13] spawn a projectile
                    Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
                }
                {
                    horizontalInput = moveAction.ReadValue<Vector2>().x;
                    transform.Translate(horizontalInput * speed * Time.deltaTime * Vector3.right);
                    
                    if (shootAction.triggered && !isCooldown)
                    {
                        if (currentBulletCount > 0)
                        {
                            Instantiate(projectilePrefab, transform.position, transform.rotation);
                            currentBulletCount--;
                           
                            if (currentBulletCount <= 0)
                            {
                                isCooldown = true;
                                cooldownTimer = 0f;
                            }
                        }
                    }
                  
                    if (isCooldown)
                    {
                        cooldownTimer += Time.deltaTime;
                        if (cooldownTimer >= bulletRegenerateCooldown)
                        {
                            currentBulletCount = maxBulletCount;
                            isCooldown = false;
                        }
                    }
                }
            }
        }
    }
}
