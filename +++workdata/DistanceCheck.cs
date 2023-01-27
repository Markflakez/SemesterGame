using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    private GameObject player;
    public float maxDistance;
    public MonoBehaviour[] scriptsToDisable;

    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Sprite startSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").gameObject;
        startSprite = sr.sprite;

    }

    private void Start()
    {
        StartCheck();
    }

    public void StopCheck()
    {
        CancelInvoke();
    }

    public void StartCheck()
    {
        InvokeRepeating("EnableDisable", 0f, .5f);
    }

    public void EnableDisable()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance > maxDistance)
        {
            DisableScripts();
        }
        else
        {
            EnableScripts();
        }
    }


    public void DisableScripts()
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            sr.sprite = startSprite;
            if (animator != null)
            {
                animator.enabled = false;
            }
            if (rb != null)
            {
                rb.simulated = false;
            }
            script.enabled = false;
        }
    }

    public void EnableScripts()
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (animator != null)
            {
                animator.enabled = true;
            }
            if (rb != null)
            {
                rb.simulated = true;
            }
            script.enabled = true;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}