using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Item Names")]
    public string[] itemNames;

    [Header("VR UI")]
    public Transform shopContainer;  // Vertical Layout Group GameObject inside Canvas
    public GameObject rowPrefab;     // Prefab with ItemName, BuyButton, UpgradeButton

    [Header("References")]
    shop shopManager;

    private float[] buyCosts;
    private float[] upgradeCosts;
    private GameObject[] rows;

    void Awake()
    {
        shopManager = GetComponent<shop>();

        buyCosts = new float[itemNames.Length];
        upgradeCosts = new float[itemNames.Length];
        rows = new GameObject[itemNames.Length];
        BuildRows();
    }

    void BuildRows()
    {
        Debug.Log("BuildRows called, itemNames length: " + itemNames.Length);
        for (int i = 0; i < itemNames.Length; i++)
        {
            if (itemNames[i] == null) continue;

            GameObject row = Instantiate(rowPrefab, shopContainer);
            rows[i] = row;

            row.transform.Find("ItemName").GetComponent<TMP_Text>().text = itemNames[i];

            int index = i; // capture for lambda
            Button buyBtn = row.transform.Find("BuyButton").GetComponent<Button>();
            buyBtn.onClick.AddListener(() => shopManager.BuyItem(index));
            buyBtn.onClick.AddListener(() => Debug.Log("Buy button clicked: " + index));

            Button upgradeBtn = row.transform.Find("UpgradeButton").GetComponent<Button>();
            upgradeBtn.onClick.AddListener(() => shopManager.UpgradeItem(index));
        }
    }

    public void UpdateDisplay(int id, float buyCost, float upgradeCost)
    {
        buyCosts[id] = buyCost;
        upgradeCosts[id] = upgradeCost;
        RefreshRow(id);
    }

    void RefreshRow(int id)
    {
        if (rows[id] == null) return;

        string buyLabel = buyCosts[id] == -1 ? "MAX" : "Buy   " + buyCosts[id] + "s";
        rows[id].transform.Find("BuyButton/Text (TMP)").GetComponent<TMP_Text>().text = buyLabel;
        rows[id].transform.Find("UpgradeButton/Text (TMP)").GetComponent<TMP_Text>().text 
            = "Upgrade   " + upgradeCosts[id].ToString("F1") + "s";
    }
}