using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniRobot : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionInterval = 1.5f;
    public float scrapReward = 1000f;

    private Vector3 moveDirection;
    private float timer;
    private resource resourceManager;
    private Rigidbody rb;
    private bool grabbed = false;

    public void Init(resource rm)
    {
        resourceManager = rm;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        PickNewDirection();

        // Try to find grab interactable and wire up event
        var interactables = GetComponents<MonoBehaviour>();
        foreach (var mb in interactables)
        {
            var type = mb.GetType();
            var evt = type.GetProperty("selectEntered");
            if (evt != null)
            {
                var selectEvent = evt.GetValue(mb);
                var addMethod = selectEvent.GetType().GetMethod("AddListener");
                var action = new UnityEngine.Events.UnityAction<SelectEnterEventArgs>(OnGrab);
                addMethod.Invoke(selectEvent, new object[] { action });
                Debug.Log("MiniRobot: Successfully wired OnGrab");
                break;
            }
        }
    }

    void FixedUpdate()
    {
        if (!grabbed)
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (grabbed) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
            PickNewDirection();
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;
        timer = changeDirectionInterval;
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("MiniRobot grabbed! Giving " + scrapReward + " scrap");
        grabbed = true;
        if (resourceManager != null)
            resourceManager.updateCoins(scrapReward);

        Destroy(gameObject);
    }
}