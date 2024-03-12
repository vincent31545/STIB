using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Voxel
{
    public Voxel(VOXEL_TYPE _type, Vector3Int _pos) {
        type = _type;
        position = _pos;
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

    public virtual void UpdateSignal() {}

    public VOXEL_TYPE type;
    public Vector3Int position;
    public Voxel[] adjacent;
    public bool[] signals;
}


public enum VOXEL_TYPE {
    None = -1,
}