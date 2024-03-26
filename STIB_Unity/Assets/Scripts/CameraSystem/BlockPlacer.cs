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

<<<<<<< Updated upstream
    private float removeTimer = -1f;
=======
    private ControlOption[] placeControls;
    private KeyCode[] numberKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, };

    private int blockType;
    private float scrollAccumulation = 0;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
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
=======
            predictionDisplayInstance.SetActive(!Input.GetKey(KeyCode.LeftAlt));
            
            // Placing block
            if (Input.GetMouseButtonDown(1)) 
                WorldManager.AddVoxel(VOXEL_TYPE.None, predictedPlacement, blockType);
            else if (Input.GetMouseButtonDown(0))
                WorldManager.RemoveVoxel(selectedVoxel);
            // Rotating block
            else if (Input.GetKeyDown(KeyCode.R)) {
>>>>>>> Stashed changes
                Debug.Log("ROTATE BLOCK ");
                WorldManager.RotateVoxel(selectedVoxel);
            }

<<<<<<< Updated upstream
            if (removeTimer != -1) {
                removeTimer += Time.deltaTime;

                if (removeTimer >= 0.25f) {
                    WorldManager.RemoveVoxel(selectedVoxel);
                    removeTimer = 0;
                }
            }
=======
            
>>>>>>> Stashed changes
        }
        else {
            selectedVoxel = null;
            selectedDisplayInstance.SetActive(false);
            predictionDisplayInstance.SetActive(false);
        }
    }
}
