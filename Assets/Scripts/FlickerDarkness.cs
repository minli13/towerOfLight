using UnityEngine;
using UnityEngine.UI;

public class FlickerDarkness : MonoBehaviour
{
    public Image overlay;       // Assign full-screen dark panel
    public float minAlpha = 0.5f; // darkest
    public float maxAlpha = 0.8f; // lightest
    public float flickerSpeed = 0.1f; // seconds between flickers

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Random alpha between min and max
            float a = Random.Range(minAlpha, maxAlpha);
            Color c = overlay.color;
            c.a = a;
            overlay.color = c;

            // Reset timer randomly for uneven flicker
            timer = Random.Range(flickerSpeed / 2f, flickerSpeed * 2f);
        }
    }
}
