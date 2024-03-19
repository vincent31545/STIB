using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [Header("Placement")]
    public GameObject selectedDisplayPrefab;
    public GameObject predictionDisplayPrefab;
    
    [Header("UI")]
    public Transform placeControlParent;
    public ControlOption placeControlPrefab;

    private Voxel selectedVoxel;
    private Vector3Int predictedPlacement;
    private GameObject selectedDisplayInstance, predictionDisplayInstance;

    private ControlOption[] placeControls;
    private KeyCode[] numberKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, };

    private int blockType;
    private float removeTimer = -1f;
    private float scrollAccumulation = 0;

    private void Start() {
        selectedDisplayInstance = Instantiate(selectedDisplayPrefab);
        predictionDisplayInstance = Instantiate(predictionDisplayPrefab);

        placeControls = new ControlOption[9];
        for (int i = 0; i < placeControls.Length; ++i) {
            placeControls[i] = Instantiate(placeControlPrefab, placeControlParent);
        }

        SelectBlockType(0, true);
    }

    private void Update() {
        for(int i = 0; i < numberKeys.Length; ++i)
            if (Input.GetKeyDown(numberKeys[i]))
                SelectBlockType(i);

        scrollAccumulation -= Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollAccumulation) > 0.3f) {
            int i = blockType + (scrollAccumulation > 0 ? 1 : -1);
            if (i >= 9) i = 0;

            SelectBlockType(i);

            scrollAccumulation = 0;
        }

        for (int i = 0; i < placeControls.Length; ++i)
            placeControls[i].Refresh();

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
                WorldManager.AddVoxel(VOXEL_TYPE.None, predictedPlacement, blockType);
            }
            else if (Input.GetMouseButtonDown(0)) {
                removeTimer = 0;
            }
            else if (Input.GetMouseButtonUp(0)) {
                if (removeTimer >= 0.25f) {
                    WorldManager.RemoveVoxel(selectedVoxel);
                }
                removeTimer = -1f;
            }

            if (removeTimer != -1) {
                removeTimer += Time.deltaTime;

                if (removeTimer >= 0.25f) {
                    if (!selectedVoxel.invincible) WorldManager.RemoveVoxel(selectedVoxel);
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

    private void SelectBlockType(int type, bool force = false) {
        blockType = type;
        for (int i = 0; i < placeControls.Length; ++i)
            placeControls[i].active = i == type;
    }
}
