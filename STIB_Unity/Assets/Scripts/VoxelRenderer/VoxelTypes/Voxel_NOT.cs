using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_NOT : Voxel {

    public Voxel_NOT(VOXEL_TYPE _type, Vector3Int _pos) : base(_type, _pos) {
        Debug.Log("NOT");
    }
}
