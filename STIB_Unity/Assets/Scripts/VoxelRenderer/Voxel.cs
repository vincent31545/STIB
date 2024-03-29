using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voxel
{
    public Voxel(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) {
        type = _type;
        position = _pos;
        adjacent = new Voxel[6];
        forward = 0;
        for (int i = 0; i < _adjacent.Length; i++) {
            if (_adjacent[i] != null) {
                adjacent[i] = _adjacent[i];
            }
        }
        signals = new bool[6] {false, false, false, false, false, false};
    }
    public virtual void Initialize() {

    }

    public bool RemoveAdjacent(int pos) {

        // Calculating which direction
        if (pos%2 == 1) pos--;
        else pos++;

        // Check that there is actually a block there
        if (adjacent[pos] == null) return false;

        adjacent[pos] = null;
        return true;
    }

    public void SendSignal(bool sig) {
        signals[forward] = sig;
        if (sig) WorldManager.instance.voxelRenderer.SetVoxData(new Vector4(1, 0, 0, 0), WorldManager.GetVoxelIndex(this));
    }

    public virtual void UpdateSignal() { }

    public VOXEL_TYPE type;
    public Vector3Int position;
    public Voxel[] adjacent;
    public bool[] signals;

    public bool invincible = false;

    // x, -x, y, -y, z, -z
    public int forward;

    public virtual Color GetVoxelColor() => Color.white;
}


public enum VOXEL_TYPE {
    None = -1,
    NOT = 0,
    OR = 1,
    AND = 2,
    WIRE = 3,
    SEND = 4,
    LED = 5
}
