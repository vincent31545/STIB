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

    public static Voxel AddVoxel(VOXEL_TYPE type, Vector3Int position) {
        Voxel v = new Voxel(type, position);
        instance.voxels.Add(v);
        instance.onAddVoxel?.Invoke();
        return v;
    }
}

/*

GetVoxels list
private voxel list
Static public reference to World Manager
static functions addvoxel removevoxel clearvoxels 
add and removee versions that take vector3 position, grid index vector3
3 delegate voids callbacks called onremove onadd onclear

*/
