using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class EggHealthRadiation : MonoBehaviour
{

    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentRadiationText;

    public PlayerController playerController;

    public Material eggMat;
    public Material healthMat;
    public Material radiationMat;

    private GameObject deathPoint;
    private bool died = false;

    public Manager manager;

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

    public Color startColor;
    public Color endColor;


    public float startValue = 0;
    public float endValue = 101;
    public float duration = 10;
    public float executeEvery = 0.1f;

    public GameObject avatarIcon;
    void Start()
    {
        deathPoint = new GameObject();

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
            Damage(25);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            addHealth(25);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            AddEggs(3);
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

                if (startValue >= 100)
                {
                    Death();
                }
            });
    }


    public void Damage(int damage)
    {
        damageDealt += damage;
        health -= damage;
        avatarIcon.GetComponent<Animator>().Play("Damage");
        UpdateHealth();


        if (health <= 0)
        {
            if (!died)
            {
                Death();
            }
        }
        else
        {
            Hit();
        }
    }

    private void Hit()
    {
        if (playerController.idleState == 0)
        {
            playerController.GetComponent<Animator>().Play("PlayerHitDown");
        }
        else if (playerController.idleState == 1)
        {
            playerController.GetComponent<Animator>().Play("PlayerHitRight");
        }
        else if (playerController.idleState == 2)
        {
            playerController.GetComponent<Animator>().Play("PlayerHitUp");
        }
        else if (playerController.idleState == 3)
        {
            playerController.GetComponent<Animator>().Play("PlayerHitLeft");
        }
        manager.inGameSound.PlayOneShot(manager.hurtSound);
    }

    private void Death()
    {
        died = true;
        if (playerController.idleState == 0)
        {
            playerController.GetComponent<Animator>().Play("PlayerDeathUp");
        }
        else if (playerController.idleState == 1)
        {
            playerController.GetComponent<Animator>().Play("PlayerDeathRight");
        }
        else if (playerController.idleState == 2)
        {
            playerController.GetComponent<Animator>().Play("PlayerDeathDown");
        }
        else if (playerController.idleState == 3)
        {
            playerController.GetComponent<Animator>().Play("PlayerDeathLeft");
        }

        manager.inputActions.Disable();
        
        Invoke("BlackFade", 1);
        manager.inGameSound.PlayOneShot(manager.cough);
        if (manager.postProcessingVolume.profile.TryGet(out manager.colorAdjustments))
        {
            DOTween.To(() => manager.colorAdjustments.colorFilter.value, x => manager.colorAdjustments.colorFilter.value = x, endColor, 12f).SetEase(Ease.InOutSine);
        }
    }


    public void BlackFade()
    {
        float finalSize = (float)2.5;
        float duration = 6;
        float time = 3;
        manager.inGameSound.PlayOneShot(manager.piano);
        

        CinemachineVirtualCamera vcam = manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>();
        
        DOTween.To(() => manager.blackCircle.GetComponent<Material>().color, x => manager.blackCircle.GetComponent<Material>().color = x, endColor, time);
        DOTween.To(() => healthCircle.GetComponent<Material>().color, x => healthCircle.GetComponent<Material>().color = x, endColor, time);
        DOTween.To(() => radiationCircle.GetComponent<Material>().color, x => radiationCircle.GetComponent<Material>().color = x, endColor, time);
        DOTween.To(() => eggCircle.GetComponent<Material>().color, x => eggCircle.GetComponent<Material>().color = x, endColor, time);

        DOTween.To(() => manager.uiPanel.GetComponent<CanvasGroup>().alpha, x => manager.uiPanel.GetComponent<CanvasGroup>().alpha = x, 0, time);

        DOTween.To(() => vcam.m_Lens.OrthographicSize, x => vcam.m_Lens.OrthographicSize = x, finalSize, duration).SetTarget(vcam.m_Lens).SetEase(Ease.InBack);
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
            removedSegments = 100;
            damageDealt = 100;
            removedSegments = 100;
            currentRadiationText.text = "100";
            currentHealthText.text = "0";
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
