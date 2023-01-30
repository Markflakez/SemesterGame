using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cinemachine;
using TMPro;
public class PlayerController : MonoBehaviour
{

    //The force that will be applied to the enemies when they are knocked
    public float force = 10.0f;

    //The range within which the player can knock the enemies
    public float range = 2.5f;

    public bool isDashing = false;
    public bool canDash = true;

    public bool canSpawnGhost = true;
    public GameObject ghostPrefab;
    public float distance;
    private float distanceTraveled;
    private Vector3 startPosition;

    public Animator cylinder;
    public Animator cylinder001;
    public Animator plane;

    private bool canThrowEgg = true;
    private bool canEatEgg = true;

    public bool isTyping = false;

    public GameObject EnemyPrefab;
    public GameObject eggThrowablePrefab;
    public Camera mainCamera;

    public InventoryManager inventoryManager;

    public Transform enemySpawn;

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

    private bool isWalking = false;
    private bool canDropItem = true;

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

    public bool buildingInRange;

    public bool isMoving;
    public float idleState = 2;
    public AudioSource stepSoundPlayer;
    public AudioClip stepsounds;

    private PlayerCombat playerCombat;

    private GameObject droppedItem;

    public BoxCollider2D weaponCollision;

    Vector2 ghoulSpawn;
    public HealthBar healthBar;

    private Vector2 dropDirection;

    [SerializeField]
    public int playerCurrentHealth;
    private int playerMaxHealth = 100;

    public bool canMove;
    public bool allowDash;
    #endregion

