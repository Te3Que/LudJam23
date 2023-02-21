using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;
    //  Used sparingly in calulations to add a bit of extra force ;)
    [SerializeField]
    private float forceAddon;
    [SerializeField]
    private float groundDrag;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float airMultiplier;
    private bool readyToJump;

    [Header("Player Movement Crouching")]
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float crouchYScale;
    [SerializeField]
    private float startYScale;


    //  This is until i can fix the Input System problem
    [Header("Temporary keybinds")]
    [SerializeField]
    private KeyCode sprintKey = KeyCode.LeftShift;
    private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Grounding Check")]
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private bool grounded;

    [Header("Slope System")]
    [SerializeField]
    private float maxSlopeAngle;
    private RaycastHit slopeHit;
    [SerializeField]
    private bool exitingSlope;


    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private GameObject go_InputManager;

    private Vector3 moveDirection;
    private Rigidbody player_rb;
    private InputManager inputManager;

    [Header("Player State")]
    [SerializeField]
    private MovementState state;
    [SerializeField]
    private enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        //  Instansiates the different required components
        inputManager = go_InputManager.GetComponent<InputManager>();
        player_rb = GetComponent<Rigidbody>();
        player_rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //  Grounding Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        PlayerMovementInput();
        VelocityControl();
        StateHandler();

        //  Handle Drag on rigidbody
        if (grounded)
        {
            player_rb.drag = groundDrag;
        }
        else
        {
            player_rb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void StateHandler()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Sprinting State
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    //  Gets input from inputmanager
    private void PlayerMovementInput()
    {
        horizontalInput = inputManager.GetPlayerMovement().x;
        verticalInput = inputManager.GetPlayerMovement().y;

        if (inputManager.PlayerJumped() && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            //  creates a cooldown between jumps
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            player_rb.AddForce(Vector3.down * forceAddon, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    
    private void PlayerMove()
    {
        //  Calculates the movement
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            player_rb.AddForce(GetMoveDirOnSlope() * moveSpeed * 20f, ForceMode.Force);
            if (player_rb.velocity.y > 0)
            player_rb.AddForce(Vector3.down * forceAddon, ForceMode.Force);
        }

        if (grounded)
        {
            player_rb.AddForce(moveDirection.normalized * moveSpeed * forceAddon, ForceMode.Force);
        }
        else if (!grounded)
        {
             player_rb.AddForce(moveDirection.normalized * moveSpeed * forceAddon * airMultiplier, ForceMode.Force);
        }

        player_rb.useGravity = !OnSlope();
    }

    private void VelocityControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (player_rb.velocity.magnitude > moveSpeed)
            {
                player_rb.velocity = player_rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(player_rb.velocity.x, 0f, player_rb.velocity.z);

            //  Limits velocity so no one is speedyboi
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                player_rb.velocity = new Vector3(limitedVelocity.x, player_rb.velocity.y, limitedVelocity.z);
            }
        }        
    }

    private void Jump()
    {
        exitingSlope = true;
        // reset the y velocity
        player_rb.velocity = new Vector3(player_rb.velocity.x, 0f, player_rb.velocity.z);
        player_rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetMoveDirOnSlope()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
