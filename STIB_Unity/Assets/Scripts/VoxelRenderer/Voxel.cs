using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    public Voxel(VOXEL_TYPE _type, Vector3 _pos) {
        type = _type;
        position = _pos;
    }

    public VOXEL_TYPE type;
    public Vector3 position;
}


public enum VOXEL_TYPE {
    TEST = -1, 
}