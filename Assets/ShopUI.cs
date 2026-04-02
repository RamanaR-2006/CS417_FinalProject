using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Item Names")]
    public string[] itemNames;

    [Header("VR UI")]
    public Transform shopContainer;
    public GameObject rowPrefab;

    [Header("References")]
    shop shopManager;

    private float[] buyCosts;
    private float[] upgradeCosts;
    private GameObject[] rows;

    [Header("Ease Settings")]
    public float easeSpeed = 8f;
    public float punchScale = 1.5f;

    // Buy button ease state
    private float[] currentScales;
    private float[] targetScales;
    private Vector3[] baseScales;

    // Upgrade button ease state
    private float[] upgradeCurrentScales;
    private float[] upgradeTargetScales;
    private Vector3[] upgradeBaseScales;

    void Awake()
    {
        shopManager = GetComponent<shop>();

        buyCosts = new float[itemNames.Length];
        upgradeCosts = new float[itemNames.Length];
        rows = new GameObject[itemNames.Length];
        BuildRows();

        currentScales = new float[itemNames.Length];
        targetScales = new float[itemNames.Length];
        baseScales = new Vector3[itemNames.Length];

        upgradeCurrentScales = new float[itemNames.Length];
        upgradeTargetScales = new float[itemNames.Length];
        upgradeBaseScales = new Vector3[itemNames.Length];

        for (int i = 0; i < itemNames.Length; i++)
        {
            currentScales[i] = 1f;
            targetScales[i] = 1f;
            upgradeCurrentScales[i] = 1f;
            upgradeTargetScales[i] = 1f;

            if (rows[i] != null)
            {
                baseScales[i] = rows[i].transform.Find("BuyButton").localScale;
                upgradeBaseScales[i] = rows[i].transform.Find("UpgradeButton").localScale;
            }
        }
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

            int index = i;
            Button buyBtn = row.transform.Find("BuyButton").GetComponent<Button>();
            buyBtn.onClick.AddListener(() => shopManager.BuyItem(index));
            buyBtn.onClick.AddListener(() => Debug.Log("Buy button clicked: " + index));

            Button upgradeBtn = row.transform.Find("UpgradeButton").GetComponent<Button>();
            upgradeBtn.onClick.AddListener(() => shopManager.UpgradeItem(index));
        }
    }

    public void UpdateDisplay(int id, float buyCost, float upgradeCost, bool punchUpgrade = false)
    {
        buyCosts[id] = buyCost;
        upgradeCosts[id] = upgradeCost;
        RefreshRow(id);

        if (punchUpgrade)
            upgradeTargetScales[id] = punchScale;
        else
            targetScales[id] = punchScale;
    }

    void RefreshRow(int id)
    {
        if (rows[id] == null) return;

        string buyLabel = buyCosts[id] == -1 ? "MAX" : "Buy   " + buyCosts[id] + "s";
        rows[id].transform.Find("BuyButton/Text (TMP)").GetComponent<TMP_Text>().text = buyLabel;
        rows[id].transform.Find("UpgradeButton/Text (TMP)").GetComponent<TMP_Text>().text
            = "Upgrade   " + upgradeCosts[id].ToString("F1") + "s";
    }

    void Update()
    {
        for (int i = 0; i < itemNames.Length; i++)
        {
            if (rows[i] == null) continue;

            // Buy button ease
            float easeDelta = easeSpeed * (targetScales[i] - currentScales[i]) * Time.deltaTime;
            currentScales[i] += easeDelta;
            Transform buyBtn = rows[i].transform.Find("BuyButton");
            buyBtn.localScale = baseScales[i] * currentScales[i];

            if (targetScales[i] > 1f && Mathf.Abs(currentScales[i] - targetScales[i]) < 0.01f)
                targetScales[i] = 1f;

            // Upgrade button ease
            float upgradeDelta = easeSpeed * (upgradeTargetScales[i] - upgradeCurrentScales[i]) * Time.deltaTime;
            upgradeCurrentScales[i] += upgradeDelta;
            Transform upgradeBtn = rows[i].transform.Find("UpgradeButton");
            upgradeBtn.localScale = upgradeBaseScales[i] * upgradeCurrentScales[i];

            if (upgradeTargetScales[i] > 1f && Mathf.Abs(upgradeCurrentScales[i] - upgradeTargetScales[i]) < 0.01f)
                upgradeTargetScales[i] = 1f;
        }
    }
}