using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    // The force that will be applied to the enemies when they are knocked
    public float force = 10.0f;

    // The range within which the player can knock the enemies
    public float range = 2.5f;

    // The layer on which the enemies are placed
    public LayerMask enemyLayer;

    public Animator cylinder;
    public Animator cylinder001;
    public Animator plane;

    public bool isTyping = false;

    public GameObject GhoulPrefab;
    public GameObject eggThrowablePrefab;
    public Camera mainCamera;

    public InventoryManager inventoryManager;

    private float throwForce = 10;
    private float rotationSpeed = 2f;


    #region Variables
    float movementX, movementY; 
    [SerializeField]
    public float speed;

    [SerializeField]
    public bool isDashing = false;
    public bool isAttacking;
    private bool canDash = true;
    private float dashSpeed = 16;
    private float dashDuration = .125f;

    private float cooldownTime = 1.0f;

    private bool onCooldown = false;

    public PhysicsMaterial2D physicsMaterial2D;

    public LayerMask hardCollisions;

    public float dashDistance = 5;


    public Volume volume;
    private ChromaticAberration chromaticAberration;
    private MotionBlur motionBlur;
    public GameObject ghostEffect;

    public Manager manager;

    private NPCMovement yes;

    public bool isMoving;
    public float idleState = 2;
    public AudioSource stepSoundPlayer;
    public AudioClip stepsounds;


    public BoxCollider2D weaponCollision;

    Vector2 ghoulSpawn;
    public HealthBar healthBar;

    [SerializeField]
    public int playerCurrentHealth;
    private int playerMaxHealth = 100;

    public bool canMove;
    public bool allowDash;
    #endregion

    #region Access
    PlayerControls inputActions;
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    TrailRenderer tr;
    #endregion

    #region OnEnable/Disable
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
    
    public void ToggleTyping()
    {
        canMove = !canMove;
        allowDash = !allowDash;

    }

    private void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        weaponCollision.gameObject.SetActive(false);
        allowDash = true;
    }

    bool IsInRange()
    {
        // Get all the enemies within range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        return enemies.Length > 0;
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.G))
        {
            Instantiate(GhoulPrefab);
        }

        if (Input.GetKey(KeyCode.J))
        {
            cylinder.Play("CylinderAction");
        }

        if (rb.velocity != Vector2.zero)
            isMoving = true;
        else
            isMoving = false;
        if (isMoving)
        {
            if (!stepSoundPlayer.isPlaying)
                stepSoundPlayer.PlayOneShot(stepsounds);
            stepSoundPlayer.pitch = Random.Range(1, 2);
        }
        else
        {
            stepSoundPlayer.Stop();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetMouseButtonDown(1) && !onCooldown)
        {
            Debug.Log("DASH");
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dashDirection = (mousePos - (Vector2)transform.position).normalized;
            Vector2 dashTarget = (Vector2)transform.position + dashDirection * dashDistance;
            transform.DOMove(dashTarget, dashDuration, false).SetEase(Ease.Flash).OnComplete(StopDash);
            onCooldown = true;
            isDashing = true;
            Invoke("ResetCooldown", cooldownTime);
            
        }

        if (isDashing)
        {
            Instantiate(ghostEffect).transform.position = this.transform.position;
        }

    }
    void StopDash()
    {
        isDashing = false;
    }


    void ResetCooldown()
    {
        onCooldown = false;
    }


    private void Awake()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputActions = new PlayerControls();
        anim.SetFloat("idleState", idleState);

        inputActions.Movement.Movement.performed += ctx => Movement(ctx.ReadValue<Vector2>());
        inputActions.Movement.Movement.canceled += ctx => Movement(ctx.ReadValue<Vector2>());
    }

    #region Movement
    void Movement(Vector2 directions)
    {
            movementX = directions.x;
            movementY = directions.y;

            if (movementY == 1)
                idleState = 0;
            else if (movementX == 1)
                idleState = 1;
            else if (movementY == -1)
                idleState = 2;
            else if (movementX == -1)
                idleState = 3;


            anim.SetFloat("idleState", idleState);
            anim.SetFloat("xDirection", movementX);
            anim.SetFloat("yDirection", movementY);
    }
    #endregion


    public void ThrowEgg(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryManager.selectedItemName == "Egg" && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
        {
            // Instantiate the object to throw
            GameObject thrownObject = Instantiate(eggThrowablePrefab, transform.position, Quaternion.identity);

            // Get the direction to the mouse cursor
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePos - (Vector2)transform.position).normalized;

            // Add force to the object in the direction of the cursor
            thrownObject.GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

            // Add rotation to the object
            thrownObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed, ForceMode2D.Impulse);
            Destroy(thrownObject, 3f);
            inventoryManager.RemoveItem(manager.items[inventoryManager.selectedSlot]);
        }
        if (context.performed && inventoryManager.selectedItemName == "Sword" && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
        {
            Attack();
        }
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            inputActions.Enable();
            Vector2 move;
            move = new Vector2(movementX * speed, movementY * speed);
            rb.velocity = move;
        }
        else
        {
            inputActions.Disable();
            Vector2 stop;
            stop = new Vector2(movementX * 0, movementY * 0);
            rb.velocity = stop;
        }
    }

    public void Attack()
    {
        if (!isAttacking && !manager.isPaused && IsInRange())
        {
            anim.Play("PlayerAttackSwordDown");
            // Get all the enemies within range
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

            // Apply a force to each enemy to knock them back
            foreach (Collider2D enemy in enemies)
            {
                Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
                if (enemyRigidbody != null)
                {
                    Vector2 forceDirection = enemyRigidbody.transform.position - transform.position;
                    enemyRigidbody.AddForce(forceDirection.normalized * force, ForceMode2D.Impulse);
                    enemy.gameObject.GetComponent<GhoulEnemy>().OnHit(Random.Range(12, 18));
                }
            }
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "NPC-Laurel2")
        {
            yes = collision.gameObject.GetComponent<NPCMovement>();
        }
    }
}
