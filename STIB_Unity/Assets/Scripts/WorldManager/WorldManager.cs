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

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }
    
    void Start() {
        for (int x = 0; x < 50; x++)
            for (int z = 0; z < 50; z++)
                AddVoxel(VOXEL_TYPE.None, new Vector3Int(x, -1, z));
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

    public static Voxel GetVoxel(int i) { return instance.voxels[i]; }
    public static Voxel GetVoxel(Vector3Int gridPos) {
        for (int i = 0; i < GetVoxelCount(); ++i) {
            if (instance.voxels[i].position == gridPos)
                return instance.voxels[i];
        }
        return null;
    }

    public static void UpdateAllSignals() {
        //for (int i = 0; i < GetVoxelCount(); ++i) {
            //instance.voxels[i].UpdateSignal();
        //}
    }

    public static Voxel AddVoxel(VOXEL_TYPE type, Vector3Int position) {

        // x, -x, y, -y, z, -z
        //var adj = new Voxel[6];

        /*
            HAHAHA REVEL AT THE GLORY OF THIS CODE REID
        */
        /*
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
                adj[index] = instance.voxels[i+2]; 
            } 
            else if (!xdiff && !ydiff && zdiff) {
                int index = ((position.z - instance.voxels[i].position.z) == 1) ? 0 : 1;
                adj[index] = instance.voxels[i+4];               
            }
        }
        */
        Voxel v = new Voxel(type, position);

        instance.voxels.Add(v);
        instance.onAddVoxel?.Invoke();
        return v;
    }
    public static Voxel RemoveVoxel(Voxel v) {
        instance.voxels.Remove(v);
        instance.onRemoveVoxel?.Invoke();
        return v;
    }
}
