using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public float distancePerStep = 0.5f; // the distance the object must travel to make a step
    public float stepInterval = 0.5f; // the interval between steps
    public AudioClip[] walkSounds; // the array of walking sounds
    public AudioSource soundPlayer;

    private float lastStepTime = 0.0f; // the time of the last step
    private Vector3 lastPosition; // the position of the object at the last step

    void Start()
    {
        // initialize the last position of the object
        lastPosition = transform.position;
    }

    void Update()
    {
        // calculate the distance traveled since the last step
        float distanceTraveled = Vector3.Distance(transform.position, lastPosition);

        // check if the object has traveled the required distance for a step
        if (distanceTraveled >= distancePerStep)
        {
            // check if the required interval between steps has passed
            if (Time.time - lastStepTime >= stepInterval)
            {
                // choose a random walking sound from the array
                AudioClip walkSound = walkSounds[Random.Range(0, walkSounds.Length)];

                // play the walking sound
                soundPlayer.GetComponent<AudioSource>().clip = walkSound;
                soundPlayer.GetComponent<AudioSource>().PlayOneShot(walkSound, Random.Range((float).4,(float).7));

                // update the time of the last step
                lastStepTime = Time.time;
            }
        }

        // update the last position of the object
        lastPosition = transform.position;
    }
}
