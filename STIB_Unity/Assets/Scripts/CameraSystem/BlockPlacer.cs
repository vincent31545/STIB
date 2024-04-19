using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockPlacer : MonoBehaviour
{
    [Header("Placement")]
    public GameObject selectedDisplayPrefab;
    public GameObject predictionDisplayPrefab;

    [Header("UI")]
    public Transform placeControlParent;
    public ControlOption placeControlPrefab;
    [Space]
    public RectTransform inspectorRT;
    public CanvasGroup inspectorCG;
    public TextMeshProUGUI inspectorItemName;

    private Voxel selectedVoxel;
    private Vector3Int predictedPlacement;
    private GameObject selectedDisplayInstance, predictionDisplayInstance;
    private int blockType;
    private float scrollAccumulation = 0;
    private ControlOption[] placeControls;
    private KeyCode[] numberKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7};

    private void Start() {
        selectedDisplayInstance = Instantiate(selectedDisplayPrefab);
        predictionDisplayInstance = Instantiate(predictionDisplayPrefab);

        placeControls = new ControlOption[numberKeys.Length];
        for (int i = 0; i < placeControls.Length; ++i) {
            if (i == placeControls.Length-1)
                placeControls[0] = Instantiate(placeControlPrefab, placeControlParent);
            else
                placeControls[i+1] = Instantiate(placeControlPrefab, placeControlParent);
        }

        SelectBlockType(0, true);
    }

    private void Update() {
        for(int i = 0; i < numberKeys.Length; ++i)
            if (Input.GetKeyDown(numberKeys[i]))
                SelectBlockType(i);

        // Save
        if (Input.GetKeyDown(KeyCode.O)) {
            WorldManager.SaveGame();
        }
        // Load
        else if (Input.GetKeyDown(KeyCode.P)) {
            WorldManager.LoadGame();
        }
        scrollAccumulation -= Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollAccumulation) > 0.3f) {
            int i = blockType + (scrollAccumulation > 0 ? 1 : -1);
            if (i > 7) i = 0;
            if (i < 0) i = 7;

            SelectBlockType(i);

            scrollAccumulation = 0;
        }

        for (int i = 0; i < placeControls.Length; ++i)
            placeControls[i].Refresh(i);

        // Selection
        Vector3Int normal;
        Voxel v = PhysicsManager.GetVoxelHit(WorldManager.instance.cameraController.transform.position, WorldManager.instance.cameraController.transform.forward, 8, out normal);
        if (v != null) {
            // New Select
            if (selectedVoxel == null || selectedVoxel != v) {
                selectedVoxel = v;
                selectedDisplayInstance.transform.GetChild(0).gameObject.SetActive(!selectedVoxel.invincible);
            }
            selectedDisplayInstance.transform.position = selectedVoxel.position + Vector3.one / 2;
            selectedDisplayInstance.transform.rotation = Quaternion.LookRotation(selectedVoxel.GetWorldDirection());

            predictedPlacement = selectedVoxel.position + normal;
            predictionDisplayInstance.transform.position = predictedPlacement;

            selectedDisplayInstance.SetActive(!Input.GetKey(KeyCode.LeftAlt));
            predictionDisplayInstance.SetActive(!Input.GetKey(KeyCode.LeftAlt));

            // Placing block
            if (Input.GetMouseButtonDown(1)) {
                WorldManager.AddVoxel((VOXEL_TYPE)blockType, predictedPlacement);
            } else if (Input.GetMouseButtonDown(0))
                WorldManager.RemoveVoxel(selectedVoxel);
            // Rotating block
            else if (Input.GetKeyDown(KeyCode.R)) {
                WorldManager.RotateVoxel(selectedVoxel);
            }
            // Turning on signal block
            else if (Input.GetKeyDown(KeyCode.F)) {
                WorldManager.TurnOnVoxel(selectedVoxel);
            }

        }
        else {
            selectedVoxel = null;
            selectedDisplayInstance.SetActive(false);
            predictionDisplayInstance.SetActive(false);
        }

        // Activate inspector UI panel and place it by selected voxel
        if (selectedVoxel != null && !selectedVoxel.invincible) {
            // Set position
            Vector3 voxelPos = selectedVoxel.position + (Vector3.one * 0.5f);
            inspectorRT.position = WorldManager.instance.cameraController.cam.WorldToScreenPoint(voxelPos);

            // Fill UI Info
            inspectorItemName.text = selectedVoxel.type.ToString();

            // Fade in UI
            if (inspectorCG.alpha < 1) inspectorCG.alpha += Time.deltaTime * 15;
        }
        else {
            // Fade out UI
            if (inspectorCG.alpha > 0) inspectorCG.alpha -= Time.deltaTime * 7.5f;
        }
    }

    private void SelectBlockType(int type, bool force = false) {
        blockType = type;
        for (int i = 0; i < placeControls.Length; ++i)
            placeControls[i].active = i == type;
    }
}
