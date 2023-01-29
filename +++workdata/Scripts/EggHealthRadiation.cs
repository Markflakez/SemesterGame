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
    public bool died = false;

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
    public float maxHealth;
    public float regenerateSpeed;
    public float regenerateDuration;
    public float damageDealt;
    public float eggs;

    public Color startColor;
    public Color endColor;


    public float hungerStartTime;
    public float speed = 0;
    public float targetSpeed = 100;
    public float increaseTime = 100;


    public float endValue = 0;

    public float hungerSpeed;
    public float hungerDuration;

    private float currentValue;
    private float startTime;

    private float hungerStart;


    public float executeEvery = 0.1f;

    public GameObject avatarIcon;
    void Start()
    {
        deathPoint = new GameObject();

        if (!PlayerPrefs.HasKey("PLAYER_HUNGER-" + manager.file))
        {
            eggs = 5;
            eggRemovedSegments = 20;
            eggMat.SetFloat("_RemovedSegments", 20);
        }
        if (!PlayerPrefs.HasKey("PLAYER_RADIATION-" + manager.file))
        {
            radiationRemovedSegments = 100;
            radiationMat.SetFloat("_RemovedSegments", 100);
        }

        if (!PlayerPrefs.HasKey("PLAYER_HEALTH-" + manager.file))
        {
            health = 100;
            removedSegments = 100;
            healthMat.SetFloat("_RemovedSegments", 100);
        }

            
        
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


    public void MyFunction()
    {
        if(eggs < 10)
        {
            targetSpeed = 450;
        }
        else if(eggs < 20)
        {
            targetSpeed = 300;
        }
        else if (eggs < 30)
        {
            targetSpeed = 220;
        }
        else if (eggs < 40)
        {
            targetSpeed = 150;
        }
        else if (eggs < 50)
        {
            targetSpeed = 100;
        }
        else if (eggs < 60)
        {
            targetSpeed = 80;
        }
        else if (eggs < 70)
        {
            targetSpeed = 60;
        }
        else if (eggs < 80)
        {
            targetSpeed = 40;
        }
        else
        {
            targetSpeed = 0;
        }

        if(manager.postProcessingVolume.profile.TryGet(out manager.chromaticAberration)) 
        {
            if(speed >= 50)
            manager.chromaticAberration.intensity.value = speed / 2 / increaseTime;
        }
        UpdateRadiation(speed);

        if (speed >= 100 && !died)
        {
            Death();
        }
    }




    public IEnumerator RadiationOverTime2()
    {
        float startTime = Time.time;
        while (speed < targetSpeed +1)
        {
            speed += (targetSpeed / increaseTime) * Time.deltaTime;
            Color currentColor = Color.Lerp(Color.white, manager.radiationColor, speed * 0.01f);
            manager.AvatarIcon.GetComponent<Image>().color = currentColor;
            UpdateRadiation(speed);

            if (speed >= 100 && !died)
            {
                Death();
            }
            yield return null;
        }
    }

    public IEnumerator RegerateHealth()
    {
        float startTime = Time.time;
        while (true)
        {
            if (health < maxHealth)
            {
                health += (regenerateSpeed / regenerateDuration) * Time.deltaTime;
                UpdateHealth();
            }
            yield return null;
        }
    }


    public IEnumerator HungerOverTime()
    {
        
        while (eggs < hungerDuration + 1)
        {
            hungerStart += Time.time;
            if (eggs <= 0)
            {
                hungerStart = 0;
                eggs = 0;
            }
            else
            {
                eggs -= (hungerSpeed / hungerDuration) * Time.deltaTime;
                MyFunction();
                UpdateEggs();
            }
            yield return null;
        }
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
        

        AudioClip[] sounds = { manager.hurtSound1, manager.hurtSound2, manager.hurtSound3 };
        int randomIndex = Random.Range(0, sounds.Length);
        manager.inGameSound.PlayOneShot(sounds[randomIndex]);


        playerController.gameObject.transform.DOShakePosition((float).25, (float).1, 0, 90, false, true);
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

        playerController.gameObject.transform.DOShakePosition((float).25, (float).1, 0, 90, false, true);
        
        StopCheckDistance();
        manager.inputActions.Disable();

        BlackFade();
        manager.inGameSound.PlayOneShot(manager.cough);
        if (manager.postProcessingVolume.profile.TryGet(out manager.colorAdjustments))
        {
            DOTween.To(() => manager.colorAdjustments.colorFilter.value, x => manager.colorAdjustments.colorFilter.value = x, endColor, 12f).SetEase(Ease.InOutSine);
        }
        
    }


    public void StopCheckDistance()
    {
        DistanceCheck[] scriptObjects = GameObject.FindObjectsOfType<DistanceCheck>();

        foreach (DistanceCheck script in scriptObjects)
        {
            script.DisableScripts();
        }
    }


    public void BlackFade()
    {
        float finalSize = (float)2.5;
        float duration = 6;
        float time = 3;
        manager.inGameSound.PlayOneShot(manager.piano);
        

        CinemachineVirtualCamera vcam = manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>();
        
        DOTween.To(() => manager.blackCircle.GetComponent<Image>().color, x => manager.blackCircle.GetComponent<Image>().color = x, endColor, (float)1.5);
        DOTween.To(() => radiationCircle.GetComponent<Image>().color, x => radiationCircle.GetComponent<Image>().color = x, endColor, (float)1.5);
        DOTween.To(() => eggCircle.GetComponent<Image>().color, x => eggCircle.GetComponent<Image>().color = x, endColor, (float)1.5);

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
        if(eggs > 105)
        {
            eggs = 105;
        }

        UpdateEggs();
    }

    

    public void UpdateEggs()
    {
        eggRemovedSegments = eggMat.GetFloat("_SegmentCount") / 200 * -eggs;
        eggMat.SetFloat("_RemovedSegments", (int)eggRemovedSegments + eggMat.GetFloat("_SegmentCount"));
    }

    public void UpdateHealth()
    {
        currentHealthText.text = "";

        float restValue = 100 - speed;

        Animator avatar = avatarIcon.GetComponent<Animator>();

        

        //Mathf.RoundToInt(startValue) == 1 && avatar.GetCurrentAnimatorStateInfo(0).length > avatar.GetCurrentAnimatorStateInfo(0).normalizedTime
        //if (Mathf.RoundToInt(startValue) > lastRadiation && avatar.GetCurrentAnimatorStateInfo(0).length > avatar.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            //avatar.Play("Radiation");
        }


        if (health <= 0)
        {
            health = 0;
            damageDealt = 100;
            removedSegments = 100;
            currentHealthText.text = 0.ToString();
        }
        else if (health <= restValue) 
        {
            currentHealthText.text = Mathf.RoundToInt(health).ToString();
            damageDealt = 100 - health;
            maxHealthText.text = Mathf.RoundToInt(100 - speed).ToString();
        }
        else if (speed >= 100)
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
            currentHealthText.text = Mathf.RoundToInt(100 - speed).ToString();
            maxHealthText.text = Mathf.RoundToInt(100 - speed).ToString();
            damageDealt = speed;
        }
        
        

        removedSegments = healthMat.GetFloat("_SegmentCount") / 200 * damageDealt;
        healthMat.SetFloat("_RemovedSegments", (int)removedSegments + healthMat.GetFloat("_SegmentCount") * (float).5);
    }


    public void addHealth(int heartHealth)
    {
        if (health >= 100 - speed)
        {
            health = 100 - speed;
            damageDealt = speed;
        }
        else
        {
            health += heartHealth;
            damageDealt = 100 - health;
        }
        UpdateHealth();
    }
}
