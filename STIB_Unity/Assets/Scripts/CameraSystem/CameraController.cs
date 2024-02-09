using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 6;
    public float sprintSpeed = 12;

    private float speed;
    private Vector2 input;

    private void Update() {
        // speed = input.GetKey(KeyCode.LeftShift) ? sprintSpeed : cameraSpeed;
        speed = sprintSpeed;

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        input.Normalize();

        transform.position += speed * Time.deltaTime * (input.x * transform.right + input.y * transform.forward);
    }
}
