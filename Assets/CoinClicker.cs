using TMPro;
using UnityEngine;

public class CoinClicker : MonoBehaviour
{
    public resource resourceManager;
    public float coinAmount = 50f;
    public float cooldownDuration = 2f;
    public TMP_Text cooldownText;

    [Header("Clicker Juice")]
    public AudioSource clickAudioSource;
    public AudioClip clickSoundClip;

    [Header("Cooldown Ease")]
    public float easeSpeed = 8f;
    public float punchScale = 1.5f;

    private float cooldownRemaining = 0f;
    private bool wasCoolingDown = false;
    private Vector3 baseScale;
    private float currentScale = 1f;
    private float targetScale = 1f;

    void Start()
    {
        baseScale = cooldownText.transform.localScale;
    }

    void Update()
    {
        if (cooldownRemaining > 0f)
        {
            cooldownRemaining -= Time.deltaTime;
            cooldownText.text = "Wait: " + cooldownRemaining.ToString("F1") + "s";

            // Cooldown just finished
            if (cooldownRemaining <= 0f)
            {
                cooldownText.text = "Grab Scrap!";
                targetScale = punchScale; // punch when ready again
            }
        }

        // Ease animation
        float easeDelta = easeSpeed * (targetScale - currentScale) * Time.deltaTime;
        currentScale += easeDelta;
        cooldownText.transform.localScale = baseScale * currentScale;

        if (targetScale > 1f && Mathf.Abs(currentScale - targetScale) < 0.01f)
            targetScale = 1f;
    }

    public void OnClick()
    {
        if (cooldownRemaining > 0f) return;
        resourceManager.updateCoins(coinAmount);
        cooldownRemaining = cooldownDuration;

        // Punch on cooldown start
        targetScale = punchScale;

        // Sound
        if (clickAudioSource != null && clickSoundClip != null)
            clickAudioSource.PlayOneShot(clickSoundClip);
    }
}