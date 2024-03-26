using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 10;
    public float shiftSpeed = 20;
    [Space]
    public float lookSensitivity = 10;

    private float moveSpeed;
    private Vector2 lookInput;
    private Vector2 moveInput;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        moveSpeed = baseSpeed;

        lookInput.x = Input.GetAxisRaw("Mouse X");
        lookInput.y = Input.GetAxisRaw("Mouse Y");
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        ProcessRotation();
        ProcessMovement();
    }

    private void ProcessRotation() {
        // Calculate the up down and left right angle
        transform.eulerAngles += (Vector3.up * lookInput.x - Vector3.right * lookInput.y) * Time.deltaTime * lookSensitivity * 40;

        // float min = -60f;
        // float max 60f;
        // float start = (min + max) * 0.5f - 180;
        // float floor = Mathf.FloorToInt((angle - start) / 360) * 360;

        // Clamp upwards rotation
        transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localEulerAngles.x, -60f, 60f), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
    private void ProcessMovement() {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = shiftSpeed;

        transform.position += (moveInput.x * transform.right + moveInput.y * transform.forward) * Time.deltaTime * moveSpeed;
    }
}
