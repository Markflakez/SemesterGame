using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class EggHealthRadiation : MonoBehaviour
{

    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI eggText;
    public TextMeshProUGUI currentRadiationText;

    public Material eggMat;
    public Material healthMat;
    public Material radiationMat;

    public GameObject eggCircle;
    public GameObject healthCircle;
    public GameObject radiationCircle;

    [HideInInspector]
    public float radiationRemovedSegments;

    [HideInInspector]
    public float removedSegments;
    public float health;
    public float damageDealt;

    public float startValue = 0;
    public float endValue = 101;
    public float duration = 10;
    public float executeEvery = 0.1f;

    void Start()
    {
        health = 100;

        radiationRemovedSegments = 1000;
        removedSegments = 0;

        RadiationOverTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.V))
        {
            Damage(5);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            addHealth(5);
        }
    }

    public void RadiationOverTime()
    {
        float timePassed = 0;
        DOTween.To(() => startValue, x => startValue = x, endValue, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                timePassed += Time.deltaTime;
                if (timePassed >= executeEvery)
                {
                    timePassed = 0;
                    UpdateRadiation(startValue);
                }
            });
    }


    public void Damage(int damage)
    {
        damageDealt += damage;
        health -= damage;
        
        UpdateHealth();
    }

    public void UpdateRadiation(float timePased)
    {
        currentRadiationText.text = "";
        currentRadiationText.text = Mathf.RoundToInt(timePased).ToString();
        radiationRemovedSegments = radiationMat.GetFloat("_SegmentCount") / 200 * -timePased;
        radiationMat.SetFloat("_RemovedSegments", (int)radiationRemovedSegments + radiationMat.GetFloat("_SegmentCount"));
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        currentHealthText.text = "";

        float restValue = 100 - startValue;

        if (health <= 0)
        {
            health = 0;
            damageDealt = 100;
            removedSegments = 1000;
            currentHealthText.text = 0.ToString();
        }
        else if (health <= restValue) 
        {
            currentHealthText.text = Mathf.RoundToInt(health).ToString();
            damageDealt = 100 - health;
            maxHealthText.text = Mathf.RoundToInt(100 - startValue).ToString();
        }
        else if (startValue >= 100)
        {
            health = 0;
            damageDealt = 100;
            removedSegments = 1000;
            maxHealthText.text = health.ToString();
        }
        else
        {
            currentHealthText.text = Mathf.RoundToInt(100 - startValue).ToString();
            maxHealthText.text = Mathf.RoundToInt(100 - startValue).ToString();
            damageDealt = startValue;
        }
        
        

        removedSegments = healthMat.GetFloat("_SegmentCount") / 200 * damageDealt;
        healthMat.SetFloat("_RemovedSegments", (int)removedSegments + healthMat.GetFloat("_SegmentCount") * (float).5);
    }


    public void addHealth(int heartHealth)
    {
        if (health >= 100 - startValue)
        {
            health = 100 - startValue;
            damageDealt = startValue;
        }
        else
        {
            health += heartHealth;
            damageDealt = 100 - health;
        }
        UpdateHealth();
    }
}
