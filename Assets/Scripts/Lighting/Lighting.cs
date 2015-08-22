using UnityEngine;
using System.Collections;

public class Lighting : MonoBehaviour
{

    public Transform Sun;

    public Gradient SunColor;
    public AnimationCurve SunShadow;

    public Gradient AmbientColor;

    public Vector2 AngleLimits;
    private Light _sunlight;

    void Start()
    {
        _sunlight = Sun.GetComponent<Light>();
    }

    void Update()
    {
        var t = TimeController.Now;

        // Set ambient lighting conditions.
        RenderSettings.ambientLight = AmbientColor.Evaluate(t);

        // Set up sunlight direction and color.
        var f = (t < 0.5f) ? t * 2 : 2 - t * 2;
        var angle = Mathf.Lerp(AngleLimits.x, AngleLimits.y, f);
        Sun.rotation = Quaternion.Euler(angle, 90, 0);
        _sunlight.color = SunColor.Evaluate(t);
        _sunlight.shadowStrength = SunShadow.Evaluate(t);
    }
}
