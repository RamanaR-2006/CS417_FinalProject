using TMPro;
using UnityEngine;

public class CoinClicker : MonoBehaviour
{
    public resource resourceManager;
    public float coinAmount = 1f;
    public float cooldownDuration = 2f;
    public TMP_Text cooldownText; // TMP text object near the coin

    private float cooldownRemaining = 0f;

    void Update()
    {
        if (cooldownRemaining > 0f)
        {
            cooldownRemaining -= Time.deltaTime;
            cooldownText.text = "Wait: " + cooldownRemaining.ToString("F1") + "s";
        }
        else
        {
            cooldownText.text = "Grab Scrap!";
        }
    }

    public void OnClick()
    {
        if (cooldownRemaining > 0f) return;
        resourceManager.updateCoins(coinAmount);
        cooldownRemaining = cooldownDuration;
    }
}