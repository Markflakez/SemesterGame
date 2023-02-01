using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public Color dayColor;
    public Color nightColor;
    public float dayDuration = 60;
    public float currentTime = 0;



    public Light2D sun;

    void Start()
    {
        sun = GetComponent<Light2D>();
        sun.color = dayColor;
        DOTween.To(() => sun.color, x => sun.color = x, nightColor, dayDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);


        DOTween.To(() => currentTime, x => currentTime = x, dayDuration, dayDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}