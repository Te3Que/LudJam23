using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Cam : MonoBehaviour
{
    [Header("Player Controll Settings")]
    [SerializeField]
    private float sensX;
    [SerializeField]
    private float sensY;
    [SerializeField]
    private Transform orientation;
    //[SerializeField]
    //private Vector2 deltaInput;

    [SerializeField]
    private GameObject go_InputManager;

    private InputManager inputManager;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        // Makes it so when playing in editor the mouse does not click out of program
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Gets inputManager and binds deltaInput to mousedelta from inputsystem
        inputManager = go_InputManager.GetComponent<InputManager>();

    }

    private void Update()
    {
        // Gets mouse input
        Vector2 deltaInput = inputManager.GetMouseDelta();
        float mouseX = deltaInput.x * Time.fixedDeltaTime * sensX; 
        float mouseY = deltaInput.y * Time.fixedDeltaTime * sensY; 

        yRotation += mouseX;
        xRotation += mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotates the player
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
