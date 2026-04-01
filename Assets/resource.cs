using TMPro;
using UnityEngine;

public class resource : MonoBehaviour
{
    public TMP_Text resourceText;

    public TMP_Text rsrc2Text;

    public GameObject butt;

    public float coins = 0.0f;
    public float rate = 0.5f;

    public float rsrc2 = 0.0f;
    public float rate2 = 0.0f;
    private bool unlocked2 = false;

    private int trophiesSpawned = 0;

    public GameObject trophy;

    public Transform roomRoot;

    void Update()
    {
        // Euler integration
        coins += rate * Time.deltaTime;
        rsrc2 += rate2 * Time.deltaTime;

        resourceText.text = $"Scrap: {FormatResource(coins)} (+{FormatRate(rate)}/s)";

        if (unlocked2) {
            rsrc2Text.text = $"Joy: {FormatResource(rsrc2)} (+{FormatRate(rate2)}/s) \n Hint: Deploying more robots \n might bring you more Joy";
        }

        if (coins >= 10f && trophiesSpawned == 0)
        {
            SpawnTrophy();
        }

        if (coins >= 200f && trophiesSpawned == 1)
        {
            SpawnTrophy();
        }

        if (coins >= 300f && trophiesSpawned == 2)
        {
            SpawnTrophy();
        }
    }

    void SpawnTrophy()
    {
        trophiesSpawned++;
        GameObject spawned = Instantiate(trophy, roomRoot);
        spawned.transform.localPosition = new Vector3(-21f, 5.4f, 7.0f + ((trophiesSpawned - 1) * 4f));
        spawned.transform.localRotation = Quaternion.identity;
        Debug.Log("Built a trophy!: " + trophiesSpawned);
    }

    string FormatResource(float value)
    {
        if (value >= 100f)
            return value.ToString("F0"); // no decimals
        else
            return value.ToString("F1"); // one decimal
    }

    string FormatRate(float value)
    {
        if (value >= 10f)
            return value.ToString("F0"); // no decimals
        else
            return value.ToString("F2"); // one decimal
    }
    public void updateRate(float inc){
        rate += inc;
    }

    public void updateCoins(float delta) {
        coins += delta;
    }
    public bool canBuy(float amount) {
        return coins >= amount;
    }
    public void updateRate2(float inc){
        rate2 += inc;
    }

    public void updateRsrc2(float delta) {
        rsrc2 += delta;
    }
    public void TryUnlock2() {
        if (coins < 50f) {
            return;
        }
        updateCoins(-50f);
        unlocked2 = true;
        butt.SetActive(false);
    }
}