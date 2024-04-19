using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class SaveData
{
    public Vector3[] positions;
    public Vector3[] forwards;
    public VOXEL_TYPE[] types;
}

public class WorldManager : MonoBehaviour
{
    public VoxelRenderer voxelRenderer;
    [Space]
    public CameraController cameraController;

    public static WorldManager instance;
    private List<Voxel> voxels = new List<Voxel>();

    public delegate void OnAddVoxel();
    public delegate void OnRemoveVoxel(Voxel v, int index);
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

        for (int x = 0; x < 100; x++)
            for (int z = 0; z < 100; z++) {
                Voxel v = AddVoxel(VOXEL_TYPE.None, new Vector3Int(x  - 50, -1, z - 50), false);
                v.invincible = true;
            }
        instance.onAddVoxel?.Invoke();
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

    public static Voxel AddVoxel(VOXEL_TYPE type, Vector3Int position, bool callback = true) {
        var adj = new Voxel[6];

        Voxel v;
        switch (type) {
            case VOXEL_TYPE.SEND:
                v = new Voxel_SEND(type, position, adj);
                break;
            case VOXEL_TYPE.WIRE:
                v = new Voxel_WIRE(type, position, adj);
                break;
            case VOXEL_TYPE.LED:
                v = new Voxel_LED(type, position, adj);
                break;
            case VOXEL_TYPE.NOT:
                v = new Voxel_NOT(type, position, adj);
                break;
            case VOXEL_TYPE.OR:
                v = new Voxel_OR(type, position, adj);
                break;
            case VOXEL_TYPE.AND:
                v = new Voxel_AND(type, position, adj);
                break;
            case VOXEL_TYPE.SPLIT:
                v = new Voxel_SPLIT(type, position, adj);
                break;
            default:
                v = new Voxel(VOXEL_TYPE.None, position, adj);
                break;
        }
        // Set neighbors
        for (int i = 0; i < GetVoxelCount(); ++i) {
            int xdiff = position.x - instance.voxels[i].position.x;
            int ydiff = position.y - instance.voxels[i].position.y;
            int zdiff = position.z - instance.voxels[i].position.z;

            if (Math.Abs(xdiff) == 1 && ydiff == 0 && zdiff == 0) {
                int index = (xdiff < 0) ? 0 : 1;
                int reverse = (xdiff > 0) ? 0 : 1;
                v.adjacent[index] = instance.voxels[i];
                instance.voxels[i].adjacent[reverse] = v;
            }
            else if (xdiff == 0 && Math.Abs(ydiff) == 1 && zdiff == 0) {
                int index = (ydiff < 0) ? 0 : 1;
                int reverse = (ydiff > 0) ? 0 : 1;
                v.adjacent[index+2] = instance.voxels[i];
                instance.voxels[i].adjacent[reverse+2] = v;
            }
            else if (xdiff == 0 && ydiff == 0 && Math.Abs(zdiff) == 1) {
                int index = (zdiff < 0) ? 0 : 1;
                int reverse = (zdiff > 0) ? 0 : 1;
                v.adjacent[index+4] = instance.voxels[i];
                instance.voxels[i].adjacent[reverse+4] = v;
            }
        }

        instance.voxels.Add(v);
        if (callback) instance.onAddVoxel?.Invoke();
        v.Initialize();
        return v;
    }
    public static Voxel RemoveVoxel(Voxel v) {
        if (v.invincible == true)
            return v;

        // Storing neighbors
        var neighbors = new Voxel[6];
        for (int i = 0; i < v.adjacent.Length; i++) {
            neighbors[i] = v.adjacent[i];
            v.signals[i] = false;
        }

        int index = instance.voxels.IndexOf(v);
        instance.voxels.Remove(v);
        instance.onRemoveVoxel?.Invoke(v, index);

        // Doing it this way bc when I tried to do it not this way
        // C sharp would try to set the reference to null instead
        // of replacing the refernce with a null reference
        for (int i = 0; i < neighbors.Length; i++) {
            int reverse = (i%2 == 1) ? (i - 1) : (i + 1);
            if (neighbors[i] != null) neighbors[i].adjacent[reverse] = null;
        }

        return v;
    }

    public static void RotateVoxel(Voxel v) {
        v.TurnSignalOff(v.forward);
        v.forward++;
        if (v.forward >= 6) v.forward = 0;
    }

    public static void TurnOnVoxel(Voxel v) {
        if (v.type == VOXEL_TYPE.SEND) {
            v.on = !v.on;
        }
    }
    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
                    + "/world.dat"); 
        SaveData data = new SaveData();
        data.positions = positions;
        data.forwards = forwards;
        data.types = types;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
}
