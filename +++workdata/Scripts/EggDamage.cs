using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EggDamage : MonoBehaviour
{
    Manager manager;

    public float fadeDuration = 3f;
    public Sprite eggSplosionSprite;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float gravityChangeDuration = 3f;

    private Transform playerTransform;

    private Vector2 eggSpawn;

    private bool Eggsploding = false;


    public Vector2 finalGravity = new Vector2(0, 4);
    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        manager = GameObject.Find("Manager").GetComponent<Manager>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        eggSpawn = gameObject.transform.position;

        DOTween.To(() => rb.gravityScale, x => rb.gravityScale = x, finalGravity.y, gravityChangeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => SpawnEggSplosion());
    }

    private void Update()
    {
        if (gameObject.transform.position.y < (eggSpawn.y + Random.Range(-4, -2)))
        {
            SpawnEggSplosion();
        }
        
    }

    //tud mir Leid Joachim
    private void SpawnEggSplosion()
    {
        if (!Eggsploding)
        {
            manager.inGameSound.PlayOneShot(manager.eggGroundHit);
            manager.inGameSound.pitch = Random.Range((float).7, (float)1.3);
            Eggsploding = true;
            sr.sprite = eggSplosionSprite;
            rb.simulated = false;
            sr.DOFade(0, fadeDuration).OnComplete(() => DestroyItself());
        }

    }

    private void DestroyItself()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("ye");
            other.gameObject.GetComponent<Health>().TakeDamage(40);
            Invoke("DestroyItself", .1f);
        }
    }
}
