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
    public RectTransform inspectorLineRT;
    public RectTransform inspectorContentRT;
    [Space(5)]
    public CanvasGroup inspectorCG;
    public TextMeshProUGUI inspectorItemName;

    private Voxel selectedVoxel;
    private Vector3Int predictedPlacement;
    private GameObject selectedDisplayInstance, predictionDisplayInstance;
    private int blockType;
    private float scrollAccumulation = 0;
    private ControlOption[] placeControls;
    private KeyCode[] numberKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, };

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
            predictionDisplayInstance.SetActive(!Input.GetKey(KeyCode.LeftAlt));
            
            // Placing block
            if (Input.GetMouseButtonDown(1)) 
                WorldManager.AddVoxel(VOXEL_TYPE.None, predictedPlacement, blockType);
            else if (Input.GetMouseButtonDown(0))
                WorldManager.RemoveVoxel(selectedVoxel);
            // Rotating block
            else if (Input.GetKeyDown(KeyCode.R)) {
                Debug.Log("ROTATE BLOCK ");
                WorldManager.RotateVoxel(selectedVoxel);
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
            inspectorContentRT.position = inspectorRT.position + (Vector3.up + Vector3.right).normalized * 30 * Mathf.Max(0.25f, 10 - Vector3.Distance(WorldManager.instance.cameraController.transform.position, voxelPos));
            // inspectorLineRT.sizeDelta = new Vector2(5, (Vector3.Distance(inspectorRT.position, inspectorContentRT.position) - 45));
            // inspectorLineRT.position = inspectorRT.position + (inspectorContentRT.position - inspectorRT.position).normalized * (inspectorLineRT.sizeDelta.y / 2);

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
