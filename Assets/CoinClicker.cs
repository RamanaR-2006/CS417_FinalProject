using TMPro;
using UnityEngine;

public class CoinClicker : MonoBehaviour
{
    public resource resourceManager;
    public float cooldownDuration = 2f;
    public TMP_Text cooldownText;

    [Header("Spawning")]
    public GameObject miniRobotPrefab;
    public Transform spawnPoint;

    [Header("Clicker Juice")]
    public AudioSource clickAudioSource;
    public AudioClip clickSoundClip;

    [Header("Cooldown Ease")]
    public float easeSpeed = 8f;
    public float punchScale = 1.5f;

    private float cooldownRemaining = 0f;
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

            if (cooldownRemaining <= 0f)
            {
                cooldownText.text = "Spawn Robot!";
                targetScale = punchScale;
            }
        }

        float easeDelta = easeSpeed * (targetScale - currentScale) * Time.deltaTime;
        currentScale += easeDelta;
        cooldownText.transform.localScale = baseScale * currentScale;

        if (targetScale > 1f && Mathf.Abs(currentScale - targetScale) < 0.01f)
            targetScale = 1f;
    }

    public void OnClick()
    {
        if (cooldownRemaining > 0f) return;

        // Spawn mini robot
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;
        GameObject robot = Instantiate(miniRobotPrefab, pos, Quaternion.identity);
        MiniRobot mr = robot.GetComponent<MiniRobot>();
        mr.Init(resourceManager);

        cooldownRemaining = cooldownDuration;
        targetScale = punchScale;

        if (clickAudioSource != null && clickSoundClip != null)
            clickAudioSource.PlayOneShot(clickSoundClip);
    }
}