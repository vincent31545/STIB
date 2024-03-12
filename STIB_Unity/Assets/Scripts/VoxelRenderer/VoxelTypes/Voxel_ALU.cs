using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_ALU : Voxel {

    public Voxel_ALU(VOXEL_TYPE _type, Vector3Int _pos) : base(_type, _pos) {
        Debug.Log("ALU");
    }
}
