using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.SceneManagement;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshPro dialogueText;
    [SerializeField] private string[] dialogueLines;

    private int currentLine = -1;
    private XRSimpleInteractable interactable;

    [SerializeField] private string room4SceneName = "Junk_Sim_V0";

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnInteract);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnInteract);
    }

    private void Start()
    {
            dialogueText.text = "...";
    }

    private void OnInteract(SelectEnterEventArgs args)
    {
        currentLine = (currentLine + 1) % dialogueLines.Length;
        dialogueText.text = dialogueLines[currentLine];

        if (currentLine == 5) {
            SceneManager.LoadScene(room4SceneName);
        }
    }
}