using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreeShake : MonoBehaviour
{
    private Quaternion originalRotation;

    private bool isShaking = false;

    Manager manager;

    private void Start()
    {
        originalRotation = transform.localRotation;
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().isMoving)
            {
                if (!isShaking)
                {
                    StartCoroutine(DoShake());
                }
            }

        }
        else if (collision.gameObject.name.Contains("Egg"))
        {
            if (!isShaking)
            {
                StartCoroutine(DoShake());
            }
        }
    }

    private IEnumerator DoShake()
    {
        isShaking = true;
        manager.inGameSound.PlayOneShot(manager.bushSound, .06f);
        float elapsed = 0f;
        while (elapsed < 2)
        {
            float sin = Mathf.Sin(elapsed * Mathf.PI * 10f);
            float zRotation = sin * 8 * (1f - elapsed / 2);
            transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, zRotation);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = originalRotation;
        isShaking = false;
    }
}
