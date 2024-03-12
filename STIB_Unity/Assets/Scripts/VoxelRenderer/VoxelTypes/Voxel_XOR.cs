using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_XOR : Voxel {

    public Voxel_XOR(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("XOR");
    }

    public override Color GetVoxelColor() => Color.yellow;
}
