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
    public float healthSegments;
    public float health;
    public float damageDealt;

    void Start()
    {
        removedSegments = 0;
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
        healthText.text = "";
        healthText.text += health;
        removedSegments = healthSegments / 100 * damageDealt;
        healthMat.SetInt("_RemovedSegments", (int)removedSegments);

        if (health <= 0)
        {
            health = 0;
            damageDealt = 100;
            removedSegments = healthSegments;
            healthText.text = "";
            healthText.text += health;
        }

    }

    public void addHealth(int heartHealth)
    {
        if (health >= 100)
        {
            health = 100;
            damageDealt = 0;
            removedSegments = 0;
            healthText.text = "";
            healthText.text += health;
        }
        else
        {
            damageDealt -= heartHealth;
            health += heartHealth;
            healthText.text = "";
            healthText.text += health;
            removedSegments = healthSegments / 100 * damageDealt;
            healthMat.SetInt("_RemovedSegments", (int)removedSegments);
        }
    }
}
