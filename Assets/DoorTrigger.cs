using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Scene3";

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}