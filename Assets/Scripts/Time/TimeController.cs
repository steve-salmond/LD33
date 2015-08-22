using UnityEngine;

public class TimeController : Singleton<TimeController>
{

    /** Duration of the day/night cycle, in seconds. */
    public float CycleDuration = 60;

    /** Initial cycle offset, as a fraction. */
    public float CycleOffset = 0.5f;

    /** Return the current day/night cycle fraction (0 = sunrise, 0.25 = noon, 0.5 = sunset, 0.75 = midnight, 1 = sunrise). */
    public static float Now
    { get { return Instance.GetCycleFraction(Time.timeSinceLevelLoad); }}

    /** Whether it's currently daytime. */
    public static bool Daytime
    { get { return Now <= 0.5f; } }

    /** Whether it's currently night time. */
    public static bool Nighttime
    { get { return Now > 0.5f; } }


    /** Returns day/night fraction from an input time. */
    private float GetCycleFraction(float t)
    { return Mathf.Repeat(t + (CycleOffset * CycleDuration), CycleDuration) / CycleDuration; }

}
