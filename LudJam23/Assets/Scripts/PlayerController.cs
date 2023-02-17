using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
[Header("Refrences")]
[SerializeField] Rigidbody rb;
[SerializeField] Transform head;
[SerializeField] Camera player_cam;

[Header("Configuration")]
[SerializeField] float walkSpeed;
[SerializeField] float runSpeed;

void FixedUpdate() 
{
    PlayerMovement();
}

void PlayerMovement()
{
    Vector3 newVelocity = Vector3.up * rb.velocity.y;
    float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    newVelocity.x = Input.GetAxis("Horizontal") * speed;
    newVelocity.z = Input.GetAxis("Vertical") * speed;

    rb.velocity = newVelocity;
}

}
