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

    //The force that will be applied to the enemies when they are knocked
    public float force = 10.0f;

    //The range within which the player can knock the enemies
    public float range = 2.5f;

    public LayerMask enemyLayer;

    public Animator cylinder;
    public Animator cylinder001;
    public Animator plane;

    private bool canThrowEgg = true;

    public bool isTyping = false;

    public GameObject GhoulPrefab;
    public GameObject eggThrowablePrefab;
    public Camera mainCamera;

    public InventoryManager inventoryManager;

    public EggHealthRadiation eggHealthRadiation;

    private float throwForce = 10;
    private float rotationSpeed = 2f;

    public Transform eggThrowSpawn;

    #region Variables
    float movementX, movementY; 
    [SerializeField]
    public float speed;

    [SerializeField]
    public bool isAttacking;


    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    private float lastDash;
    private Vector2 dashVelocity;

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
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    TrailRenderer tr;
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
        //Gets all the enemies within range
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && !onCooldown)
        {
            
            
        }

    }



    public void PerformDash()
    {
        Debug.Log("ye");
        if (idleState == 1)
        {
            // set x direction to 0 and y direction to 1
            dashVelocity = new Vector2(0f, dashSpeed);
        }
        else if (idleState == 2)
        {
            // set x direction to 0 and y direction to -1
            dashVelocity = new Vector2(0f, -dashSpeed);
        }
        else if (idleState == 3)
        {
            // set x direction to -1 and y direction to 0
            dashVelocity = new Vector2(-dashSpeed, 0f);
        }
        else if (idleState == 4)
        {
            // set x direction to 1 and y direction to 0
            dashVelocity = new Vector2(dashSpeed, 0f);
        }

        rb.velocity = dashVelocity;
        Invoke("StopDash", dashDuration);
    }

    private void StopDash()
    {
        rb.velocity = Vector2.zero;
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
        anim.SetFloat("idleState", idleState);
    }

    #region Movement
    public void Movement(Vector2 directions)
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


    public void ItemLMB()
    {
        if (!manager.isPaused)
        {
            if (inventoryManager.selectedItemName == "Egg" && canThrowEgg && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
            {
                //Instantiates the object to throw
                GameObject thrownObject = Instantiate(eggThrowablePrefab, eggThrowSpawn.position, Quaternion.identity);

                //Gets the direction to the mouse cursor
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 throwDirection = (mousePos - (Vector2)transform.position).normalized;

                //Adds force to the object in the direction of the cursor
                thrownObject.GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

                //Adds rotation to the object
                thrownObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed, ForceMode2D.Impulse);
                Destroy(thrownObject, 3f);
                inventoryManager.RemoveItem(manager.items[inventoryManager.selectedSlot]);
                canThrowEgg = false;
                Invoke("EggThrowCoaldown", .5f);
            }
            if (inventoryManager.selectedItemName == "Sword" && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
            {
                gameObject.GetComponent<PlayerCombat>().Attack();
            }
            inventoryManager.CheckSelectedItem();
        }
    }

    public void ItemRMB()
    {
        if (!manager.isPaused)
        {
            if (inventoryManager.selectedItemName == "Egg" && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
            {
                inventoryManager.RemoveItem(manager.items[inventoryManager.selectedSlot]);
                eggHealthRadiation.AddEggs(1);
            }
            inventoryManager.CheckSelectedItem();
        }
    }


    void EggThrowCoaldown()
    {
        canThrowEgg = true;
    }




    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 move;
            move = new Vector2(movementX * speed, movementY * speed);
            rb.velocity = move;
        }
        else
        {
            Vector2 stop;
            stop = new Vector2(movementX * 0, movementY * 0);
            rb.velocity = stop;
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
