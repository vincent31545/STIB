using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    public Voxel(VOXEL_TYPE _type, Vector3Int _pos) {
        type = _type;
        position = _pos;
    }

    public VOXEL_TYPE type;
    public Vector3Int position;
}


public enum VOXEL_TYPE {
    None = -1, 
}