using UnityEngine;

public class FaceColor : MonoBehaviour
{
    public resource resourceManager; // drag your resource object here
    Renderer faceRenderer;

    void Start()
    {
        faceRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float t = Mathf.Clamp01(resourceManager.coins / 2000f);
        Color color = Color.Lerp(Color.red, Color.green, t);
        faceRenderer.material.color = color;
    }
}