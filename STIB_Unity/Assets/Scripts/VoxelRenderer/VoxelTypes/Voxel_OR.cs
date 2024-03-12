using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel_OR : Voxel {

    public Voxel_OR(VOXEL_TYPE _type, Vector3Int _pos, Voxel[] _adjacent) : base(_type, _pos, _adjacent) {
        Debug.Log("OR");
    }

    public override Color GetVoxelColor() => Color.red;
}
