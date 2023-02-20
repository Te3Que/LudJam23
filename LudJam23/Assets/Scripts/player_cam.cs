using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_cam : MonoBehaviour
{
    [Header("Player Controll Settings")]
    [SerializeField]
    private float sensX;
    [SerializeField]
    private float sensY;
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private Vector2 deltaInput;

    private InputManager inputManager;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        // Makes it so when playing in editor the mouse does not click out of program
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Gets inputManager and binds deltaInput to mousedelta from inputsystem
        inputManager = GetComponent<InputManager>();

    }

    private void Update()
    {
        // Gets mouse input
        deltaInput = inputManager.GetMouseDelta();
        Debug.Log(deltaInput.x);
        float mouseX = deltaInput.x * Time.deltaTime * sensX; 
        float mouseY = deltaInput.y * Time.deltaTime * sensY; 

        yRotation += mouseX;
        xRotation += mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotates the player
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
