using UnityEngine;
using UnityEngine.InputSystem;

public class shop : MonoBehaviour
{
    public ShopItem[] items;
    resource resourceManager;
    ShopUI shopText;

    public Transform roomRoot;

    [Tooltip("Assign the RobotCountDisplay component to track and animate deployed robots.")]
    public RobotCountDisplay robotCountDisplay;

    void Start()
    {
        resourceManager = GetComponent<resource>();
        shopText = GetComponent<ShopUI>();

        for (int i = 0; i < items.Length; i++) {
            shopText.UpdateDisplay(i, items[i].costs[0], items[i].upgradeCost);
        }
    }

    void Update()
    {
        #region Keyboard Input (Debug/Testing Only - Remove for final VR build)
        if (Keyboard.current.cKey.wasPressedThisFrame) BuyItem(0);
        if (Keyboard.current.sKey.wasPressedThisFrame) BuyItem(1);
        if (Keyboard.current.wKey.wasPressedThisFrame) BuyItem(2);
        if (Keyboard.current.mKey.wasPressedThisFrame) BuyItem(3);
        if (Keyboard.current.lKey.wasPressedThisFrame) BuyItem(4);
        if (Keyboard.current.vKey.wasPressedThisFrame) UpgradeItem(0);
        if (Keyboard.current.dKey.wasPressedThisFrame) UpgradeItem(1);
        #endregion

        if (Keyboard.current.cKey.wasPressedThisFrame) BuyItem(0);
        if (Keyboard.current.sKey.wasPressedThisFrame) BuyItem(1);
        if (Keyboard.current.wKey.wasPressedThisFrame) BuyItem(2);
        if (Keyboard.current.mKey.wasPressedThisFrame) BuyItem(3);
        if (Keyboard.current.lKey.wasPressedThisFrame) BuyItem(4);
        if (Keyboard.current.vKey.wasPressedThisFrame) UpgradeItem(0);
        if (Keyboard.current.dKey.wasPressedThisFrame) UpgradeItem(1);
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
        }
        Debug.Log("Bought " + item.itemName + " (" + item.count + "/" + item.maxCount + ")");
        resourceManager.updateRate(item.rate);
        resourceManager.updateRate2(item.rate2);

        // Notify the robot counter — triggers the eased animation
        if (robotCountDisplay != null)
            robotCountDisplay.AddRobot();
        bool isMaxed = item.count >= item.maxCount;
        shopText.UpdateDisplay(id, isMaxed ? -1 : items[id].costs[item.count], items[id].upgradeCost);
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

        shopText.UpdateDisplay(id, -1, items[id].upgradeCost); // HARDCODE cuz we only have 1....
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