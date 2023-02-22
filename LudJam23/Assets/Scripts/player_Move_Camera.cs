using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Move_Camera : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
