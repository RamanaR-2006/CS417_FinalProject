using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections;


public class shop : MonoBehaviour
{
    public ShopItem[] items;
    resource resourceManager;
    ShopUI shopText;

    private GameObject[] spawnedObjects;
    private float[] originalRates;
    private float[] originalRates2;
    private float[] originalUpgradeCosts;

    public Transform roomRoot;

    [Tooltip("Assign the RobotCountDisplay component to track and animate deployed robots.")]
    public RobotCountDisplay robotCountDisplay;

    [Header("Power-up Juice")]
    public float hapticDuration = 0.3f;
    public float hapticAmplitude = 0.7f;
    public ParticleSystem upgradeBurstParticles;

    [Header("Generator Juice")]
    public ParticleSystem generatorBurstParticles;

    void Start()
    {
        resourceManager = GetComponent<resource>();
        shopText = GetComponent<ShopUI>();
        spawnedObjects = new GameObject[items.Length * 10];

        originalRates = new float[items.Length];
        originalRates2 = new float[items.Length];
        originalUpgradeCosts = new float[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            originalRates[i] = items[i].rate;
            originalRates2[i] = items[i].rate2;
            originalUpgradeCosts[i] = items[i].upgradeCost;
            shopText.UpdateDisplay(i, items[i].costs[0], items[i].upgradeCost);
        }
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame) BuyItem(0);
        if (Keyboard.current.sKey.wasPressedThisFrame) BuyItem(1);
        if (Keyboard.current.wKey.wasPressedThisFrame) BuyItem(2);
        if (Keyboard.current.mKey.wasPressedThisFrame) BuyItem(3);
        if (Keyboard.current.lKey.wasPressedThisFrame) BuyItem(4);
        if (Keyboard.current.vKey.wasPressedThisFrame) UpgradeItem(0);
        if (Keyboard.current.dKey.wasPressedThisFrame) UpgradeItem(1);
    }

    IEnumerator HapticBurst()
    {
        for (int i = 0; i < 20; i++)
        {
            UnityEngine.XR.InputDevice rightHand = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            UnityEngine.XR.InputDevice leftHand = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightHand.SendHapticImpulse(0, 1.0f, 0.3f);
            leftHand.SendHapticImpulse(0, 1.0f, 0.3f);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void BuyItem(int id)
    {
        ShopItem item = items[id];
        int count = item.count;
        if (item.count >= item.maxCount)
        {
            Debug.Log(item.itemName + " is maxed out!");
            return;
        }
        bool abletoBuy = resourceManager.canBuy(item.costs[count]);
        if (!abletoBuy) return;

        resourceManager.updateCoins(-1 * item.costs[count]);
        item.count++;
        if (item.prefab != null)
        {
            Transform spawnPoint = item.spawnPoints[count];
            GameObject spawned = Instantiate(item.prefab, roomRoot);
            spawned.transform.position = spawnPoint.position;
            spawned.transform.rotation = spawnPoint.rotation;
            spawnedObjects[id * 3 + count] = spawned;
        }
        Debug.Log("Bought " + item.itemName + " (" + item.count + "/" + item.maxCount + ")");
        resourceManager.updateRate(item.rate);
        resourceManager.updateRate2(item.rate2);

        // Notify the robot counter
        if (robotCountDisplay != null)
            robotCountDisplay.AddRobot();
        bool isMaxed = item.count >= item.maxCount;
        shopText.UpdateDisplay(id, isMaxed ? -1 : items[id].costs[item.count], items[id].upgradeCost);

        // Haptics
        UnityEngine.XR.InputDevice rightHand = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        UnityEngine.XR.InputDevice leftHand = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHand.SendHapticImpulse(0, 1f, 3f);
        leftHand.SendHapticImpulse(0, 1f, 3f);

        // Particle burst
        if (generatorBurstParticles != null)
            generatorBurstParticles.Play();
    }

    public void ResetShop()
    {
        ShopUI shopText = GetComponent<ShopUI>();

        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            if (spawnedObjects[i] != null)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects[i] = null;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i].count = 0;
            items[i].rate = originalRates[i];
            items[i].rate2 = originalRates2[i];
            items[i].upgradeCost = originalUpgradeCosts[i];
        }

        shopText.ResetAllDisplays(items);
    }

    public void UpgradeItem(int id)
    {
        ShopItem item = items[id];
        bool abletoBuy = resourceManager.canBuy(item.upgradeCost);
        if (!abletoBuy) return;

        resourceManager.updateCoins(-1 * item.upgradeCost);

        float oldrate = item.rate;
        item.rate *= 1.25f;
        resourceManager.updateRate(item.rate - oldrate);

        oldrate = item.rate2;
        item.rate2 *= 1.1f;
        resourceManager.updateRate2(item.rate2 - oldrate);

        item.upgradeCost *= 1.33f;

        shopText.UpdateDisplay(id, -1, items[id].upgradeCost, true);

        StartCoroutine(HapticBurst());

        // Particle burst
        if (upgradeBurstParticles != null)
            upgradeBurstParticles.Play();
    }
}

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public GameObject prefab;
    public Transform[] spawnPoints;
    public int[] costs;
    public float upgradeCost = 0.0f;
    public int count = 0;
    public float rate = 0.0f;
    public float rate2 = 0.0f;
    public int maxCount = 3;
}