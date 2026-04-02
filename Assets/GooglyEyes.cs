using UnityEngine;

public class GooglyEyes : MonoBehaviour
{
    [Header("Eye Objects")]
    public Transform leftEye;
    public Transform rightEye;

    [Header("Pupil Objects")]
    public Transform leftPupil;
    public Transform rightPupil;

    [Header("Tracking")]
    public Transform playerController;
    public float lookSpeed = 5f;
    public float pupilRange = 0.3f; // how far the pupil can slide

    [Header("Blinking")]
    public float blinkInterval = 4f;
    public float blinkDuration = 0.15f;

    private float blinkTimer;
    private float blinkRemaining = 0f;
    private Vector3 leftBaseScale;
    private Vector3 rightBaseScale;

    void Start()
    {
        leftBaseScale = leftEye.localScale;
        rightBaseScale = rightEye.localScale;
        blinkTimer = blinkInterval + Random.Range(-1f, 1f);
    }

    void Update()
    {
        // Move pupils toward controller
        if (playerController != null)
        {
            MovePupil(leftEye, leftPupil, playerController.position);
            MovePupil(rightEye, rightPupil, playerController.position);
        }

        // Blinking
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0f)
        {
            blinkRemaining = blinkDuration;
            blinkTimer = blinkInterval + Random.Range(-1f, 1f);
        }

        if (blinkRemaining > 0f)
        {
            blinkRemaining -= Time.deltaTime;
            leftEye.localScale = new Vector3(leftBaseScale.x, 0.05f, leftBaseScale.z);
            rightEye.localScale = new Vector3(rightBaseScale.x, 0.05f, rightBaseScale.z);
        }
        else
        {
            leftEye.localScale = leftBaseScale;
            rightEye.localScale = rightBaseScale;
        }
    }

    void MovePupil(Transform eye, Transform pupil, Vector3 target)
    {
        Vector3 direction = (target - eye.position).normalized;
        Vector3 localDir = eye.InverseTransformDirection(direction);
        Vector3 clampedLocal = Vector3.ClampMagnitude(localDir, 1f) * pupilRange;
        Vector3 goalPos = clampedLocal;
        pupil.localPosition = Vector3.Lerp(pupil.localPosition, goalPos, lookSpeed * Time.deltaTime);
    }
}