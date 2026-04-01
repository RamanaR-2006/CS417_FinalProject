using UnityEngine;
using UnityEngine.InputSystem;

public class lightswitch : MonoBehaviour
{
    public Light light;
    public InputActionReference action;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();

        action.action.Enable();
        action.action.performed += (ctx) => 
        {
            light.color = new Color(0.5f, 0.7f, 0.7f);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.tabKey.wasPressedThisFrame) {
            light.color = new Color(0.5f, 0.7f, 0.7f);
        }
    }
}

/*
public class lightswitch : MonoBehaviour
{
    public Light light;
    public InputActionReference switchAction; // Assign a controller button in Inspector
    
    void Start()
    {
        light = GetComponent<Light>();
        switchAction.action.Enable();
        switchAction.action.performed += (ctx) => {
            light.color = new Color(0.39f, 0.78f, 0.78f);
        };
    }
}
*/