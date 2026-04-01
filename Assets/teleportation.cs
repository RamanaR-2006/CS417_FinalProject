using UnityEngine;
using UnityEngine.InputSystem;

public class teleportation : MonoBehaviour
{
    Vector3 basePos  = new Vector3(0, 0, 0);
    Vector3 birdPos = new Vector3(0, 25, -35);
    bool inRoom = true;

    public InputActionReference action;

    //CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        action.action.Enable();
        action.action.performed += (ctx) => 
        {
            if (inRoom) {
                //controller.enabled = false;
                transform.position = birdPos;
                //transform.rotation = Quaternion.Euler(45, 0, 0);
                inRoom = false;
            }
            else {
                //controller.enabled = true;
                transform.position = basePos;
                //transform.rotation = Quaternion.Euler(0, 0, 0);
                inRoom = true;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.pKey.wasPressedThisFrame) { 
        
            if (inRoom) {
                //controller.enabled = false;
                transform.position = birdPos;
                //transform.rotation = Quaternion.Euler(45, 0, 0);
                inRoom = false;
            }
            else {
                //controller.enabled = true;
                transform.position = basePos;
                //transform.rotation = Quaternion.Euler(0, 0, 0);
                inRoom = true;
            }
        }
    }
}
