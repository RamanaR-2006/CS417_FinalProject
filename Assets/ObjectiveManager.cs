using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;
    public TextMeshProUGUI objectiveText;

    void Awake()
    {
        Instance = this;
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Farm_Scene")
            SetObjective("Objective: Water the Plants!");
        else if (sceneName == "Farm_Scene_part2")
            SetObjective("Objective: ???");
    }

    public void SetObjective(string text)
    {
        objectiveText.text = text;
    }
}