using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WorldManager : MonoBehaviour
{
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
 
 
    public static List<Voxel> GetVoxels() { return instance.voxels; }

    public static Voxel GetVoxel(int i) { return instance.voxels[i]; }
    public static void AddVoxel(Voxel v) {
        instance.voxels.Add(v);
        instance.onAddVoxel?.Invoke();
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