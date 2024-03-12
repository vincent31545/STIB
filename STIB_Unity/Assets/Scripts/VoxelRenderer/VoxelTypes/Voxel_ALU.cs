using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_ALU : Voxel {

    public Voxel_ALU(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("ALU");
    }

    public override Color GetVoxelColor() => Color.cyan;
}
