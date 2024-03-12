using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_AND : Voxel {

    public Voxel_AND(VOXEL_TYPE _type, Vector3Int _pos) : base(_type, _pos) {
        Debug.Log("AND");
    }
}
