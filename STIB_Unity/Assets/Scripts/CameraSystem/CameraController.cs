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

    [Header("Placement")]
    public GameObject selectedDisplay;
    public GameObject predictionDisplay;

    private float moveSpeed;
    private Vector2 lookInput;
    private Vector2 moveInput;

    private Voxel selectedVoxel;
    private Vector3Int predictedPlacement;

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

        // Selection
        Vector3Int normal;
        selectedVoxel = PhysicsManager.GetVoxelHit(WorldManager.instance.cameraController.transform.position, WorldManager.instance.cameraController.transform.forward, 8, out normal);
        if (selectedVoxel != null) {
            if (!selectedDisplay.activeInHierarchy) {
                selectedDisplay.SetActive(true);
                predictionDisplay.SetActive(true);
            }

            selectedDisplay.transform.position = selectedVoxel.position;

            predictedPlacement = selectedVoxel.position + normal;
            predictionDisplay.transform.position = predictedPlacement;

            if (Input.GetMouseButtonDown(0)) {
                WorldManager.AddVoxel(VOXEL_TYPE.None, predictedPlacement);
            }
        }
        else if (selectedDisplay.activeInHierarchy) {
            selectedDisplay.SetActive(false);
            predictionDisplay.SetActive(false);
        }
    }

    private void ProcessRotation() {
        transform.eulerAngles += (Vector3.up * lookInput.x - Vector3.right * lookInput.y) * Time.deltaTime * lookSensitivity * 40;
    }
    private void ProcessMovement() {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = shiftSpeed;

        transform.position += (moveInput.x * transform.right + moveInput.y * transform.forward) * Time.deltaTime * moveSpeed;
    }
}
