using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 10;
    public float shiftSpeed = 20;
    public float upSpeedMultiplier = 1;
    [Space]
    public float lookSensitivity = 10;

    private float moveSpeed;
    private Vector2 lookInput;
    private Vector2 moveInput;
    private Vector3 rotation;

    public Camera cam { get; set; }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();
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
        rotation += (Vector3.up * lookInput.x - Vector3.right * lookInput.y) * Time.deltaTime * lookSensitivity * 40;
        rotation.x = Mathf.Clamp(rotation.x, -85, 85);
        
        // Clamp upwards rotation
        transform.localEulerAngles = rotation;
    }
    private void ProcessMovement() {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = shiftSpeed;

        if (Input.GetKey(KeyCode.Q))
            transform.position += -Vector3.up * Time.deltaTime * moveSpeed * upSpeedMultiplier;
        else if (Input.GetKey(KeyCode.E))
            transform.position += Vector3.up * Time.deltaTime * moveSpeed * upSpeedMultiplier;

        transform.position += (moveInput.x * transform.right + moveInput.y * transform.forward) * Time.deltaTime * moveSpeed;
    }
}
