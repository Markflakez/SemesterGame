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
        //Initializes the deathPoint game object
        deathPoint = new GameObject();

        //Checks if the player hunger is saved in the PlayerPrefs
        if (!PlayerPrefs.HasKey("PLAYER_HUNGER-" + manager.file))
        {
            eggs = 5;
            //Initializes the removed segments for egg material to 20
            eggRemovedSegments = 20;
            eggMat.SetFloat("_RemovedSegments", 20);
        }

        //Checks if the player radiation is saved in the PlayerPrefs
        if (!PlayerPrefs.HasKey("PLAYER_RADIATION-" + manager.file))
        {
            radiationRemovedSegments = 100;
            radiationMat.SetFloat("_RemovedSegments", 100);
        }

        //Checks if the player health is saved in the PlayerPrefs
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
        if (Input.GetKeyUp(KeyCode.V))
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
        if (eggs < 10)
        {
            targetSpeed = 450;
        }
        else if (eggs < 20)
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

        if (manager.postProcessingVolume.profile.TryGet(out manager.chromaticAberration))
        {
            if (speed >= 50)
                manager.chromaticAberration.intensity.value = speed / 2 / increaseTime;
        }
        UpdateRadiation(speed);

        if (speed >= 100 && !died)
        {
            Death();
        }
    }




    //This script starts the radiation increase over time effect
    public IEnumerator RadiationOverTime2()
    {
        //Start time of the coroutine
        float startTime = Time.time;

        //The loop will continue until the speed is greater than the target speed plus 1
        while (speed < targetSpeed + 1)
        {
            //Increments speed towards the target speed over the increaseTime
            speed += (targetSpeed / increaseTime) * Time.deltaTime;

            //Lerps the color of the avatar icon between white and radiation color based on speed
            Color currentColor = Color.Lerp(Color.white, manager.radiationColor, speed * 0.01f);
            manager.AvatarIcon.GetComponent<Image>().color = currentColor;

            //Updates radiation level based on the current speed
            UpdateRadiation(speed);

            //If speed reaches 100 and the player has not died yet, trigger the death function
            if (speed >= 100 && !died)
            {
                Death();
            }

            //Yield control back to the main loop for one frame
            yield return null;
        }
    }

    //This script starts the regeneration of the player's health over time
    public IEnumerator RegerateHealth()
    {
        //Start time of the coroutine
        float startTime = Time.time;
        //Infinite loop to continuously regenerate health
        while (true)
        {
            //Checks if the player's health is below the maximum
            if (health < maxHealth)
            {
                //Increases the player's health over time
                health += (regenerateSpeed / regenerateDuration) * Time.deltaTime;
                //Calls the UpdateHealth function to visually update the player's health
                UpdateHealth();
            }
            //Yield control back to the main loop for one frame
            yield return null;
        }
    }


    //This script increases hunger over time
    public IEnumerator HungerOverTime()
    {
        //Starts an infinite loop 
        while (true)
        {
            hungerStart += Time.time;
            //Checks if the player's eggs are less than or equal to zero
            if (eggs <= 0)
            {
                //If so, reset hungerStart to 0
                hungerStart = 0;
                //Sets the player's eggs to 0
                eggs = 0;
            }
            else
            {
                //Else, reduce the player's eggs by hungerSpeed/hungerDuration * time since last frame
                eggs -= (hungerSpeed / hungerDuration) * Time.deltaTime;
                MyFunction();
                UpdateEggs();
            }
            //Yield control back to the main loop for one frame
            yield return null;
        }
    }

    //Decreases Health and triggers the Damage animation and eventually kills the player if it's zero or under
    public void Damage(int damage)
    {
        //Increments the damageDealt by the given damage
        damageDealt += damage;
        //Decrements the health by the given damage
        avatarIcon.GetComponent<Animator>().Play("Damage");
        // Update the player's visual health
        UpdateHealth();

        //Checks if the player's health is less than or equal to 0
        if (health <= 0)
        {
            //If the player hasn't died yet, call the Death method
            if (!died)
            {
                Death();
            }
        }
        else
        {
            //If the player's health is not 0 or less, call the Hit method
            Hit();
        }
    }

    //Plays the hit animation and sound when the player takes damage
    private void Hit()
    {
        //Checks the idle state of the player and plays the corresponding animation
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

        //Plays a random sound from the hurtSound1, hurtSound2, hurtSound3 sound clips
        AudioClip[] sounds = { manager.hurtSound1, manager.hurtSound2, manager.hurtSound3 };
        int randomIndex = Random.Range(0, sounds.Length);
        manager.inGameSound.PlayOneShot(sounds[randomIndex]);

        //Shakes the player's position
        playerController.gameObject.transform.DOShakePosition((float).25, (float).1, 0, 90, false, true);
    }


    //Invokes death for player
    private void Death()
    {
        //Set the player death status to true
        died = true;

        //Plays the player death animation based on the idle state of the player
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

        //Shakes the player's position 
        playerController.gameObject.transform.DOShakePosition((float).25, (float).1, 0, 90, false, true);

        //Stops checking distance
        StopCheckDistance();

        manager.inputActions.Disable();

        //Calls a method for a blackfade
        BlackFade();

        //Plays acoughing sound
        manager.inGameSound.PlayOneShot(manager.cough);

        //Changes the color filter to the endColor over a duration of 12 seconds with a InOutSine ease
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



    //Updates the visual Radiation Bar and text
    public void UpdateRadiation(float timePased)
    {
        //Updates the text display for the current amount of radiation
        currentRadiationText.text = "";
        currentRadiationText.text = Mathf.RoundToInt(timePased).ToString();

        //Calculates the amount of radiation segments to be removed 
        radiationRemovedSegments = radiationMat.GetFloat("_SegmentCount") / 200 * -timePased;

        //Updates the material to the amount of removed Segments
        radiationMat.SetFloat("_RemovedSegments", (int)radiationRemovedSegments + radiationMat.GetFloat("_SegmentCount"));

        //Calls a function to update the player's health based on the current radiation level
        UpdateHealth();
    }

    public void AddEggs(int eggNumber)
    {
        eggs += eggNumber * 10;
        if (eggs > 105)
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


    //Updates players visual health
    public void UpdateHealth()
    {
        //Sets the current health text to an empty string
        currentHealthText.text = "";

        float restValue = 100 - speed;

        Animator avatar = avatarIcon.GetComponent<Animator>();


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


    // Function to add health to player's current health
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
        //Calls a function to update the player's health
        UpdateHealth();
    }
}
