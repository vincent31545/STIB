using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [Header("Placement")]
    public GameObject selectedDisplayPrefab;
    public GameObject predictionDisplayPrefab;

    private Voxel selectedVoxel;
    private Vector3Int predictedPlacement;
    private GameObject selectedDisplayInstance, predictionDisplayInstance;

    private float removeTimer = -1f;

    private void Start() {
        selectedDisplayInstance = Instantiate(selectedDisplayPrefab);
        predictionDisplayInstance = Instantiate(predictionDisplayPrefab);
    }

    private void Update() {
        // Selection
        Vector3Int normal;
        Voxel v = PhysicsManager.GetVoxelHit(WorldManager.instance.cameraController.transform.position, WorldManager.instance.cameraController.transform.forward, 8, out normal);
        if (v != null) {
            // New Select
            if (selectedVoxel == null || selectedVoxel != v) {
                selectedVoxel = v;
                selectedDisplayInstance.transform.position = selectedVoxel.position;
            }

            predictedPlacement = selectedVoxel.position + normal;
            predictionDisplayInstance.transform.position = predictedPlacement;

            selectedDisplayInstance.SetActive(!Input.GetKey(KeyCode.LeftAlt));
            predictionDisplayInstance.SetActive(!Input.GetKey(KeyCode.LeftAlt) && removeTimer == -1);

            if (Input.GetMouseButtonDown(1)) {
                WorldManager.AddVoxel(VOXEL_TYPE.None, predictedPlacement);
            }
            else if (Input.GetMouseButtonDown(0)) {
                removeTimer = 0;
            }
            else if (Input.GetMouseButtonUp(0)) {
                if (removeTimer >= 0.25f) {
                    WorldManager.RemoveVoxel(selectedVoxel);
                }
                removeTimer = -1f;
            } else if (Input.GetKeyDown(KeyCode.R)) {
                Debug.Log("ROTATE BLOCK ");
                WorldManager.RotateVoxel(selectedVoxel);
            }

            if (removeTimer != -1) {
                removeTimer += Time.deltaTime;

                if (removeTimer >= 0.25f) {
                    WorldManager.RemoveVoxel(selectedVoxel);
                    removeTimer = 0;
                }
            }
        }
        else {
            selectedVoxel = null;
            selectedDisplayInstance.SetActive(false);
            predictionDisplayInstance.SetActive(false);
        }
    }
}
