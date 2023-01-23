using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Ghoul Enemy"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(40);
            Destroy(gameObject);
        }
    }
}