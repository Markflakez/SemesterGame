using UnityEngine;

public class UnscaledTime : MonoBehaviour
{
    void Start()
    {
        // Set the time scale of the current GameObject to be unscaled
        Time.timeScale = 1;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}