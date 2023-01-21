using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class EggHealthRadiation : MonoBehaviour
{

    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentRadiationText;

    public Material eggMat;
    public Material healthMat;
    public Material radiationMat;

    public GameObject eggCircle;
    public GameObject healthCircle;
    public GameObject radiationCircle;

    [HideInInspector]
    public float radiationRemovedSegments;
    public float eggRemovedSegments;

    [HideInInspector]
    public float removedSegments;
    public float health;
    public float damageDealt;
    public float eggs;

    public float startValue = 0;
    public float endValue = 101;
    public float duration = 10;
    public float executeEvery = 0.1f;

    public GameObject avatarIcon;
    void Start()
    {
        health = 100;

        radiationRemovedSegments = 100;
        eggRemovedSegments = 20;
        removedSegments = 0;

        eggMat.SetFloat("_RemovedSegments", 20);
        radiationMat.SetFloat("_RemovedSegments", 100);
    }

    //Updates is called once per frame
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

        if (Input.GetKeyUp(KeyCode.B))
        {
            AddEggs(1);
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
        avatarIcon.GetComponent<Animator>().Play("Damage");
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

    public void AddEggs(int eggNumber)
    {
        eggs += eggNumber * 10;
        UpdateEggs();
    }

    

    public void UpdateEggs()
    {
        eggRemovedSegments = eggMat.GetFloat("_SegmentCount") / 200 * -eggs;
        eggMat.SetFloat("_RemovedSegments", (int)eggRemovedSegments + eggMat.GetFloat("_SegmentCount"));
        if(eggs == 100)
        {
            Invoke("ResumeRadiation", 120);
        }
    }

    public void ResumeRadiation()
    {
        DOTween.Play(startValue);
        AddEggs(-1);
    }

    public void UpdateHealth()
    {
        currentHealthText.text = "";

        float restValue = 100 - startValue;

        Animator avatar = avatarIcon.GetComponent<Animator>();

        

        //Mathf.RoundToInt(startValue) == 1 && avatar.GetCurrentAnimatorStateInfo(0).length > avatar.GetCurrentAnimatorStateInfo(0).normalizedTime
        //if (Mathf.RoundToInt(startValue) > lastRadiation && avatar.GetCurrentAnimatorStateInfo(0).length > avatar.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            //avatar.Play("Radiation");
        }


        if (health <= 0)
        {
            DOTween.Kill(startValue,false);
            health = 0;
            damageDealt = 100;
            removedSegments = 100;
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
            removedSegments = 100;
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
