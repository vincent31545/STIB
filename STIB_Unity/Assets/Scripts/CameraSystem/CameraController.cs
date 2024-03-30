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
    private Vector3 moveInput;
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
        moveInput.z = Mathf.Lerp(moveInput.z, Input.GetKey(KeyCode.Q) ? -1 : (Input.GetKey(KeyCode.E) ? 1 : 0), 10 * Time.deltaTime);

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

        transform.position += (moveInput.z * Vector3.up + moveInput.x * transform.right + moveInput.y * transform.forward) * Time.deltaTime * moveSpeed;
    }
}
