using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_OR : Voxel {

    public Voxel_OR(VOXEL_TYPE _type, Vector3Int _pos) : base(_type, _pos) {
        Debug.Log("OR");
    }
}
