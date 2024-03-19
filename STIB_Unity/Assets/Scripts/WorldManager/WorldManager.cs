using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldManager : MonoBehaviour
{
    public VoxelRenderer voxelRenderer;
    [Space]
    public CameraController cameraController;

    public static WorldManager instance;
    private List<Voxel> voxels = new List<Voxel>();

    public delegate void OnAddVoxel();
    public delegate void OnRemoveVoxel();
    public delegate void OnClearVoxels();
    private OnAddVoxel onAddVoxel;
    private OnRemoveVoxel onRemoveVoxel;
    private OnClearVoxels onClearVoxels;
    private float counter = 0;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }

    void Start() {
        Application.targetFrameRate = 60;

        for (int x = 0; x < 50; x++)
            for (int z = 0; z < 50; z++) {
                Voxel v = AddVoxel(VOXEL_TYPE.None, new Vector3Int(x, -1, z), -1);
                v.invincible = true;
            }
    }

    void Update() {
        counter += Time.deltaTime;
        if (counter >= (1 / 3)) {
            UpdateAllSignals();
        } 
    }
    
    public static void RegisterAddVoxelEvent(OnAddVoxel a) {
        instance.onAddVoxel += a;
    }
    public static void RegisterRemoveVoxelEvent(OnRemoveVoxel a) {
        instance.onRemoveVoxel += a;
    }
    public static void RegisterClearVoxelsEvent(OnClearVoxels a) {
        instance.onClearVoxels += a;
    }

    public static Vector3Int GetGridPos(Vector3 position) {
        return new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
    }
 
 
    public static int GetVoxelCount() { return instance.voxels.Count; }
    public static int GetVoxelIndex(Voxel v) { return instance.voxels.IndexOf(v); }

    public static Voxel GetVoxel(int i) { return instance.voxels[i]; }
    public static Voxel GetVoxel(Vector3Int gridPos) {
        for (int i = 0; i < GetVoxelCount(); ++i) {
            if (instance.voxels[i].position == gridPos)
                return instance.voxels[i];
        }
        return null;
    }

    private void UpdateAllSignals() {
        for (int i = 0; i < GetVoxelCount(); ++i) {
            instance.voxels[i].UpdateSignal();
        }
    }

    public static Voxel AddVoxel(VOXEL_TYPE type, Vector3Int position, int blockType) {
        var adj = new Voxel[6];

        // Set neighbors
        for (int i = 0; i < GetVoxelCount(); ++i) {
            bool xdiff = Math.Abs(position.x - instance.voxels[i].position.x) == 1;
            bool ydiff = Math.Abs(position.y - instance.voxels[i].position.y) == 1;
            bool zdiff = Math.Abs(position.z - instance.voxels[i].position.z) == 1;
            if (xdiff && !ydiff && !zdiff) {
                int index = ((position.x - instance.voxels[i].position.x) == 1) ? 0 : 1;
                adj[index] = instance.voxels[i]; 
            } 
            else if (!xdiff && ydiff && !zdiff) {
                int index = ((position.y - instance.voxels[i].position.y) == 1) ? 0 : 1;
                adj[index+2] = instance.voxels[i]; 
            } 
            else if (!xdiff && !ydiff && zdiff) {
                int index = ((position.z - instance.voxels[i].position.z) == 1) ? 0 : 1;
                adj[index+4] = instance.voxels[i];               
            }
        }

        Voxel v;
        switch (blockType) {
            case 0:
                v = new Voxel_SEND(type, position, adj);
                break;
            case 1:
                v = new Voxel_OR(type, position, adj);
                break;
            case 2:
                v = new Voxel_AND(type, position, adj);
                break;
            case 3:
                v = new Voxel_WIRE(type, position, adj);
                break;
            case 4:
                v = new Voxel_XAND(type, position, adj);
                break;
            case 5:
                v = new Voxel_ALU(type, position, adj);
                break;
            default:
                v = new Voxel(type, position, adj);
                break;
        }

        instance.voxels.Add(v);
        instance.onAddVoxel?.Invoke();
        v.Initialize();
        return v;
    }
    public static Voxel RemoveVoxel(Voxel v) {
        instance.voxels.Remove(v);
        instance.onRemoveVoxel?.Invoke();
        return v;
    }
}
