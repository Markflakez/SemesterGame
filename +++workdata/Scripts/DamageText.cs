using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshPro text;
    public float duration = 5f;

    private Color initialColor;
    private Color targetColor;
    private float elapsedTime = 0.0f;


    void Start()
    {
        // Save the initial color of the text
        initialColor = text.color;

        // Set the target color of the text (same as initial, but with 0 alpha)
        targetColor = initialColor;
        targetColor.a = 0.0f;


        transform.DOJump(new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y + Random.Range(-2, 2), transform.position.z), (float).3, 3, duration, false);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Smoothly interpolate between the initial and target colors
        text.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
    }
}