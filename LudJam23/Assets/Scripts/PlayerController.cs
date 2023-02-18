using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
[Header("Refrences")]
[SerializeField] Transform player_Player;
[SerializeField] Rigidbody rb;
[SerializeField] Transform head;
[SerializeField] Camera player_cam;

[Header("Configuration")]
[SerializeField] float walk_Speed;
[SerializeField] float run_Speed;
[SerializeField] float jump_Force;

[Header("Player Stats")]
[SerializeField] float player_Acurate_Velocity;
[SerializeField] Vector3 player_Acurate_Position;
[SerializeField] int player_Acurate_Health;

void Awake()
{
    rb = GetComponent<Rigidbody>();
}

void FixedUpdate() 
{
    PlayerMovement();
}

void PlayerMovement()
{
    Vector3 newVelocity = Vector3.up * rb.velocity.y;
    float speed = Input.GetKey(KeyCode.LeftShift) ? run_Speed : walk_Speed;
    newVelocity.x = Input.GetAxis("Horizontal") * speed;
    newVelocity.z = Input.GetAxis("Vertical") * speed;

    rb.velocity = newVelocity;
}

void UpdatePlayerStats()
{
    player_Acurate_Position = player_Player.transform.position;
}

}
