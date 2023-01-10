using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EggHealthRadiation : MonoBehaviour
{



    public Material eggMat;
    public Material healthMat;
    public Material radiationMat;

    public GameObject eggCircle;
    public GameObject healthCircle;
    public GameObject radiationCircle;

    public TextMeshProUGUI eggText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI radiationText;

    public float removedSegments;
    public float health;
    public float damageDealt;

    void Start()
    {
        if(Application.isEditor)
        {
            health = 100;
        }
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Damage(int damage)
    {
        healthText.GetComponent<Animator>().Play("BarDamage");
        damageDealt += damage;
        health -= damage;
        UpdateHealth();

        if (health <= 0)
        {
            health = 0;
            damageDealt = 100;
            UpdateHealth();
        }

    }


    public void UpdateHealth()
    {
        healthText.text = "";
        healthText.text += health;
        removedSegments = healthMat.GetFloat("_SegmentCount") / 100 * damageDealt;
        healthMat.SetFloat("_RemovedSegments", (int)removedSegments);
    }


    public void addHealth(int heartHealth)
    {
        if (health >= 100)
        {
            health = 100;
            damageDealt = 0;
            removedSegments = 0;
            UpdateHealth();
        }
        else
        {
            damageDealt -= heartHealth;
            health += heartHealth;
            UpdateHealth();
        }
    }
}
