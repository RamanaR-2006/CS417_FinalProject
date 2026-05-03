using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.SceneManagement;

public class RoomTriggerObject : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private string room2SceneName = "Room2";

    [Header("Timing")]
    [SerializeField] private float soundDuration = 1.5f;
    [SerializeField] private float holdBlackDuration = 0.5f;

    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;
    private bool triggered = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(TransitionSequence());
    }

    private IEnumerator TransitionSequence()
    {
        // 1. Play loud sound
        if (triggerSound != null)
            audioSource.PlayOneShot(triggerSound);

        yield return new WaitForSeconds(soundDuration);

        // 2. Fade to black
        yield return StartCoroutine(ScreenFader.Instance.FadeToBlack());

        yield return new WaitForSeconds(holdBlackDuration);

        // 3. Load Room 2
        SceneManager.LoadScene(room2SceneName);
    }
}