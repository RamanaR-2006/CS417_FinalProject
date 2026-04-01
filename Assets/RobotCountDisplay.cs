using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Tracks the total number of robots deployed on the mountain and animates
/// the display with a motion ease whenever a new robot is added.
/// 
/// The easing animation uses an AnimationCurve (configurable in the Inspector)
/// to drive a "punch scale" on the countText transform so the display
/// bounces when the count changes — fulfilling the requirement for
/// motion eases on a visual element that tracks generator count.
/// </summary>
public class RobotCountDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text countText;

    [Header("Ease Animation")]
    [Tooltip("Curve controlling the ease of the punch animation. X = normalized time (0–1), Y = normalized displacement (0–1).")]
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Tooltip("Total duration of the punch animation in seconds.")]
    public float animDuration = 0.6f;

    [Tooltip("Peak scale multiplier at the apex of the punch.")]
    public float peakScale = 1.5f;

    private int robotCount = 0;
    private Coroutine animCoroutine;

    void Start()
    {
        RefreshText();
    }

    /// <summary>
    /// Called by shop.cs each time a robot generator is successfully purchased.
    /// Increments the counter and triggers the eased punch animation.
    /// </summary>
    public void AddRobot()
    {
        robotCount++;
        RefreshText();

        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(PunchScaleEased());
    }

    void RefreshText()
    {
        countText.text = robotCount == 1
            ? "1 Robot Deployed"
            : $"{robotCount} Robots Deployed";
    }

    /// <summary>
    /// Animates the countText transform with an eased "punch" scale:
    ///   • First half  (t 0→0.5): scale rises from 1 → peakScale  (ease-in via curve)
    ///   • Second half (t 0.5→1): scale falls from peakScale → 1   (ease-out via curve)
    /// The AnimationCurve is sampled to provide non-linear (eased) interpolation,
    /// ensuring the motion is never a plain linear lerp.
    /// </summary>
    IEnumerator PunchScaleEased()
    {
        float elapsed = 0f;

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animDuration);

            // Sample the ease curve: maps linear t → eased progress (0–1)
            float easedT = scaleCurve.Evaluate(t);

            // Sin envelope produces a smooth bell: 0 at start/end, 1 at midpoint
            float envelope = Mathf.Sin(easedT * Mathf.PI);

            float scale = 1f + (peakScale - 1f) * envelope;
            countText.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        countText.transform.localScale = Vector3.one;
        animCoroutine = null;
    }
}