    #region Access
    public Animator anim;
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
        startPosition = transform.position;
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Alpha9))
        {
            Instantiate(EnemyPrefab, null);
        }

        if (Input.GetKey(KeyCode.Alpha8))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            manager.StopAllCoroutines();
            manager.CancelInvoke();
            if (manager.isPaused)
            {
                manager.PauseGame();
            }
            manager.UpdateUIAlpha(1);
            manager.videoPlayer.gameObject.SetActive(false);
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
    }





    private void Awake()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCombat = gameObject.GetComponent<PlayerCombat>();
        anim.SetFloat("idleState", idleState);
    }

    #region Movement
    public void Movement(Vector2 directions)
    {
        movementX = directions.x;
        movementY = directions.y;

        manager.saved = false;
        manager.saveButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;


        manager.closestBuilding = null;
        manager.closestDistanceBuilding = Mathf.Infinity;

        Building[] buildings = FindObjectsOfType<Building>();

        // Iterate through the list of buildings
        foreach (Building building in buildings)
        {
            float distance = Vector2.Distance(manager.player.transform.position, building.transform.position);
            if (distance < manager.closestDistanceBuilding)
            {
                manager.closestBuilding = building.gameObject;
                manager.closestDistanceBuilding = distance;
            }
        }


        NPCdialog[] npcArray = FindObjectsOfType<NPCdialog>();

        // Iterate through the list of NPCs
        foreach (NPCdialog npc in npcArray)
        {
            manager.distanceNPC = Vector2.Distance(manager.player.transform.position, npc.transform.position);
            if (manager.distanceNPC < 3)
            {
                manager.closestNPC = npc.gameObject;
                manager.closestDistanceNPC = manager.distanceNPC;
            }
        }

        if (manager.closestDistanceBuilding < 1 && manager.closestDistanceNPC < manager.closestDistanceBuilding)
        {
            manager.interactControl.GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
        }

        else if(manager.closestDistanceNPC < 3 && manager.closestDistanceNPC < manager.closestDistanceBuilding)
        {
            manager.interactControl.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
        }
        else
        {
            manager.interactControl.GetComponentInChildren<TextMeshProUGUI>().color = manager.uiFontColorDisabled;
        }


        if (movementY == 1)
        {
            idleState = 0;
            playerCombat.colliderPosObj.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        }
        else if (movementX == 1)
        {
            playerCombat.colliderPosObj.transform.position = new Vector3(transform.position.x + 1, transform.position.y + .5f, transform.position.z);
            idleState = 1;
        }
        else if (movementY == -1)
        {
            playerCombat.colliderPosObj.transform.position = new Vector3(transform.position.x, transform.position.y + -.5f, transform.position.z);
            idleState = 2;
        }
        else if (movementX == -1)
        {
            playerCombat.colliderPosObj.transform.position = new Vector3(transform.position.x + -1, transform.position.y + .5f, transform.position.z);
            idleState = 3;
        }

        anim.SetFloat("xDirection", movementX);
        anim.SetFloat("yDirection", movementY);
        anim.SetFloat("idleState", idleState);

        
    }
    #endregion


    public void ItemLMB()
    {
        if (!manager.isPaused)
        {
            if (inventoryManager.selectedItemName == "Egg" && canThrowEgg && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
            {
                GameObject eggSpawn = null;

                if(idleState == 0)
                {
                    eggSpawn = manager.eggBack;
                }
                else if (idleState == 1)
                {
                    eggSpawn = manager.eggRight;
                }
                else if (idleState == 2)
                {
                    eggSpawn = manager.eggFront;
                }
                else if (idleState == 3)
                {
                    eggSpawn = manager.eggLeft;
                }


                //Instantiates the object to throw
                GameObject thrownObject = Instantiate(eggThrowablePrefab, eggSpawn.transform.position, Quaternion.identity);
                if (idleState == 0 || idleState == 3)
                {
                    thrownObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                }
                else
                {
                    thrownObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }

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
                Invoke("EggThrowCoaldown", .125f);
                manager.inGameSound.PlayOneShot(manager.eggThrowSound);
                inventoryManager.CheckSelectedItem();
            }
            else if (inventoryManager.selectedItemName == "Sword" && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
            {
                gameObject.GetComponent<PlayerCombat>().Attack();
                inventoryManager.CheckSelectedItem();
            }
            else
            {
                inventoryManager.CheckSelectedItem();
                return;
            }
        }
    }

    public void UseItem()
    {
        if (!manager.isPaused)
        {
            if (inventoryManager.selectedItemName == "Egg" && canEatEgg && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0 && eggHealthRadiation.eggs < 100)
            {
                inventoryManager.RemoveItem(manager.items[inventoryManager.selectedSlot]);
                eggHealthRadiation.AddEggs(1);
                canEatEgg = false;
                Invoke("EggEatCoaldown", .125f);
            }
            inventoryManager.CheckSelectedItem();
        }
    }

    public void DropItem()
    {
        //Checks if the game is not paused
        if (!manager.isPaused)
        {
            //Checks if the player can drop an item and the selected slot has an item count greater than 0
            if (canDropItem && inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().count > 0)
            {
                //Marks the item as dropped
                inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<InventoryItem>().item.isDropped = true;

                //Checks if the selected item is an egg
                if (inventoryManager.selectedItemName == "Egg")
                {
                    //Spawns a egg prefab
                    GameObject dropped = manager.eggPrefab;
                    StartCoroutine(activateTrigger(dropped));
                }
                //Checks if the selected item is a sword
                else if (inventoryManager.selectedItemName == "Sword")
                {
                    //Spawns a sword prefab
                    GameObject dropped = manager.swordPrefab;
                    StartCoroutine(activateTrigger(dropped));
                }
                //Removes the item that is currently selected from the inventory
                inventoryManager.RemoveItem(manager.items[inventoryManager.selectedSlot]);
                //Prevents the player from dropping another item for a short period of time
                canDropItem = false;
                Invoke("canDropItemCooldown", .125f);
            }
            //Checks if the selected item is still available
            inventoryManager.CheckSelectedItem();
        }
    }

    private IEnumerator activateTrigger(GameObject prefabItem)
    {
        GameObject eggSpawn = null;

        if (idleState == 0)
        {
            eggSpawn = manager.eggBack;
            droppedItem = Instantiate(prefabItem, eggSpawn.transform.position, transform.rotation);
            dropDirection = new Vector2(2, 5);
        }
        else if (idleState == 1)
        {
            eggSpawn = manager.eggRight;
            droppedItem = Instantiate(prefabItem, eggSpawn.transform.position, transform.rotation);
            dropDirection = new Vector2(5, -2);
        }
        else if (idleState == 2)
        {
            eggSpawn = manager.eggFront;
            droppedItem = Instantiate(prefabItem, eggSpawn.transform.position, transform.rotation);
            dropDirection = new Vector2(-2, -5);
        }
        else if (idleState == 3)
        {
            eggSpawn = manager.eggLeft;
            droppedItem = Instantiate(prefabItem, eggSpawn.transform.position, transform.rotation);
            dropDirection = new Vector2(-5, 2);
        }

        if (idleState == 0 || idleState == 3)
        {
            droppedItem.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        else
        {
            droppedItem.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        droppedItem.GetComponent<BoxCollider2D>().enabled = false;
        droppedItem.GetComponent<CircleCollider2D>().enabled = false;

        droppedItem.GetComponent<Rigidbody2D>().AddForce(dropDirection, ForceMode2D.Impulse);

        yield return new WaitForSeconds(2f);
        droppedItem.GetComponent<BoxCollider2D>().enabled = true;
        droppedItem.GetComponent<CircleCollider2D>().enabled = true;
    }

    void EggThrowCoaldown()
    {
        canThrowEgg = true;
    }

    void canDropItemCooldown()
    {
        canDropItem = true;
    }


    void EggEatCoaldown()
    {
        canEatEgg = true;
    }

    public IEnumerator Dash()
    {
        if (!isDashing && canDash)
        {
            manager.inGameSound.PlayOneShot(manager.dashSound);
            isDashing = true;
            canDash = false;
            yield return new WaitForSeconds(.25f);
            isDashing = false;
            yield return new WaitForSeconds(1);
            canDash = true;
        }
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

        //if (isDashing)
        {
        //    Instantiate(ghostEffect).transform.position = this.transform.position;
        }

        if (idleState == 0)
        {
            weaponCollision.transform.rotation = Quaternion.Euler(0, 0, 90);
            if (isDashing)
            {
                rb.AddForce(transform.up * dashSpeed, ForceMode2D.Impulse);
            }

        }
        else if (idleState == 1)
        {
            weaponCollision.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (isDashing)
            {
                rb.AddForce(transform.right * dashSpeed, ForceMode2D.Impulse);
            }
        }
        else if (idleState == 2)
        {
            weaponCollision.transform.rotation = Quaternion.Euler(0, 0, -90);
            if (isDashing)
            {
                rb.AddForce(transform.up * -dashSpeed, ForceMode2D.Impulse);
            }
        }
        else if (idleState == 3)
        {
            weaponCollision.transform.rotation = Quaternion.Euler(0, 0, 180);
            if (isDashing)
            {
                rb.AddForce(transform.right * -dashSpeed, ForceMode2D.Impulse);
            }
        }

        isWalking = true;

        distanceTraveled = Vector3.Distance(transform.position, startPosition);

        if (isDashing)
        {
            anim.speed = 2;
            if (distanceTraveled >= distance)
            {
                InstantiateObject();
                startPosition = transform.position;
            }
        }
        else
        {
            anim.speed = 1;
        }
    }

    void InstantiateObject()
    {
        GameObject newObject = Instantiate(ghostPrefab, transform.position, Quaternion.identity);
        newObject.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
