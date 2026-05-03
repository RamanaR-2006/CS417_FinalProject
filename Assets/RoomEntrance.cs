using UnityEngine;
using TMPro;

public class RoomEntrance : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private TextMeshPro npcText;
    [SerializeField] private string entranceLine = "Hey! Come here!";

    private void OnTriggerEnter(Collider other)
    {
        if (musicSource != null)
            musicSource.Stop();

        if (npcText != null)
            npcText.text = entranceLine;
    }
}