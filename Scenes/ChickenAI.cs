using UnityEngine;
using Pathfinding;

public class ChickenAI : MonoBehaviour
{
    private Animator anim;
    private Seeker seeker;
    private AIPath aiPath;
    private AIDestinationSetter aiDestinationSetter;

    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    public float collisionCheckRadius = 0.1f;
    public LayerMask collisionCheckLayers;
    public float changeDirectionDelay = 1f;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        anim = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        aiDestinationSetter = GetComponent<AIDestinationSetter>();

        currentPatrolIndex = 0;
        aiDestinationSetter.target = patrolPoints[currentPatrolIndex];
    }

    private void Update()
    {
        Vector3 velocity = aiPath.velocity;

        if (aiPath.reachedEndOfPath)
        {
            if (Physics2D.OverlapCircle(transform.position, collisionCheckRadius, collisionCheckLayers))
            {
                Invoke("ChangeDirection", changeDirectionDelay);
            }
            else
            {
                GoToNextPatrolPoint();
            }
        }

        if (velocity.magnitude > 0)
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                if (velocity.x > 0)
                {
                    anim.Play("ChickenWalkRight");
                    Debug.Log("RIGHT");
                }
                else
                {
                    anim.Play("ChickenWalkLeft");
                    Debug.Log("LEFT");
                }
            }
            else
            {
                if (velocity.y > 0)
                {
                    anim.Play("ChickenWalkUp");
                    Debug.Log("UP");
                }
                else
                {
                    anim.Play("ChickenWalkDown");
                    Debug.Log("DOWN");
                }
            }
        }

    }

    private void GoToNextPatrolPoint()
    {
        currentPatrolIndex++;
        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }
        aiDestinationSetter.target = patrolPoints[currentPatrolIndex];
    }

    private void ChangeDirection()
    {
        Vector2 randomDirection = Random.insideUnitCircle;
        GameObject go = new GameObject();
        go.transform.position = (Vector2)transform.position + randomDirection;
        aiDestinationSetter.target = go.transform;
    }
}