using TMPro;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public TMP_Text popupText;
    public float easeSpeed = 6f;
    public float overshootScale = 1.3f;

    private float currentScale = 0f;
    private float targetScale = 0f;
    private bool overshotDone = false;
    private float displayTimer = 0f;

    void Update()
    {
        if (targetScale == 0f && currentScale < 0.01f) return;

        float easeDelta = easeSpeed * (targetScale - currentScale) * Time.deltaTime;
        currentScale += easeDelta;
        popupText.transform.localScale = Vector3.one * currentScale;

        if (!overshotDone && targetScale > 1f && Mathf.Abs(currentScale - targetScale) < 0.05f)
        {
            targetScale = 1f;
            overshotDone = true;
        }

        if (overshotDone)
        {
            displayTimer += Time.deltaTime;
            if (displayTimer > 4f)
            {
                targetScale = 0f;
                if (currentScale < 0.05f)
                {
                    currentScale = 0f;
                    popupText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);
        currentScale = 0f;
        targetScale = overshootScale;
        overshotDone = false;
        displayTimer = 0f;
    }
}