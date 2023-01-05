using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

	public Rigidbody2D rb;
	public Animator anim;
    public GameObject player;
    public PhysicsMaterial2D material;
    private float movementX, movementY;

    public bool isTalking = false;
    public bool canWalk = true;

    public float knockbackForce = 10f;


    public float idleState;

    // ------------------------------------------------
    // Public variables, visible in Unity Inspector
    // Use these for settings for your script
    // that can be changed easily
    // ------------------------------------------------
    public float forceStrength;     // How fast we move
    public float stopDistance;      // How close we get before moving to next patrol point
    public Vector2[] patrolPoints;  // List of patrol points we will go between

    // ------------------------------------------------
    // Private variables, NOT visible in the Inspector
    // Use these for tracking data while the game
    // is running
    // ------------------------------------------------
    private int currentPoint = 0;

    // ------------------------------------------------
    // Awake is called when the script is loaded
    // ------------------------------------------------
    void Awake()
    {
        // Get the rigidbody that we'll be using for movement
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    public void OnHit()
    {
        Vector2 knockback = transform.position.normalized * knockbackForce;
        Debug.Log("hu");
    }




    // ------------------------------------------------
    // Update is called once per frame
    // ------------------------------------------------
    void Update()
    {
        // How far away are we from the target?
        float distance = (patrolPoints[currentPoint] - (Vector2)transform.position).magnitude;

        // If we are closer to our target than our minimum distance...
        if (distance <= stopDistance)
        {
            // Update to the next target
            currentPoint = currentPoint + 1;

            // If we've gone past the end of our list...
            // (if our current point index is equal or bigger than
            // the length of our list)
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }

        // Now, move in the direction of our target

        // Get the direction
        // Subtract the current position from the target position to get a distance vector
        // Normalise changes it to be length 1, so we can then multiply it by our speed / force
        Vector2 direction = (patrolPoints[currentPoint] - (Vector2)transform.position).normalized;

        // Move in the correct direction with the set force strength
    }



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

        if (canWalk)
        {
            anim.SetFloat("xDirection", movementX);
            anim.SetFloat("yDirection", movementY);
        }
        anim.SetFloat("idleState", idleState);
    }

    /**
	 * Update is called once per frame
	 * 
	 */
    private void FixedUpdate()
    {
        if (rb.velocity.x > 0 && rb.velocity.y < rb.velocity.x)
        {
            Movement(Vector2.right);
        }
        else if (rb.velocity.x < 0 && rb.velocity.y > rb.velocity.x)
        {
            Movement(Vector2.left);
        }
        else if (rb.velocity.y > 0)
        {
            Movement(Vector2.up);
        }
        else if (rb.velocity.y < 0)
        {
            Movement(Vector2.down);
        }
        else
        {
            Movement(Vector2.one);
        }

    }


    public void EnterDialog()
    {
        canWalk = false;
        anim.StopPlayback();
        rb.velocity = Vector3.zero;

        if (this.transform.position.x > player.transform.position.x)
        {
            idleState = 0;
        }
        else if (this.transform.position.x < player.transform.position.x)
        {
            idleState = 2;
        }
        else if (this.transform.position.x > player.transform.position.x)
        {
            idleState = 1;
        }
        else if (this.transform.position.x > player.transform.position.x)
        {
            idleState = 3;
        }
    }
    public void ExitDialog()
    {
        canWalk = true;
    }
}


